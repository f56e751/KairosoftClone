using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleButtons : MonoBehaviour
{
    [System.Serializable]
    public class ScreenConfig
    {
        public string screenName;       // 스크린 이름 (Button 이름과 연결 가능)
        public GameObject screenPrefab; // 생성할 Screen 프리팹
        [HideInInspector] public GameObject currentScreen; // 현재 생성된 Screen 오브젝트 참조
    }

    public Canvas canvas;                // Canvas 참조
    public ScreenConfig[] screens;       // 각 스크린 프리팹 및 상태 관리 배열
    public Button[] buttons;             // 버튼 배열

    private void Start()
    {
        // 각 버튼에 클릭 이벤트 연결
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // 이벤트에 클로저 문제 방지를 위해 지역 변수 사용
            buttons[i].onClick.AddListener(() => ToggleScreenObject(index));
        }
    }

    // 해당 index의 버튼이 눌렸을 때 호출되는 메서드
    public void ToggleScreenObject(int index)
    {
        if (screens[index].currentScreen == null)
        {
            // Screen 프리팹 생성
            screens[index].currentScreen = Instantiate(screens[index].screenPrefab, canvas.transform);

            // Canvas 중앙에 위치 조정
            RectTransform rectTransform = screens[index].currentScreen.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;

            // 닫기 버튼 이벤트 등록
            Transform closingButtonTransform = screens[index].currentScreen.transform.Find("CloseButton");
            if (closingButtonTransform != null)
            {
                Button closingButton = closingButtonTransform.GetComponent<Button>();
                if (closingButton != null)
                {
                    closingButton.onClick.AddListener(() => CloseScreen(index));
                }
            }

            // 드래그 기능 추가
            AddDragFunctionality(screens[index].currentScreen);
        }
    }

    // 스크린을 닫는 메서드
    private void CloseScreen(int index)
    {
        if (screens[index].currentScreen != null)
        {
            Destroy(screens[index].currentScreen);
            screens[index].currentScreen = null;
        }
    }

    // 드래그 기능 추가 메서드
    private void AddDragFunctionality(GameObject screen)
    {
        // 부모 오브젝트에만 드래그 이벤트를 처리하도록 설정
        DragHandler dragHandler = screen.AddComponent<DragHandler>();

        // 드래그가 가능한 RectTransform 및 Canvas를 전달
        RectTransform rectTransform = screen.GetComponent<RectTransform>();
        dragHandler.Initialize(rectTransform, canvas);
    }
}

// 드래그 이벤트 처리 클래스
public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPosition;

    // 초기화 메서드
    public void Initialize(RectTransform rectTransform, Canvas canvas)
    {
        this.rectTransform = rectTransform;
        this.canvas = canvas;
    }

    // 드래그 시작 시 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
    }

    // 드래그 중 호출
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = originalPosition + eventData.delta / canvas.scaleFactor;
    }
}
