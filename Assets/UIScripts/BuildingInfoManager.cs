using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static BuildingInfoManager;

public class BuildingInfoManager : MonoBehaviour
{
    // Building 구조체 정의
    public struct Building
    {
        public string name;         // 건물 이름
        public int cost;            // 필요 비용
        public string size;         // 규모 (숫자*숫자*숫자)
        public int capacity;        // 학생 정원
        public float reputation;    // 명성

        // 생성자
        public Building(string name, int cost, string size, int capacity, float reputation)
        {
            this.name = name;
            this.cost = cost;
            this.size = size;
            this.capacity = capacity;
            this.reputation = reputation;
        }
    }

    // 건물 정보를 저장할 리스트
    List<Building> buildingList = new List<Building>();

    // **[추가된 부분]** 건설된 건물의 개수를 저장할 Dictionary
    Dictionary<string, int> constructedBuildingCounts = new Dictionary<string, int>();

    // ScrollView의 Content 참조 (이미 존재하는 BuildingInfo들을 포함한)
    public RectTransform contentTransform;

    // 게임 매니저 참조
    private GameManager gameManager;

    // BuildScene이 활성화될 때 호출
    void OnEnable()
    {
        // GameManager 오브젝트 찾기 및 gameManager 할당
        GameObject managerObject = GameObject.Find("GameManager");
        if (managerObject != null)
        {
            gameManager = managerObject.GetComponent<GameManager>();
            if (gameManager == null)
            {
                Debug.LogError("GameManager 스크립트를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("GameManager 오브젝트를 찾을 수 없습니다.");
        }

        // 예시 건물 데이터 추가
        AddBuildingData();

        // 이미 존재하는 BuildingInfo들에 정보 업데이트
        UpdateExistingBuildingInfoUI();
    }

    // 예시 건물 데이터 추가
    void AddBuildingData()
    {
        buildingList.Add(new Building("소형 강의실", 3000, "13*3*3", 20, 1));
        buildingList.Add(new Building("중형 강의실", 5000, "3*6*3", 40, 2));
        buildingList.Add(new Building("기숙사", 15000, "6*6*6", 100, 5));
        buildingList.Add(new Building("식당", 7000, "3*3*3", 20, 3));
        buildingList.Add(new Building("운동장", 100000, "10*10*1", 0, 10));
        buildingList.Add(new Building("도로", 10, "1*1*1", 0, 0));
        buildingList.Add(new Building("나무", 100, "1*1*2", 100, 0.01f));
        buildingList.Add(new Building("기본 타일", 0, "1*1*1", 0, 0));
    }

    // 기존 BuildingInfo 오브젝트들에 건물 정보 반영
    void UpdateExistingBuildingInfoUI()
    {
        int childCount = contentTransform.childCount;

        for (int i = 0; i < Mathf.Min(buildingList.Count, childCount); i++)
        {
            Building building = buildingList[i];
            string buildingInfoName = "BuildingInfo" + (i + 1).ToString();

            try
            {
                Transform buildingInfoTransform = contentTransform.Find(buildingInfoName);

                Text buildingNameText = buildingInfoTransform.Find("BuildingName").GetComponent<Text>();
                Text goldText = buildingInfoTransform.Find("GoldText").GetComponent<Text>();
                Text sizeText = buildingInfoTransform.Find("SizeText").GetComponent<Text>();
                Text incomeText = buildingInfoTransform.Find("IncomeText").GetComponent<Text>();
                Text fameText = buildingInfoTransform.Find("FameText").GetComponent<Text>();

                buildingNameText.text = building.name;
                goldText.text = building.cost.ToString();
                sizeText.text = building.size;
                incomeText.text = building.capacity.ToString();
                fameText.text = building.reputation.ToString();

                Button installButton = buildingInfoTransform.Find("InstallButton").GetComponent<Button>();
                if (installButton != null)
                {
                    installButton.onClick.AddListener(() => InstallBuilding(building, buildingInfoName));
                }
            }
            catch (Exception e)
            {
                Debug.LogError("BuildingInfo 업데이트 중 오류 발생: " + e.Message);
            }
        }
    }

    void InstallBuilding(Building building, string buildingInfoName)
    {
        GameObject buildingInfoObject = GameObject.Find(buildingInfoName);
        if (buildingInfoObject == null)
        {
            Debug.LogError("BuildingInfo 오브젝트를 찾을 수 없습니다: " + buildingInfoName);
            return;
        }

        Text buildingNameText = buildingInfoObject.transform.Find("BuildingName")?.GetComponent<Text>();
        if (buildingNameText == null)
        {
            Debug.LogError("'BuildingName' 자식 객체를 찾을 수 없습니다: " + buildingInfoName);
            return;
        }

        if (building.name == buildingNameText.text)
        {
            if (gameManager != null)
            {
                int cost = building.cost;
                gameManager.UpdateGold(-cost);
                Debug.Log(building.name + " 설치 완료! 비용: -" + cost + " 골드 차감됨.");

                // **[추가된 부분]** 건물 건설 시 개수 업데이트
                if (constructedBuildingCounts.ContainsKey(building.name))
                {
                    constructedBuildingCounts[building.name]++;
                }
                else
                {
                    constructedBuildingCounts[building.name] = 1;
                }

                // 모든 건물의 건설 횟수 출력
                foreach (var entry in constructedBuildingCounts)
                {
                    Debug.Log(entry.Key + " 건설 횟수: " + entry.Value);
                }


            }
            else
            {
                Debug.LogError("GameManager가 할당되지 않았습니다.");
            }

            Button installButton = buildingInfoObject.transform.Find("InstallButton")?.GetComponent<Button>();
        }
        else
        {
            Debug.LogError("건물 이름이 일치하지 않습니다: " + building.name + " != " + buildingNameText.text);
        }
    }
}
