using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryScreenManager : MonoBehaviour
{
    public GameObject professorInfoPrefab;  // ProfessorInfo ������ ����
    private List<GameObject> professorInfoObjects = new List<GameObject>();  // ������ �����յ��� ������ ����Ʈ

    void OnEnable()
    {
        // ProfessorInfo ��ũ��Ʈ���� ���� ���� ����Ʈ ����
        ProfessorInfo professorInfoScript = FindObjectOfType<ProfessorInfo>();

        if (professorInfoScript != null)
        {
            // ���� ���� ����Ʈ ��������
            List<ProfessorInfo.Professor> hiredProfessors = ProfessorInfo.hiredProfessors;

            // SummaryScreen�� Content ����
            GameObject content = GameObject.Find("SummaryScreen/Scroll View1/Viewport/Content");

            if (content != null)
            {
                // ������ ������ �����յ� ����
                foreach (var obj in professorInfoObjects)
                {
                    Destroy(obj);
                }
                professorInfoObjects.Clear();

                // Content�� ���̸� ���� ���� ���� ���� ����
                RectTransform contentRect = content.GetComponent<RectTransform>();
                contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, hiredProfessors.Count * 50);

                // ���� ���� ����ŭ ProfessorInfo ������ ���� �� UI ������Ʈ
                for (int i = 0; i < hiredProfessors.Count; i++)
                {
                    // ProfessorInfo ������ ����
                    GameObject professorInfo = Instantiate(professorInfoPrefab, content.transform);

                    // ProfessorInfo ��ġ ���� (y�� �������� �Ʒ��� ����)
                    RectTransform professorRect = professorInfo.GetComponent<RectTransform>();
                    professorRect.anchoredPosition = new Vector2(professorRect.anchoredPosition.x, -i * 50);

                    // �ؽ�Ʈ ������Ʈ �����ͼ� ���� ������Ʈ
                    Text professorText = professorInfo.GetComponent<Text>();
                    professorText.text = $"{hiredProfessors[i].name} / {hiredProfessors[i].kind} / {hiredProfessors[i].department}";

                    // ������ �������� ����Ʈ�� �߰�
                    professorInfoObjects.Add(professorInfo);
                }
            }
            else
            {
                Debug.LogWarning("SummaryScreen�� Content�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("ProfessorInfo ��ũ��Ʈ�� ã�� �� �����ϴ�.");
        }
    }
}
