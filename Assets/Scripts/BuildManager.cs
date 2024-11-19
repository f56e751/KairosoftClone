using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Analytics;
// when user is in build mode, other ui should not be shown
// so when user is in build mode, we can consider stopping time in game like 
// void Update() -> other gameManager update function
// {
//     if (buildMode) return; -> put this code on top of update function
// }
// ?? Or is there any other way ??
// -> we can add UIManager class and control UI overlap
// this class get info of current ui mode such as (build mode, 교수채용모드, 등) from other manager class

// TODO consider rotation when building object
// TODO add delete function
public class BuildManager : Singleton<BuildManager>
{
    // Start is called before the first frame update
    public GameObject basicTile;
    public Building basicBuilding;
    public Building Building_Dorm;
    public Building Building_Lib;
    public Building Building_Study;
    public Building Building_Restaurant;
    private Building currentBuilding;
    private bool isConstructionMode;

    [SerializeField] int xAxisNum = 100; // basic map x axis tile num
    [SerializeField] int zAxisNum = 100; // basic map z axis tile num
    [SerializeField] int length = 1; // tile prefab length of side

    public int minXAxisPosition => - xAxisNum / 2 * length;
    public int maxXAxisPosition => - xAxisNum / 2 * length + (xAxisNum - 1) * length;
    public int minZAxisPosition => - zAxisNum / 2 * length;
    public int maxZAxisPosition => - zAxisNum / 2 * length + (zAxisNum - 1) * length;

    
    public (int,int) entrancePosition => ((minXAxisPosition + maxXAxisPosition) / 2, minZAxisPosition); //학생들이 들어오는 정문
    // private Dictionary<Building, (int centerX, int centerY)> buildingCenters = new Dictionary<Building, (int centerX, int centerY)>();
    // 빌딩의 ID를 키로 사용하여 중심 좌표를 저장하는 해시맵
    private Dictionary<int, (int centerX, int centerY)> buildingCenters = new Dictionary<int, (int centerX, int centerY)>();


    public Graph graph; // save tile position as graph
    public int totalBuildingsConstructedNum = 0;

    private GameObject previewBuildingInstance;
    private Material transparentMaterial;
    private Material invalidPlacementMaterial;
    private Quaternion currentBuildingRotation = Quaternion.identity;


    public override void Awake()
    {
        base.Awake(); // singleton class awake method executed

        CreateTileGrid(xAxisNum, zAxisNum, length);

        (int, int) startNodeKey = (0,0); 
        (int, int) endNodeKey = (10,10);
        List<Vector3> shortestPath = graph.GetShortestPath(startNodeKey, endNodeKey);
    }
    void Start()
    {
        CreateTransparentMaterial();
        CreateInvalidPlacementMaterial();   
    }

    // Update is called once per frame
    void Update()
    {
        UserBuild();

        // 'T' 키를 눌렀을 때 (0, 0, 0) 위치에 Building 생성, 테스트용
        if (Input.GetKeyDown(KeyCode.T))
        {
            InstantiateBuildingAtOrigin();
        }


        if (isConstructionMode)
        {   
            PreviewBuildingPlacement();
            // 건물 회전 처리
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                RotatePreviewBuilding(scroll);
            }

        }
            
        else
        {
            RemovePreview();
        }
            
    }


    // function for test
    void InstantiateBuildingAtOrigin()
    {
        Vector3 originPosition = new Vector3(0, 0, 0); // 원점 위치
        Building libInstance = Instantiate(Building_Lib, originPosition, Quaternion.identity); // Building_Lib 인스턴스화
        libInstance.setId(totalBuildingsConstructedNum++); // ID 할당 및 건물 수 증가
    }

    private void RotatePreviewBuilding(float scroll)
    {
        if (previewBuildingInstance != null)
        {
            currentBuildingRotation *= Quaternion.Euler(0, 90 * Mathf.Sign(scroll), 0);
            previewBuildingInstance.transform.rotation = currentBuildingRotation;
        }
    }

    private void CreateTransparentMaterial()
    {
        transparentMaterial = new Material(Shader.Find("Standard"));
        transparentMaterial.color = new Color(1f, 1f, 1f, 0.5f); // 반투명
        transparentMaterial.SetFloat("_Mode", 3); // Transparent mode
        transparentMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        transparentMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        transparentMaterial.SetInt("_ZWrite", 0);
        transparentMaterial.DisableKeyword("_ALPHATEST_ON");
        transparentMaterial.EnableKeyword("_ALPHABLEND_ON");
        transparentMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        transparentMaterial.renderQueue = 3000;
    }


    private void CreateInvalidPlacementMaterial()
    {
        invalidPlacementMaterial = new Material(Shader.Find("Standard"));
        invalidPlacementMaterial.color = Color.red;
        invalidPlacementMaterial.SetFloat("_Mode", 3); // Transparent mode
        invalidPlacementMaterial.renderQueue = 3000;
    }

    private void PreviewBuildingPlacement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) // Assuming groundLayer is defined
        {
            Vector3 buildPosition = CalculateBuildPosition(hit.point);
            if (previewBuildingInstance == null)
            {
                previewBuildingInstance = Instantiate(currentBuilding.gameObject, buildPosition, Quaternion.identity);
                SetMaterialToTransparent(previewBuildingInstance);
            }
            else
            {
                previewBuildingInstance.transform.position = buildPosition;
            }

            
            // 위치 유효성 검사 및 색상 조정
            if (isConstructable(CalculateOccupiedTiles(currentBuilding, (int)CalculateBuildPosition(buildPosition).x, (int)CalculateBuildPosition(buildPosition).z)))
            {
                SetMaterialToTransparent(previewBuildingInstance);
            }
            else
            {
                SetMaterialToInvalid(previewBuildingInstance);
            }
        }
    }

        private void SetMaterialToInvalid(GameObject building)
    {
        Renderer[] renderers = building.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = invalidPlacementMaterial;
        }
    }

    

    private Vector3 CalculateBuildPosition(Vector3 hitPoint)
    {
        int xTilePosition = Mathf.FloorToInt(hitPoint.x / length) * length;
        int zTilePosition = Mathf.FloorToInt(hitPoint.z / length) * length;
        return new Vector3(xTilePosition, 0, zTilePosition);
    }

    private void RemovePreview()
    {
        if (previewBuildingInstance != null)
            Destroy(previewBuildingInstance);
    }

    private void SetMaterialToTransparent(GameObject building)
    {
        Renderer[] renderers = building.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = transparentMaterial;
        }
    }


    public void SetCurrentBuilding(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.Basic:
                currentBuilding = basicBuilding;
                break;
            case BuildingType.Dorm:
                currentBuilding = Building_Dorm;
                break;
            case BuildingType.Library:
                currentBuilding = Building_Lib;
                break;
            case BuildingType.Study:
                currentBuilding = Building_Study;
                break;
            case BuildingType.Restaurant:
                currentBuilding = Building_Restaurant;
                break;
        }
    }


    private void UserBuild()
    {
        if (isConstructionMode && Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼을 클릭했는지 확인
        {   
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // hit.point는 클릭된 지점의 정확한 월드 좌표입니다.
                Vector3 buildPosition = hit.point;
                buildPosition.y = 0; // 지면 높이로 설정(높이 조정이 필요하면 변경)

                // 타일의 정확한 위치를 계산하기 위해 빌딩 크기의 반을 고려하여 정렬합니다.
                int xTilePosition = Mathf.FloorToInt(buildPosition.x / length) * length;
                int zTilePosition = Mathf.FloorToInt(buildPosition.z / length) * length;

                BuildObjectOnTile(currentBuilding, xTilePosition, zTilePosition, currentBuildingRotation);
                currentBuilding.setId(totalBuildingsConstructedNum);
                
                currentBuildingRotation = Quaternion.identity;
            }
            totalBuildingsConstructedNum += 1;
            isConstructionMode = false;
        }
    }

    public void SetConstructionMode()
    {
        isConstructionMode = true;
    }


    private void CreateTileGrid(int xAxisNum,  int zAxisNum, int length)  
    {   
        // float offset = 50.0f;
        Vector3 startPosition = new Vector3(minXAxisPosition, 0, minZAxisPosition);
        for (int i = 0; i < xAxisNum; i++)
        {
            for (int j = 0; j < zAxisNum; j++)
            {
                Vector3 position = new Vector3(startPosition.x + length * i, startPosition.y, startPosition.z + length * j);
                Instantiate(basicTile, position, Quaternion.identity);
            }
        }
        graph = new Graph(xAxisNum, zAxisNum, length);
    }

    private List<(int,int)> CalculateOccupiedTiles(Building building, int xPosition, int zPosition)
    {
        int xLength = building.GetXLength();
        int yLength = building.GetYLength();
        int zLength = building.GetZLength();
        List<(int,int)> occupiedTiles = new List<(int,int)> ();

        int startX, startZ; 
        (startX, startZ) = CalculateStartPoint(building, xPosition, zPosition);

        for (int i = 0; i < xLength; i++)
        {
            for (int j = 0; j < zLength; j++)
            {   
                if (j <= building.centerZ && i == building.centerX) continue; // 빌딩 중심까지 빈칸으로 남겨둠
                occupiedTiles.Add((startX + i * length, startZ + j * length));
            }
        }
        return occupiedTiles;
    }

    private (int startX, int startZ) CalculateStartPoint(Building building, int xPosition, int zPosition)
    {   
        int xLength = building.GetXLength();
        int zLength = building.GetZLength();


        

        // Vector3 eulerAngles = currentBuildingRotation.eulerAngles;
        // bool isRotated90Or270 = Math.Abs(Math.Round(eulerAngles.y) % 360) == 90 || Math.Abs(Math.Round(eulerAngles.y) % 360) == 270;

        // // 90도나 270도 회전했다면, X와 Z 길이를 스왑
        // if (isRotated90Or270)
        // {
        //     int temp = xLength;
        //     xLength = zLength;
        //     zLength = temp;
        // }



        int startX = xPosition - (xLength - 1) / 2;
        int startZ = zPosition - (zLength - 1) / 2;
        return (startX, startZ);
    }


    private void BuildObjectOnTile(Building building, int xPosition, int zPosition, Quaternion rotation)
    {
        
        // int xLength = building.GetXLength();
        // int yLength = building.GetYLength();
        // int zLength = building.GetZLength();
        List<(int,int)> occupiedTiles = new List<(int,int)> ();

        int startX, startZ; 
        (startX, startZ) = CalculateStartPoint(building, xPosition, zPosition);


        // for (int i = 0; i < xLength; i++)
        // {
        //     for (int j = 0; j < zLength; j++)
        //     {   
        //         if (j == building.centerZ && i <= building.centerX) continue; // 빌딩 중심까지 빈칸으로 남겨둠
        //         occupiedTiles.Add((startX + i * length, startZ + j * length));
        //     }
        // }

        occupiedTiles = CalculateOccupiedTiles(building, xPosition, zPosition);
    

        if (isConstructable(occupiedTiles))
        {   
            int globalCoorBuilingCenterX = startX + building.centerX;
            int globalCoorBuilingCenterZ = startZ + building.centerZ;

            // Debug.Log($"constructable!!!!!, building center is {globalCoorBuilingCenterX}, {globalCoorBuilingCenterZ}");
            
            Vector3 position = new Vector3(xPosition, (float) 0, zPosition);
            Building newBuilding = Instantiate(building, position, rotation);
            newBuilding.setId(totalBuildingsConstructedNum);
            // Debug.Log($"building.Id: {newBuilding.Id}");

            GameManager.Instance.UpdateGold(-newBuilding.BaseCost);
            graph.ChangeNodeOccupiedState(occupiedTiles, true);
            buildingCenters[newBuilding.Id] = (globalCoorBuilingCenterX, globalCoorBuilingCenterZ);
        }
        else
        {
            Debug.Log("not constructable");
        }
    }

    // bool isConstructable(List<(int,int)> occupiedTiles)
    private bool isConstructable(List<(int x, int z)> TilePositionList)
    {
        foreach (var tilePositionPair in TilePositionList)
        {
            int xPosition = tilePositionPair.x;
            int zPosition = tilePositionPair.z;

            if (xPosition < minXAxisPosition) return false;
            if (xPosition > maxXAxisPosition) return false;
            if (zPosition < minZAxisPosition) return false;
            if (zPosition > maxZAxisPosition) return false;
            if (graph.isOccupied(tilePositionPair)) return false;
        }
        return true;
    }


    public (int centerX, int centerY) GetRandomBuildingCenter()
    {
        if (buildingCenters.Count > 0)
        {
            // 빌딩들이 존재할 경우, 랜덤한 빌딩을 선택하여 그 중심 좌표를 반환
            int randomIndex = UnityEngine.Random.Range(0, buildingCenters.Count);
            int randomBuildingId = 0;

            // Dictionary의 key 값 중 랜덤하게 하나 선택
            int currentIndex = 0;
            foreach (var building in buildingCenters.Keys)
            {
                if (currentIndex == randomIndex)
                {
                    randomBuildingId = building;
                    break;
                }
                currentIndex++;
            }

            return buildingCenters[randomBuildingId];
        }
        else
        {
            // 빌딩이 없을 경우, 랜덤한 좌표를 반환
            int randomX = UnityEngine.Random.Range(minXAxisPosition, maxXAxisPosition);
            int randomY = UnityEngine.Random.Range(minZAxisPosition, maxZAxisPosition);
            Debug.LogWarning("No buildings available. Returning random coordinates.");
            return (randomX, randomY);
        }
    }
}

