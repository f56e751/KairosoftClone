using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FoundationEventEnd : MonoBehaviour
{
    // 확률로 실제로 성공 여부를 판정내리는 함수
    public void DecideSuccess(int FoundationID)
    {
        FoundationID = 1; // 임의로 설정

        Debug.Log(FoundationID + "보상 메소드 시작");

        // Resources 폴더에서 모든 Foundation ScriptableObject 로드
        Foundation[] allFoundations = Resources.LoadAll<Foundation>("");

        // 위에서 설정한 FoundationID에 해당하는 ScriptableObject 찾기
        Foundation targetFoundation = allFoundations.FirstOrDefault(f => f.FoundationID == FoundationID);

        //성공확률이 1이면 95%로 낮추어 성공 여부 결정
        if (targetFoundation.SuccessProbability == 1)
        {
            float randomValue = Random.Range(0f, 100f);
            if (randomValue < 95f)
            {
                giveReward(targetFoundation);
            }
        }
        // 그렇지 않으면 성공확률에 맞추어 성공 여부 결정
        else if (Random.value < targetFoundation.SuccessProbability)
        {
            giveReward(targetFoundation);
        }

        //실패한 경우
        else
        {
            Debug.Log(targetFoundation.SuccessProbability);
            Debug.Log("보상 실패!");
        }
    }

    // 보상을 주기 위한 메소드. 구체적인 조건이나 값은 추후에 바꿔도 좋다!
    public void giveReward(Foundation targetFoundation)
    {
        Debug.Log("보상 주기");
        if (targetFoundation.FoundationLevel == 1)
        {
            float Reward = 50000;
            GameManager.Instance.UpdateGold(50000f); //메인 시스템에 반영하기
            Debug.Log(Reward + "보상 금액입니당");
        }
        if (targetFoundation.FoundationLevel == 2)
        {
            int Reward = 100000;
            GameManager.Instance.UpdateGold(100000f);
            Debug.Log(Reward + "보상 금액입니당");
        }
        if (targetFoundation.FoundationLevel == 3)
        {
            int Reward = 200000;
            GameManager.Instance.UpdateGold(200000f);
            Debug.Log(Reward + "보상 금액입니당");
        }
    }
}
