using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryScreenManager : MonoBehaviour
{
    public GameObject professorInfoPrefab;  // ProfessorInfo 프리팹 참조
    private List<GameObject> professorInfoObjects = new List<GameObject>();  // 생성된 프리팹들을 저장할 리스트

    void OnEnable()
    {
        // ProfessorInfo 스크립트에서 고용된 교수 리스트 참조
        ProfessorInfo professorInfoScript = FindObjectOfType<ProfessorInfo>();

        if (professorInfoScript != null)
        {
            // 고용된 교수 리스트 가져오기
            List<ProfessorInfo.Professor> hiredProfessors = ProfessorInfo.hiredProfessors;

            // SummaryScreen의 Content 참조
            GameObject content = GameObject.Find("SummaryScreen/Scroll View1/Viewport/Content");

            if (content != null)
            {
                // 기존에 생성된 프리팹들 삭제
                foreach (var obj in professorInfoObjects)
                {
                    Destroy(obj);
                }
                professorInfoObjects.Clear();

                // Content의 높이를 고용된 교수 수에 따라 조정
                RectTransform contentRect = content.GetComponent<RectTransform>();
                contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, hiredProfessors.Count * 50);

                // 고용된 교수 수만큼 ProfessorInfo 프리팹 생성 및 UI 업데이트
                for (int i = 0; i < hiredProfessors.Count; i++)
                {
                    // ProfessorInfo 프리팹 생성
                    GameObject professorInfo = Instantiate(professorInfoPrefab, content.transform);

                    // ProfessorInfo 위치 설정 (y축 기준으로 아래로 나열)
                    RectTransform professorRect = professorInfo.GetComponent<RectTransform>();
                    professorRect.anchoredPosition = new Vector2(professorRect.anchoredPosition.x, -i * 50);

                    // 텍스트 컴포넌트 가져와서 정보 업데이트
                    Text professorText = professorInfo.GetComponent<Text>();
                    professorText.text = $"{hiredProfessors[i].name} / {hiredProfessors[i].kind} / {hiredProfessors[i].department}";

                    // 생성된 프리팹을 리스트에 추가
                    professorInfoObjects.Add(professorInfo);
                }
            }
            else
            {
                Debug.LogWarning("SummaryScreen의 Content를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("ProfessorInfo 스크립트를 찾을 수 없습니다.");
        }
    }
}
