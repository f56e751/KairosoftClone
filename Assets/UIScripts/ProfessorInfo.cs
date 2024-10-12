using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfessorInfo : MonoBehaviour
{
    // ���� �̸�, �а� ���� ������ ����Ʈ
    List<string> professorNames = new List<string> { "������", "������", "���¼�", "�ֺ���", "���", "������", "����Ŭ �轼", "������", "������", "����ö", "������", "�� ������" };
    List<string> kind = new List<string> { "������", "�α���", "������" };
    List<string> departments = new List<string> { "��������", "�̰�����", "������", "��������", "�ǰ�����" };

    // ���� �������� ������ ������ ����Ʈ (static���� ����)
    public static List<Professor> hiredProfessors = new List<Professor>();

    // Professor ������ ���� ����ü
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

    // 3���� ���� ������ ������ ����
    Professor Professor1;
    Professor Professor2;
    Professor Professor3;

    // AddProfessorScreen ������ ����
    public GameObject addProfessorScreenPrefab;

    void OnEnable()
    {
        // 3���� ���� ������ ����
        Professor1 = GenerateRandomProfessor();
        Professor2 = GenerateRandomProfessor();
        Professor3 = GenerateRandomProfessor();

        // UI�� ���� ���� ������Ʈ
        UpdateProfessorUI(Professor1, "ProfessorList");
        UpdateProfessorUI(Professor2, "ProfessorList2");
        UpdateProfessorUI(Professor3, "ProfessorList3");
    }

    // ���� ���� ������ ����Ʈ�� �����ϴ� �޼���
    public void SaveHiredProfessor(Professor professor)
    {
        for (int i = 0; i < hiredProfessors.Count; i++)
        {
            Debug.Log($"���� {i + 1}: �̸� - {hiredProfessors[i].name}, ���� - {hiredProfessors[i].kind}, �а� - {hiredProfessors[i].department}");
        }
    }

    Professor GenerateRandomProfessor()
    {
        string randomName = professorNames[Random.Range(0, professorNames.Count)];
        string randomKind = kind[Random.Range(0, kind.Count)];
        string randomDepartment = departments[Random.Range(0, departments.Count)];

        return new Professor(randomName, randomKind, randomDepartment);
    }

    // UpdateProfessorUI �޼��� ���ο��� ������ ����
    void UpdateProfessorUI(Professor professor, string professorListName)
    {
        GameObject professorList = GameObject.Find(professorListName);
        if (professorList != null)
        {
            Text professorNameText = professorList.transform.Find("ProfessorName").GetComponent<Text>();
            Text salaryText = professorList.transform.Find("HopeSalary").GetComponent<Text>();
            Text departmentText = professorList.transform.Find("Kind").GetComponent<Text>();

            professorNameText.text = "���� �̸�: " + professor.name;
            salaryText.text = "���� ����: " + professor.kind;
            departmentText.text = "�а�: " + professor.department;

            // EmployButton�� ã�� Ŭ�� �̺�Ʈ �߰� �� ��Ȱ��ȭ
            Button employButton = professorList.transform.Find("EmployButton")?.GetComponent<Button>();
            if (employButton != null)
            {
                employButton.onClick.AddListener(() => OnEmployButtonClick(professorListName, professor));
            }
        }
    }

    // OnEmployButtonClick �޼��� ����
    void OnEmployButtonClick(string professorListName, Professor professor)
    {
        // ���� ���� ������ ����Ʈ�� �߰�
        hiredProfessors.Add(professor);

        // ���� �� ����
        GameManager.Instance.IncreaseProfessorCount(1);

        // ������ ���޿� ���� ��� ����
        float goldCost = 0;
        if (professor.kind == "������")
        {
            goldCost = 500;
        }
        else if (professor.kind == "�α���")
        {
            goldCost = 750;
        }
        else if (professor.kind == "������")
        {
            goldCost = 1000;
        }

        // ��� ����
        GameManager.Instance.UpdateGold(-goldCost);
        SaveHiredProfessor(professor);
        Debug.Log("���� ����: " + professor.name + ", ��� ����: " + goldCost);

        // EmployButton ��Ȱ��ȭ
        GameObject professorList = GameObject.Find(professorListName);
        if (professorList != null)
        {
            Button employButton = professorList.transform.Find("EmployButton")?.GetComponent<Button>();
            if (employButton != null)
            {
                employButton.interactable = false;  // ��ư ��Ȱ��ȭ
            }
        }
    }


    // SummaryScreen�� Scroll View1�� Content�� ���� ���� ���� ������Ʈ
    public void UpdateSummaryScreen()
    {
        GameObject summaryScreen = GameObject.Find("SummaryScreen");
        if (summaryScreen != null)
        {
            Transform content = summaryScreen.transform.Find("Scroll View1/Viewport/Content");
            if (content != null)
            {
                // �ִ� 8���� ProfessorInfo ������Ʈ
                for (int i = 0; i < 8; i++)
                {
                    Transform professorInfo = content.Find("ProfessorInfo" + (i + 1));
                    if (professorInfo != null)
                    {
                        Text professorText = professorInfo.GetComponent<Text>();

                        if (i < hiredProfessors.Count)
                        {
                            // ���� ���� ���� �ݿ�: �����̸�/��������/�а�
                            professorText.text = $"{hiredProfessors[i].name} / {hiredProfessors[i].kind} / {hiredProfessors[i].department}";
                        }
                        else
                        {
                            // ���� ������ ������ ��ĭ���� ����
                            professorText.text = "";
                        }
                    }
                }
            }
        }
    }


}
