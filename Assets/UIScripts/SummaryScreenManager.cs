using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryScreenManager : MonoBehaviour
{
    public GameObject professorInfoPrefab;  // ProfessorInfo 프리팹 참조
    public GameObject BuildingInfoPrefab;  // ProfessorInfo 프리팹 참조
    private List<GameObject> professorInfoObjects = new List<GameObject>();  // 생성된 프리팹들을 저장할 리스트
    private List<GameObject> BuildingInfoObjects = new List<GameObject>();  // 생성된 프리팹들을 저장할 리스트
    private int totalSalary = 0;  // 총 월급을 저장할 변수  (추가된 부분)
    public Text IncomeText;

    void OnEnable()
    {
        // 고용된 교수 리스트 가져오기
        List<ProfessorInfo.Professor> hiredProfessors = ProfessorInfo.hiredProfessors;
        // SummaryScreen의 Content 참조
        GameObject content = GameObject.Find("SummaryScreen(Clone)/Scroll View1/Viewport/Content");

        if (content != null)
        {
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
                professorRect.anchoredPosition = new Vector2(professorRect.anchoredPosition.x, -i * 60);

                // 텍스트 컴포넌트 가져와서 정보 업데이트
                Text professorText = professorInfo.GetComponent<Text>();
                professorText.text = $"{hiredProfessors[i].name} / {hiredProfessors[i].kind} / {hiredProfessors[i].department}";

                // 생성된 프리팹을 리스트에 추가
                professorInfoObjects.Add(professorInfo);

                // 월급 계산 및 합산 (추가된 부분)
                int salary = 0;
                if (hiredProfessors[i].kind == "정교수")
                {
                    salary = 1000;
                }
                else if (hiredProfessors[i].kind == "부교수")
                {
                    salary = 750;
                }
                else if (hiredProfessors[i].kind == "조교수")
                {
                    salary = 500;
                }
                totalSalary += salary;
            }

            IncomeText.text = $"{totalSalary}";

        }

        // 건설된 건물 리스트 가져오기
        Dictionary<string, int> buildingCounts = BuildingInfoManager.constructedBuildingCounts;

        // SummaryScreen의 Content 참조
        GameObject content2 = GameObject.Find("SummaryScreen(Clone)/Scroll View2/Viewport/Content");

        Debug.Log("2 실시");

        if (content2 != null)
        {
            RectTransform contentRect = content2.GetComponent<RectTransform>();
            //나중에 리스트가 많아지면 적당히 조절하는 코드를 넣기

            int i = 0; // 인덱스 초기화

            foreach (var building in buildingCounts)
            {
                string buildingName = building.Key; // 키 값을 가져오기
                int buildingCount = building.Value; // 해당 키의 값 가져오기

                // ProfessorInfo 프리팹 생성
                GameObject BuildingInfo = Instantiate(BuildingInfoPrefab, content2.transform);

                // ProfessorInfo 위치 설정 (y축 기준으로 아래로 나열)
                RectTransform BuildingRect = BuildingInfo.GetComponent<RectTransform>();
                BuildingRect.anchoredPosition = new Vector2(BuildingRect.anchoredPosition.x, -i * 60);

                // 텍스트 컴포넌트 가져와서 정보 업데이트
                Text BuildingText = BuildingInfo.GetComponent<Text>();
                BuildingText.text = $"{buildingName}: {buildingCount}"; // 건물 이름과 카운트를 함께 표시

                // 생성된 프리팹을 리스트에 추가
                professorInfoObjects.Add(BuildingInfo);

                i++; // 인덱스 증가
            }

            IncomeText.text = $"{totalSalary}";
        }
    }
}
