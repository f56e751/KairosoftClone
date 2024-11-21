using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class FoundationEvent : MonoBehaviour
{
    //창업 이벤트를 발생시키는 메소드. 아이디 int만 주어지면 그에 맞는 스크립터블 오브젝트인 Foundation을 불러온다.
    //이 메소드 안에서 창업 제안서, 다른 말로는 창업 제안 UI에 들어갈 만한 요인은 모두 표시한다. 버튼을 통해 제안을 수락할지 말지 결정하게 한다. 
    public static void MakeFoundationEvent()
    {

        Debug.Log("창업 제안서를 받았습니다.");
        GameObject uiCanvas = GameObject.Find("Canvas"); // Canvas 오브젝트 찾기
        Transform foundationProposalPanel = uiCanvas.transform.Find("FoundationProposalPanel");
        // 특정 텍스트 오브젝트 찾기 (이름에 따라 조정)
        TextMeshProUGUI Name = foundationProposalPanel.Find("Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI FoundationDescription = foundationProposalPanel.Find("FoundationDescription").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI FoundationCost = foundationProposalPanel.Find("FoundationCost").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI FoundationLevel = foundationProposalPanel.Find("FoundationLevel").GetComponent<TextMeshProUGUI>();




        int FoundationID = 1; //지금은 임의로 설정
        // int FoundationID = Random.Range(1, 21); // 1에서 20 사이의 랜덤 숫자(나중에 창업 데이터를 더 만들면 주석 풀고 사용)
        Debug.Log(FoundationID + "ID입니다");

        // Resources 폴더에서 모든 Foundation ScriptableObject 로드. Foundation은 반드시 Resources안에 있어야만 작동함!
        Foundation[] allFoundations = Resources.LoadAll<Foundation>("");

        // 위에서 설정한 FoundationID에 해당하는 ScriptableObject 찾기
        Foundation targetFoundation = allFoundations.FirstOrDefault(f => f.FoundationID == FoundationID);

        Name.text = $"Name: {targetFoundation.FoundationName}";
        FoundationDescription.text = $"FoundationDescription: {targetFoundation.FoundationDescription}";
        FoundationCost.text = $"FoundationCost: {targetFoundation.FoundationCost}";
        FoundationLevel.text = $"FoundationLevel: {targetFoundation.FoundationLevel}";


        CalculateSuccess(targetFoundation, targetFoundation.RequiredMajorID, targetFoundation.RequiredStat1, targetFoundation.RequiredStat2, targetFoundation.RequiredStat3, targetFoundation.RequiredStat4, targetFoundation.RequiredStat5, targetFoundation.RequiredStat6, targetFoundation.RequiredStat0, targetFoundation.Students);
    }



    // 제안을 수락했으면 계산 시작.
    // 창업 제안서에서 요구하는 스탯과 실제 팀원의 스탯 정보를 비교해서 확률을 측정하는 메소드.
    public static float CalculateSuccess(Foundation targetFoundation, List<int> RequiredMajorID, List<int> RequiredStat1, List<int> RequiredStat2, List<int> RequiredStat3, List<int> RequiredStat4, List<int> RequiredStat5, List<int> RequiredStat6, List<int> RequiredStat0, List<Student> Students)
    {
        Debug.Log("메소드4 실행");

        // 학과 아이디, 학과마다 우선순위에 따른 스탯의 아이디(?)를 [MajorID, main1, main2, 추가 스탯1, 추가 스탯2, 추가스탯3] 순서대로 저장.
        // 이름을 이렇게 지었을 뿐. 학과마다 비중이 큰 스탯 순서입니다. 끝의 두 개는 갑자기 추가한거라 수정 필요.
        List<int> listA = new List<int> { 1, 4, 1, 2 ,0,3};      // 국어국문학과
        List<int> listB = new List<int> { 2, 4,0,2,3,1};        //심리학과
        List<int> listC = new List<int> { 3, 3,2,4,1,0 };        //경영학과
        List<int> listD = new List<int> { 4, 1,4,2,0,3};          //언론홍보영상학부
        List<int> listE = new List<int> { 5, 3,2,0,4,1};      //수학과
        List<int> listF = new List<int> { 6, 0,3,2,4,1};      //전기전자공학부
        List<int> listG = new List<int> { 7, 0,3,2,4,1};      //기계공학과
        List<int> listH = new List<int> { 8, 2,0,3,4,1};      //컴퓨터과학과
        List<int> listI = new List<int> { 9, 1, 4,0 ,2,3};      //통합디자인학과
        List<int> listJ = new List<int> { 10, 1,0,4 ,2,3};        //스포츠응용산업학과

        // 위 리스트들을 모은 리스트 생성
        List<List<int>> nestedList = new List<List<int>>
        {
            listA,
            listB,
            listC,
            listD,
            listE,
            listF,
            listG,
            listH,
            listI,
            listJ,
        };

        // 제안서에서 요구하는 학과별 스탯 정보 리스트의 이름과 실제 리스트 매핑
        Dictionary<string, List<int>> listMap = new Dictionary<string, List<int>>
        {
            { "RequiredStat0", RequiredStat0 },
            { "RequiredStat1", RequiredStat1 },
            { "RequiredStat2", RequiredStat2 },
            { "RequiredStat3", RequiredStat3 },
            { "RequiredStat4", RequiredStat4 },
            { "RequiredStat5", RequiredStat5 },
            { "RequiredStat6", RequiredStat6 },
        };



        int ListCount = nestedList.Count;
        int studentCount = Students.Count;
        // 학과 마다 반복하면서 창업 지원한 학생들의 학과 정보를 탐색한다. 정렬된 학과와 일치하면 그 학생도 정렬된 학생 리스트에 추가된다. 즉 정해진 학과 순서대로 학생을 배열한 것.
        // EX)  sortedStudent = [학생4, 없음, 학생2, 없음, 없음, 학생 3, 없음, 없음, 학생1]
        List<Student> sortedStudent = new List<Student>(new Student[ListCount]); // 학과별 크기 초기화

        for (int i = 0; i < ListCount; i++)
        {
            bool found = false; // 학생이 학과에 할당되었는지 확인
            for (int j = 0; j < studentCount; j++)
            {
                if (Students[j].majorID == i) // 학과 ID가 i인 학생 탐색
                {
                    sortedStudent[i] = Students[j]; // 학생 추가
                    found = true;
                    break; // 한 학과에 한 학생만 추가한다면 반복 종료
                }
            }
            if (!found)
            {
                sortedStudent[i] = null; // 학과에 학생이 없으면 null 추가
            }
        }
        foreach (var student in sortedStudent)
        {
            Debug.Log(student); // Student 클래스에 ToString()이 구현되어 있어야 자세한 정보가 출력됨
        }

        // 제안서 마다 필요한 학과 수 만큼 반복해서 학과의 정보와 학과 스탯의 우선 순위 정보를 저장한다. 
        // 필요한 학과 정보 정렬
        int RequiredCount = RequiredMajorID.Count;
        List<List<int>> sortedMajorList = new List<List<int>>();

        for (int i = 0; i < ListCount; i++)
        {
            bool isMatched = false; // 학과 매칭 여부 확인
            for (int j = 0; j < RequiredCount; j++)
            {
                if (RequiredMajorID[j] == i)
                {
                    List<int> targetList = nestedList[i];
                    sortedMajorList.Add(targetList);

                    // 필요한 학과 우선순위를 sortedMajorList에 추가. 그 다음에 필요한 학과 정보도 함께 추가.
                    string requiredName = "RequiredStat" + j.ToString();
                    // requiredName을 키로 사용하여 listMap에서 해당 리스트를 찾아서 targetList에 저장
                    if (listMap.ContainsKey(requiredName))
                    {
                        sortedMajorList.Add(listMap[requiredName]);
                        Debug.Log(requiredName);
                    }
                    isMatched = true; // 매칭되었음을 표시
                    break; // 학과가 매칭되면 추가 작업 없이 탈출
                }
            }

            // 매칭되지 않은 경우 null 추가
            if (!isMatched)
            {
                sortedMajorList.Add(null);
                sortedMajorList.Add(null);
            }
        }
        Debug.Log($"List Count: {sortedMajorList.Count}");




        Debug.Log("학생들 정보 비교시작");
        float GoalNumber = studentCount;  // 학과 당 성공 여부를 기록
        float Number = 0; // 0은 실패 

        //학과 리스트 숫자마다 돌면서 창업에 필요한 스탯과 학생들이 갖고 있는 스탯을 비교한다. 비교할 때는 우선순위에 맞추어 비교한다.
        for (int i = 0; i < ListCount; i++)
        {
            if (sortedStudent[i] == null && sortedMajorList[i*2] == null) continue;
            {
                List<int> targetList = sortedMajorList[i*2];
                int main1ID = targetList[1];
                int main2ID = targetList[2];
                int extra1ID = targetList[3];
                int extra2ID = targetList[4];
                int extra3ID = targetList[5];
                List<int> compareList = sortedMajorList[i * 2 +1];



                // 메인1 스탯은 무조건 특정 값을 넘어야 하고 나머지 스탯은 그 총합이 특정 수를 넘으면 성공.
                // 추가 스탯 조건 값은 고정해야 할 듯...
                if (targetFoundation.FoundationLevel == 1)
                {
                    if (sortedStudent[i].Stats[main1ID] > compareList[main1ID]) 
                    {
                        Number += 0.5f;
                        Debug.Log(sortedStudent[i].Stats[main1ID]);
                        Debug.Log(compareList[main1ID] + i + "입니다.");
                    }
                    if (sortedStudent[i].Stats[main2ID] + sortedStudent[i].Stats[extra1ID] + sortedStudent[i].Stats[extra2ID] + sortedStudent[i].Stats[extra3ID] > 25)
                    {
                        Number += 0.5f;
                    }
                }

                // 메인1 스탯과 메인2 스탯은 무조건 특정 값을 넘어야 하고 나머지 스탯은 그 총합이 특정 수를 넘으면 성공.
                // 추가 스탯 조건 값은 역시 고정해야 할 듯...
                if (targetFoundation.FoundationLevel == 2)
                {
                    if (sortedStudent[i].Stats[main1ID] > compareList[main1ID])
                    {
                        //추가 스탯 조건을 충족하는가는 아직 구현하지 못함...
                        Number += 0.4f;
                        Debug.Log(sortedStudent[i].Stats[main1ID]);
                        Debug.Log(compareList[main1ID] + i +"입니다.");
                    }

                    if (sortedStudent[i].Stats[main2ID] > compareList[main2ID])
                    {
                        Number += 0.3f;
                        Debug.Log(sortedStudent[i].Stats[main2ID]);
                        Debug.Log(compareList[main2ID] + i + "입니다. 2");
                    }

                    if (sortedStudent[i].Stats[extra1ID] + sortedStudent[i].Stats[extra2ID] + sortedStudent[i].Stats[extra3ID] > 35)
                    {
                        Number += 0.3f;
                    }
                }

                // 메인1 스탯, 메인2 스탯, 추가 스탯 모두 무조건 특정 값을 넘어야 성공.
                if (targetFoundation.FoundationLevel == 3)
                {
                    if (sortedStudent[i].Stats[main1ID] > compareList[main1ID])
                    {
                        Number += 0.35f;
                        Debug.Log(sortedStudent[i].Stats[main1ID]);
                        Debug.Log(compareList[main1ID] + i + "입니다.");
                    }
                    if (sortedStudent[i].Stats[main2ID] > compareList[main2ID])
                    {
                        Number += 0.35f;
                        Debug.Log(sortedStudent[i].Stats[main2ID]);
                        Debug.Log(compareList[main2ID] + i + "입니다. 2");
                    }
                    if (sortedStudent[i].Stats[extra1ID] > compareList[extra1ID])
                    {
                        Number += 0.3f;
                        Debug.Log(sortedStudent[i].Stats[extra1ID]);
                        Debug.Log(compareList[extra1ID] + i + "입니다. 3");
                    }
                }
            }
        }

        float Probability = Number / GoalNumber;
        Debug.Log(Number + " 현재값");
        Debug.Log(GoalNumber + " 목표");
        Debug.Log(Probability + " 확률");

        PutProbability(targetFoundation, Probability);
        return Probability;
    }

    // 성공 확률을 해당 스크립터블 오브젝트인 Foundation에도 반영하는 메소드
    public static void PutProbability(Foundation UpdateFoundation, float Probability)
    {
        UpdateFoundation.SuccessProbability = Probability;
    }

}
