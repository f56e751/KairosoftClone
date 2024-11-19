using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI  studentCountText;
    [SerializeField] private TextMeshProUGUI  goldText;
    [SerializeField] private TextMeshProUGUI  professorCountText;
    [SerializeField] private TextMeshProUGUI  fameCountText;
    [SerializeField] private TextMeshProUGUI  monthText;
    [SerializeField] private Button hireProfessorButton;

    [SerializeField] private Button buildButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button studyBuildingButton;
    [SerializeField] private Button dormBuildingButton;
    [SerializeField] private Button libraryBuildingButton;
    [SerializeField] private Button restaurantBuildingButton;

    private bool isBuildingMenuVisible = false; // 건물 메뉴의 표시 상태를 추적하는 변수

    void Start()
    {
        GameManager.Instance.OnStudentCountChanged += UpdateStudentCount;
        GameManager.Instance.OnGoldChanged += UpdateGold;
        GameManager.Instance.OnProfessorCountChanged += UpdateProfessorCount;
        GameManager.Instance.OnFameChanged += UpdateFameCount;
        GameManager.Instance.OnSeasonChanged += UpdateMonth;

        hireProfessorButton.onClick.AddListener(OnHireProfessorButtonClicked);
        buildButton.onClick.AddListener(ToggleBuildingButtons);

        // 버튼 클릭 리스너를 추가
        studyBuildingButton.onClick.AddListener(() => SetBuildingMode(BuildingType.Study));
        dormBuildingButton.onClick.AddListener(() => SetBuildingMode(BuildingType.Dorm));
        libraryBuildingButton.onClick.AddListener(() => SetBuildingMode(BuildingType.Library));
        restaurantBuildingButton.onClick.AddListener(() => SetBuildingMode(BuildingType.Restaurant));

        deleteButton.onClick.AddListener(OnDeleteButtonClicked);

        // 초기 UI 업데이트
        UpdateStudentCount(GameManager.Instance.studentNum);
        UpdateGold(GameManager.Instance.gold);
        UpdateProfessorCount(GameManager.Instance.professorNum);
        UpdateFameCount(GameManager.Instance.fame);
    }

    void SetBuildingMode(BuildingType buildingType)
    {
        BuildManager.Instance.SetCurrentBuilding(buildingType);
        BuildManager.Instance.SetConstructionMode(); // 건설 모드를 활성화
    }

    private void OnDeleteButtonClicked()
    {
        // BuildManager.Instance.DeleteSelectedBuilding(); // 선택된 건물 삭제
    }

    private void ToggleBuildingButtons()
    {
        if (isBuildingMenuVisible)
        {
            HideBuildingButtons();
        }
        else
        {
            ShowBuildingButtons();
        }
        isBuildingMenuVisible = !isBuildingMenuVisible; // 상태 토글
    }


    private void ShowBuildingButtons()
    {
        // 각 건물 건설 버튼을 활성화
        studyBuildingButton.gameObject.SetActive(true);
        dormBuildingButton.gameObject.SetActive(true);
        libraryBuildingButton.gameObject.SetActive(true);
        restaurantBuildingButton.gameObject.SetActive(true);
    }

    public void HideBuildingButtons()
    {
        studyBuildingButton.gameObject.SetActive(false);
        dormBuildingButton.gameObject.SetActive(false);
        libraryBuildingButton.gameObject.SetActive(false);
        restaurantBuildingButton.gameObject.SetActive(false);
    }



    private void OnHireProfessorButtonClicked()
    {
        // 교수 수를 1명 증가시킴
        GameManager.Instance.IncreaseProfessorCount(1);
    }

    private void UpdateStudentCount(int count)
    {
        studentCountText.text = $"Students: {count}";
    }

    private void UpdateGold(float amount)
    {
        goldText.text = $"Gold: {amount}";
    }

    private void UpdateProfessorCount(int count)
    {
        professorCountText.text = $"Professors: {count}";
    }

    private void UpdateFameCount(float amount)
    {
        fameCountText.text = $"Fame: {amount}";
    }
    private void UpdateMonth(int season)
    {
        monthText.text = $"Month: {season}";
    }

    void OnDestroy()
    {
        // 이벤트 구독 해제
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStudentCountChanged -= UpdateStudentCount;
            GameManager.Instance.OnGoldChanged -= UpdateGold;
            GameManager.Instance.OnProfessorCountChanged -= UpdateProfessorCount;
            GameManager.Instance.OnFameChanged -= UpdateFameCount;
        }
    }
}
