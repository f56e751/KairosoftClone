using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryScreenManager : MonoBehaviour
{
    public GameObject professorInfoPrefab;  // ProfessorInfo ������ ����
    public GameObject BuildingInfoPrefab;  // ProfessorInfo ������ ����
    private List<GameObject> professorInfoObjects = new List<GameObject>();  // ������ �����յ��� ������ ����Ʈ
    private List<GameObject> BuildingInfoObjects = new List<GameObject>();  // ������ �����յ��� ������ ����Ʈ
    private int totalSalary = 0;  // �� ������ ������ ����  (�߰��� �κ�)
    public Text IncomeText;

    void OnEnable()
    {
        // ���� ���� ����Ʈ ��������
        List<ProfessorInfo.Professor> hiredProfessors = ProfessorInfo.hiredProfessors;
        // SummaryScreen�� Content ����
        GameObject content = GameObject.Find("SummaryScreen(Clone)/Scroll View1/Viewport/Content");

        if (content != null)
        {
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
                professorRect.anchoredPosition = new Vector2(professorRect.anchoredPosition.x, -i * 60);

                // �ؽ�Ʈ ������Ʈ �����ͼ� ���� ������Ʈ
                Text professorText = professorInfo.GetComponent<Text>();
                professorText.text = $"{hiredProfessors[i].name} / {hiredProfessors[i].kind} / {hiredProfessors[i].department}";

                // ������ �������� ����Ʈ�� �߰�
                professorInfoObjects.Add(professorInfo);

                // ���� ��� �� �ջ� (�߰��� �κ�)
                int salary = 0;
                if (hiredProfessors[i].kind == "������")
                {
                    salary = 1000;
                }
                else if (hiredProfessors[i].kind == "�α���")
                {
                    salary = 750;
                }
                else if (hiredProfessors[i].kind == "������")
                {
                    salary = 500;
                }
                totalSalary += salary;
            }

            IncomeText.text = $"{totalSalary}";

        }

        // �Ǽ��� �ǹ� ����Ʈ ��������
        Dictionary<string, int> buildingCounts = BuildingInfoManager.constructedBuildingCounts;

        // SummaryScreen�� Content ����
        GameObject content2 = GameObject.Find("SummaryScreen(Clone)/Scroll View2/Viewport/Content");

        Debug.Log("2 �ǽ�");

        if (content2 != null)
        {
            RectTransform contentRect = content2.GetComponent<RectTransform>();
            //���߿� ����Ʈ�� �������� ������ �����ϴ� �ڵ带 �ֱ�

            int i = 0; // �ε��� �ʱ�ȭ

            foreach (var building in buildingCounts)
            {
                string buildingName = building.Key; // Ű ���� ��������
                int buildingCount = building.Value; // �ش� Ű�� �� ��������

                // ProfessorInfo ������ ����
                GameObject BuildingInfo = Instantiate(BuildingInfoPrefab, content2.transform);

                // ProfessorInfo ��ġ ���� (y�� �������� �Ʒ��� ����)
                RectTransform BuildingRect = BuildingInfo.GetComponent<RectTransform>();
                BuildingRect.anchoredPosition = new Vector2(BuildingRect.anchoredPosition.x, -i * 60);

                // �ؽ�Ʈ ������Ʈ �����ͼ� ���� ������Ʈ
                Text BuildingText = BuildingInfo.GetComponent<Text>();
                BuildingText.text = $"{buildingName}: {buildingCount}"; // �ǹ� �̸��� ī��Ʈ�� �Բ� ǥ��

                // ������ �������� ����Ʈ�� �߰�
                professorInfoObjects.Add(BuildingInfo);

                i++; // �ε��� ����
            }

            IncomeText.text = $"{totalSalary}";
        }
    }
}
