using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // IPointerClickHandler를 사용하기 위한 네임스페이스

public class BuildFollowMouse : MonoBehaviour, IPointerClickHandler
{
    public Button movableButton; // 움직일 UIButton
    private bool isMoving = false; // 마우스 따라 움직일지 여부

    void Update()
    {
        if (isMoving)
        {
            // 마우스 위치를 캔버스 좌표로 변환
            Vector3 mousePosition = Input.mousePosition;

            // RectTransformUtility를 사용하여 마우스 위치를 캔버스 좌표로 변환
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)movableButton.transform.parent, // 부모 RectTransform
                mousePosition,
                null, // 카메라가 없다면 null
                out Vector2 localPoint
            );

            movableButton.transform.localPosition = localPoint; // UIButton의 로컬 위치 설정
        }
    }

    public void StartMoving()
    {
        Debug.Log("마우스를 따라 움직이기 시작");
        isMoving = true; // 마우스를 따라 움직이기 시작
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 마우스 클릭 시 UIButton 고정
        if (isMoving)
        {
            isMoving = false; // 움직임 종료
            movableButton.gameObject.SetActive(false); // 움직일 버튼 비활성화
            Debug.Log("버튼이 비활성화되었습니다.");
        }
    }

    private void OnEnable()
    {
        StartMoving(); // 마우스를 따라 움직이기 시작
    }

    private void OnDisable()
    {
        // Canvas 오브젝트 찾기
        GameObject canvasObject = GameObject.Find("Canvas"); // Canvas 이름에 맞게 변경

        if (canvasObject != null)
        {
            // Canvas의 자식 중 BuildScene(Clone) 찾기
            Transform buildSceneTransform = canvasObject.transform.Find("BuildScene(Clone)");
            if (buildSceneTransform != null)
            {
                buildSceneTransform.gameObject.SetActive(true); // BuildScene(Clone) 활성화
                Debug.Log("BuildScene(Clone)이 활성화되었습니다.");
            }
            else
            {
                Debug.LogError("BuildScene(Clone)을 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("Canvas를 찾을 수 없습니다.");
        }

        // 버튼이 비활성화될 때 isMoving 상태 초기화
        isMoving = false; // 비활성화 시 이동 상태 초기화
    }

}
