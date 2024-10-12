using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfessorInfo : MonoBehaviour
{
    // 교수 이름, 학과 등을 저장할 리스트
    List<string> professorNames = new List<string> { "문형근", "김지은", "박태성", "최보희", "김상만", "정의진", "마이클 잭슨", "나영석", "백정원", "정상철", "박찬욱", "빌 게이츠" };
    List<string> kind = new List<string> { "조교수", "부교수", "정교수" };
    List<string> departments = new List<string> { "문과대학", "이과대학", "상경대학", "공과대학", "의과대학" };

    // 고용된 교수들의 정보를 저장할 리스트 (static으로 변경)
    public static List<Professor> hiredProfessors = new List<Professor>();

    // Professor 정보를 담을 구조체
    public struct Professor
    {
        public string name;
        public string kind;
        public string department;

        public Professor(string name, string kind, string department)
        {
            this.name = name;
            this.kind = kind;
            this.department = department;
        }
    }

    // 3명의 교수 정보를 저장할 변수
    Professor Professor1;
    Professor Professor2;
    Professor Professor3;

    // AddProfessorScreen 프리팹 참조
    public GameObject addProfessorScreenPrefab;

    void OnEnable()
    {
        // 3명의 교수 정보를 생성
        Professor1 = GenerateRandomProfessor();
        Professor2 = GenerateRandomProfessor();
        Professor3 = GenerateRandomProfessor();

        // UI에 교수 정보 업데이트
        UpdateProfessorUI(Professor1, "ProfessorList");
        UpdateProfessorUI(Professor2, "ProfessorList2");
        UpdateProfessorUI(Professor3, "ProfessorList3");
    }

    // 고용된 교수 정보를 리스트에 저장하는 메서드
    public void SaveHiredProfessor(Professor professor)
    {
        for (int i = 0; i < hiredProfessors.Count; i++)
        {
            Debug.Log($"교수 {i + 1}: 이름 - {hiredProfessors[i].name}, 직급 - {hiredProfessors[i].kind}, 학과 - {hiredProfessors[i].department}");
        }
    }

    Professor GenerateRandomProfessor()
    {
        string randomName = professorNames[Random.Range(0, professorNames.Count)];
        string randomKind = kind[Random.Range(0, kind.Count)];
        string randomDepartment = departments[Random.Range(0, departments.Count)];

        return new Professor(randomName, randomKind, randomDepartment);
    }

    // UpdateProfessorUI 메서드 내부에서 변수를 선언
    void UpdateProfessorUI(Professor professor, string professorListName)
    {
        GameObject professorList = GameObject.Find(professorListName);
        if (professorList != null)
        {
            Text professorNameText = professorList.transform.Find("ProfessorName").GetComponent<Text>();
            Text salaryText = professorList.transform.Find("HopeSalary").GetComponent<Text>();
            Text departmentText = professorList.transform.Find("Kind").GetComponent<Text>();

            professorNameText.text = "교수 이름: " + professor.name;
            salaryText.text = "교수 직급: " + professor.kind;
            departmentText.text = "학과: " + professor.department;

            // EmployButton을 찾아 클릭 이벤트 추가 및 비활성화
            Button employButton = professorList.transform.Find("EmployButton")?.GetComponent<Button>();
            if (employButton != null)
            {
                employButton.onClick.AddListener(() => OnEmployButtonClick(professorListName, professor));
            }
        }
    }

    // OnEmployButtonClick 메서드 수정
    void OnEmployButtonClick(string professorListName, Professor professor)
    {
        // 고용된 교수 정보를 리스트에 추가
        hiredProfessors.Add(professor);

        // 교수 수 증가
        GameManager.Instance.IncreaseProfessorCount(1);

        // 교수의 직급에 따라 골드 차감
        float goldCost = 0;
        if (professor.kind == "조교수")
        {
            goldCost = 500;
        }
        else if (professor.kind == "부교수")
        {
            goldCost = 750;
        }
        else if (professor.kind == "정교수")
        {
            goldCost = 1000;
        }

        // 골드 차감
        GameManager.Instance.UpdateGold(-goldCost);
        SaveHiredProfessor(professor);
        Debug.Log("교수 고용됨: " + professor.name + ", 골드 차감: " + goldCost);

        // EmployButton 비활성화
        GameObject professorList = GameObject.Find(professorListName);
        if (professorList != null)
        {
            Button employButton = professorList.transform.Find("EmployButton")?.GetComponent<Button>();
            if (employButton != null)
            {
                employButton.interactable = false;  // 버튼 비활성화
            }
        }
    }


    // SummaryScreen의 Scroll View1의 Content에 고용된 교수 정보 업데이트
    public void UpdateSummaryScreen()
    {
        GameObject summaryScreen = GameObject.Find("SummaryScreen");
        if (summaryScreen != null)
        {
            Transform content = summaryScreen.transform.Find("Scroll View1/Viewport/Content");
            if (content != null)
            {
                // 최대 8개의 ProfessorInfo 업데이트
                for (int i = 0; i < 8; i++)
                {
                    Transform professorInfo = content.Find("ProfessorInfo" + (i + 1));
                    if (professorInfo != null)
                    {
                        Text professorText = professorInfo.GetComponent<Text>();

                        if (i < hiredProfessors.Count)
                        {
                            // 고용된 교수 정보 반영: 교수이름/교수직급/학과
                            professorText.text = $"{hiredProfessors[i].name} / {hiredProfessors[i].kind} / {hiredProfessors[i].department}";
                        }
                        else
                        {
                            // 고용된 교수가 없으면 빈칸으로 설정
                            professorText.text = "";
                        }
                    }
                }
            }
        }
    }


}
