using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FoundationEventEnd : MonoBehaviour
{
    // Ȯ���� ������ ���� ���θ� ���������� �Լ�
    public void DecideSuccess(int FoundationID)
    {
        FoundationID = 1; // ���Ƿ� ����

        Debug.Log(FoundationID + "���� �޼ҵ� ����");

        // Resources �������� ��� Foundation ScriptableObject �ε�
        Foundation[] allFoundations = Resources.LoadAll<Foundation>("");

        // ������ ������ FoundationID�� �ش��ϴ� ScriptableObject ã��
        Foundation targetFoundation = allFoundations.FirstOrDefault(f => f.FoundationID == FoundationID);

        //����Ȯ���� 1�̸� 95%�� ���߾� ���� ���� ����
        if (targetFoundation.SuccessProbability == 1)
        {
            float randomValue = Random.Range(0f, 100f);
            if (randomValue < 95f)
            {
                giveReward(targetFoundation);
            }
        }
        // �׷��� ������ ����Ȯ���� ���߾� ���� ���� ����
        else if (Random.value < targetFoundation.SuccessProbability)
        {
            giveReward(targetFoundation);
        }

        //������ ���
        else
        {
            Debug.Log(targetFoundation.SuccessProbability);
            Debug.Log("���� ����!");
        }
    }

    // ������ �ֱ� ���� �޼ҵ�. ��ü���� �����̳� ���� ���Ŀ� �ٲ㵵 ����!
    public void giveReward(Foundation targetFoundation)
    {
        Debug.Log("���� �ֱ�");
        if (targetFoundation.FoundationLevel == 1)
        {
            float Reward = 50000;
            GameManager.Instance.UpdateGold(50000f); //���� �ý��ۿ� �ݿ��ϱ�
            Debug.Log(Reward + "���� �ݾ��Դϴ�");
        }
        if (targetFoundation.FoundationLevel == 2)
        {
            int Reward = 100000;
            GameManager.Instance.UpdateGold(100000f);
            Debug.Log(Reward + "���� �ݾ��Դϴ�");
        }
        if (targetFoundation.FoundationLevel == 3)
        {
            int Reward = 200000;
            GameManager.Instance.UpdateGold(200000f);
            Debug.Log(Reward + "���� �ݾ��Դϴ�");
        }
    }
}
