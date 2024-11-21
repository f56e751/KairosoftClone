using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class FoundationEvent : MonoBehaviour
{
    //â�� �̺�Ʈ�� �߻���Ű�� �޼ҵ�. ���̵� int�� �־����� �׿� �´� ��ũ���ͺ� ������Ʈ�� Foundation�� �ҷ��´�.
    //�� �޼ҵ� �ȿ��� â�� ���ȼ�, �ٸ� ���δ� â�� ���� UI�� �� ���� ������ ��� ǥ���Ѵ�. ��ư�� ���� ������ �������� ���� �����ϰ� �Ѵ�. 
    public static void MakeFoundationEvent()
    {

        Debug.Log("â�� ���ȼ��� �޾ҽ��ϴ�.");
        GameObject uiCanvas = GameObject.Find("Canvas"); // Canvas ������Ʈ ã��
        Transform foundationProposalPanel = uiCanvas.transform.Find("FoundationProposalPanel");
        // Ư�� �ؽ�Ʈ ������Ʈ ã�� (�̸��� ���� ����)
        TextMeshProUGUI Name = foundationProposalPanel.Find("Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI FoundationDescription = foundationProposalPanel.Find("FoundationDescription").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI FoundationCost = foundationProposalPanel.Find("FoundationCost").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI FoundationLevel = foundationProposalPanel.Find("FoundationLevel").GetComponent<TextMeshProUGUI>();




        int FoundationID = 1; //������ ���Ƿ� ����
        // int FoundationID = Random.Range(1, 21); // 1���� 20 ������ ���� ����(���߿� â�� �����͸� �� ����� �ּ� Ǯ�� ���)
        Debug.Log(FoundationID + "ID�Դϴ�");

        // Resources �������� ��� Foundation ScriptableObject �ε�. Foundation�� �ݵ�� Resources�ȿ� �־�߸� �۵���!
        Foundation[] allFoundations = Resources.LoadAll<Foundation>("");

        // ������ ������ FoundationID�� �ش��ϴ� ScriptableObject ã��
        Foundation targetFoundation = allFoundations.FirstOrDefault(f => f.FoundationID == FoundationID);

        Name.text = $"Name: {targetFoundation.FoundationName}";
        FoundationDescription.text = $"FoundationDescription: {targetFoundation.FoundationDescription}";
        FoundationCost.text = $"FoundationCost: {targetFoundation.FoundationCost}";
        FoundationLevel.text = $"FoundationLevel: {targetFoundation.FoundationLevel}";


        CalculateSuccess(targetFoundation, targetFoundation.RequiredMajorID, targetFoundation.RequiredStat1, targetFoundation.RequiredStat2, targetFoundation.RequiredStat3, targetFoundation.RequiredStat4, targetFoundation.RequiredStat5, targetFoundation.RequiredStat6, targetFoundation.RequiredStat0, targetFoundation.Students);
    }



    // ������ ���������� ��� ����.
    // â�� ���ȼ����� �䱸�ϴ� ���Ȱ� ���� ������ ���� ������ ���ؼ� Ȯ���� �����ϴ� �޼ҵ�.
    public static float CalculateSuccess(Foundation targetFoundation, List<int> RequiredMajorID, List<int> RequiredStat1, List<int> RequiredStat2, List<int> RequiredStat3, List<int> RequiredStat4, List<int> RequiredStat5, List<int> RequiredStat6, List<int> RequiredStat0, List<Student> Students)
    {
        Debug.Log("�޼ҵ�4 ����");

        // �а� ���̵�, �а����� �켱������ ���� ������ ���̵�(?)�� [MajorID, main1, main2, �߰� ����1, �߰� ����2, �߰�����3] ������� ����.
        // �̸��� �̷��� ������ ��. �а����� ������ ū ���� �����Դϴ�. ���� �� ���� ���ڱ� �߰��ѰŶ� ���� �ʿ�.
        List<int> listA = new List<int> { 1, 4, 1, 2 ,0,3};      // ������а�
        List<int> listB = new List<int> { 2, 4,0,2,3,1};        //�ɸ��а�
        List<int> listC = new List<int> { 3, 3,2,4,1,0 };        //�濵�а�
        List<int> listD = new List<int> { 4, 1,4,2,0,3};          //���ȫ�������к�
        List<int> listE = new List<int> { 5, 3,2,0,4,1};      //���а�
        List<int> listF = new List<int> { 6, 0,3,2,4,1};      //�������ڰ��к�
        List<int> listG = new List<int> { 7, 0,3,2,4,1};      //�����а�
        List<int> listH = new List<int> { 8, 2,0,3,4,1};      //��ǻ�Ͱ��а�
        List<int> listI = new List<int> { 9, 1, 4,0 ,2,3};      //���յ������а�
        List<int> listJ = new List<int> { 10, 1,0,4 ,2,3};        //�������������а�

        // �� ����Ʈ���� ���� ����Ʈ ����
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

        // ���ȼ����� �䱸�ϴ� �а��� ���� ���� ����Ʈ�� �̸��� ���� ����Ʈ ����
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
        // �а� ���� �ݺ��ϸ鼭 â�� ������ �л����� �а� ������ Ž���Ѵ�. ���ĵ� �а��� ��ġ�ϸ� �� �л��� ���ĵ� �л� ����Ʈ�� �߰��ȴ�. �� ������ �а� ������� �л��� �迭�� ��.
        // EX)  sortedStudent = [�л�4, ����, �л�2, ����, ����, �л� 3, ����, ����, �л�1]
        List<Student> sortedStudent = new List<Student>(new Student[ListCount]); // �а��� ũ�� �ʱ�ȭ

        for (int i = 0; i < ListCount; i++)
        {
            bool found = false; // �л��� �а��� �Ҵ�Ǿ����� Ȯ��
            for (int j = 0; j < studentCount; j++)
            {
                if (Students[j].majorID == i) // �а� ID�� i�� �л� Ž��
                {
                    sortedStudent[i] = Students[j]; // �л� �߰�
                    found = true;
                    break; // �� �а��� �� �л��� �߰��Ѵٸ� �ݺ� ����
                }
            }
            if (!found)
            {
                sortedStudent[i] = null; // �а��� �л��� ������ null �߰�
            }
        }
        foreach (var student in sortedStudent)
        {
            Debug.Log(student); // Student Ŭ������ ToString()�� �����Ǿ� �־�� �ڼ��� ������ ��µ�
        }

        // ���ȼ� ���� �ʿ��� �а� �� ��ŭ �ݺ��ؼ� �а��� ������ �а� ������ �켱 ���� ������ �����Ѵ�. 
        // �ʿ��� �а� ���� ����
        int RequiredCount = RequiredMajorID.Count;
        List<List<int>> sortedMajorList = new List<List<int>>();

        for (int i = 0; i < ListCount; i++)
        {
            bool isMatched = false; // �а� ��Ī ���� Ȯ��
            for (int j = 0; j < RequiredCount; j++)
            {
                if (RequiredMajorID[j] == i)
                {
                    List<int> targetList = nestedList[i];
                    sortedMajorList.Add(targetList);

                    // �ʿ��� �а� �켱������ sortedMajorList�� �߰�. �� ������ �ʿ��� �а� ������ �Բ� �߰�.
                    string requiredName = "RequiredStat" + j.ToString();
                    // requiredName�� Ű�� ����Ͽ� listMap���� �ش� ����Ʈ�� ã�Ƽ� targetList�� ����
                    if (listMap.ContainsKey(requiredName))
                    {
                        sortedMajorList.Add(listMap[requiredName]);
                        Debug.Log(requiredName);
                    }
                    isMatched = true; // ��Ī�Ǿ����� ǥ��
                    break; // �а��� ��Ī�Ǹ� �߰� �۾� ���� Ż��
                }
            }

            // ��Ī���� ���� ��� null �߰�
            if (!isMatched)
            {
                sortedMajorList.Add(null);
                sortedMajorList.Add(null);
            }
        }
        Debug.Log($"List Count: {sortedMajorList.Count}");




        Debug.Log("�л��� ���� �񱳽���");
        float GoalNumber = studentCount;  // �а� �� ���� ���θ� ���
        float Number = 0; // 0�� ���� 

        //�а� ����Ʈ ���ڸ��� ���鼭 â���� �ʿ��� ���Ȱ� �л����� ���� �ִ� ������ ���Ѵ�. ���� ���� �켱������ ���߾� ���Ѵ�.
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



                // ����1 ������ ������ Ư�� ���� �Ѿ�� �ϰ� ������ ������ �� ������ Ư�� ���� ������ ����.
                // �߰� ���� ���� ���� �����ؾ� �� ��...
                if (targetFoundation.FoundationLevel == 1)
                {
                    if (sortedStudent[i].Stats[main1ID] > compareList[main1ID]) 
                    {
                        Number += 0.5f;
                        Debug.Log(sortedStudent[i].Stats[main1ID]);
                        Debug.Log(compareList[main1ID] + i + "�Դϴ�.");
                    }
                    if (sortedStudent[i].Stats[main2ID] + sortedStudent[i].Stats[extra1ID] + sortedStudent[i].Stats[extra2ID] + sortedStudent[i].Stats[extra3ID] > 25)
                    {
                        Number += 0.5f;
                    }
                }

                // ����1 ���Ȱ� ����2 ������ ������ Ư�� ���� �Ѿ�� �ϰ� ������ ������ �� ������ Ư�� ���� ������ ����.
                // �߰� ���� ���� ���� ���� �����ؾ� �� ��...
                if (targetFoundation.FoundationLevel == 2)
                {
                    if (sortedStudent[i].Stats[main1ID] > compareList[main1ID])
                    {
                        //�߰� ���� ������ �����ϴ°��� ���� �������� ����...
                        Number += 0.4f;
                        Debug.Log(sortedStudent[i].Stats[main1ID]);
                        Debug.Log(compareList[main1ID] + i +"�Դϴ�.");
                    }

                    if (sortedStudent[i].Stats[main2ID] > compareList[main2ID])
                    {
                        Number += 0.3f;
                        Debug.Log(sortedStudent[i].Stats[main2ID]);
                        Debug.Log(compareList[main2ID] + i + "�Դϴ�. 2");
                    }

                    if (sortedStudent[i].Stats[extra1ID] + sortedStudent[i].Stats[extra2ID] + sortedStudent[i].Stats[extra3ID] > 35)
                    {
                        Number += 0.3f;
                    }
                }

                // ����1 ����, ����2 ����, �߰� ���� ��� ������ Ư�� ���� �Ѿ�� ����.
                if (targetFoundation.FoundationLevel == 3)
                {
                    if (sortedStudent[i].Stats[main1ID] > compareList[main1ID])
                    {
                        Number += 0.35f;
                        Debug.Log(sortedStudent[i].Stats[main1ID]);
                        Debug.Log(compareList[main1ID] + i + "�Դϴ�.");
                    }
                    if (sortedStudent[i].Stats[main2ID] > compareList[main2ID])
                    {
                        Number += 0.35f;
                        Debug.Log(sortedStudent[i].Stats[main2ID]);
                        Debug.Log(compareList[main2ID] + i + "�Դϴ�. 2");
                    }
                    if (sortedStudent[i].Stats[extra1ID] > compareList[extra1ID])
                    {
                        Number += 0.3f;
                        Debug.Log(sortedStudent[i].Stats[extra1ID]);
                        Debug.Log(compareList[extra1ID] + i + "�Դϴ�. 3");
                    }
                }
            }
        }

        float Probability = Number / GoalNumber;
        Debug.Log(Number + " ���簪");
        Debug.Log(GoalNumber + " ��ǥ");
        Debug.Log(Probability + " Ȯ��");

        PutProbability(targetFoundation, Probability);
        return Probability;
    }

    // ���� Ȯ���� �ش� ��ũ���ͺ� ������Ʈ�� Foundation���� �ݿ��ϴ� �޼ҵ�
    public static void PutProbability(Foundation UpdateFoundation, float Probability)
    {
        UpdateFoundation.SuccessProbability = Probability;
    }

}
