using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f; // 카메라 이동 속도
    public float scrollSpeed = 5f; // 스크롤 속도로 카메라 줌 인/아웃 속도 조정
    public float rotationSpeed = 50f; // 카메라 회전 속도
    public float dragRotationSpeed = 5f; // 드래그로 회전할 때의 속도

    private Vector3 lastMousePosition; // 마지막 마우스 위치를 저장

    void Update()
    {
        // WASD를 이용한 카메라 이동
        float horizontal = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        Vector3 forwardMovement = transform.forward * vertical;
        Vector3 rightMovement = transform.right * horizontal;

        // Y축 이동 제거 후, 정규화하여 일관된 속도 유지
        forwardMovement.y = 0;
        rightMovement.y = 0;

        Vector3 movement = forwardMovement + rightMovement;
        if (movement.magnitude > 0) // 방향 벡터가 0이 아닐 때만 정규화
        {
            movement = movement.normalized * moveSpeed * Time.deltaTime;
        }

        transform.position += movement;

        // // 마우스 스크롤을 이용한 줌 인/아웃
        // float scroll = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        // transform.Translate(0, 0, scroll, Space.Self);

        // Q와 E를 사용한 카메라 회전
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0, Space.World);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        }

        // Alt + 마우스 왼쪽 버튼 드래그로 카메라 시점 조정
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastMousePosition = Input.mousePosition;
            }

            Vector3 delta = Input.mousePosition - lastMousePosition;
            float rotationX = delta.y * dragRotationSpeed * Time.deltaTime;
            float rotationY = -delta.x * dragRotationSpeed * Time.deltaTime;

            transform.eulerAngles += new Vector3(rotationX, rotationY, 0);
            lastMousePosition = Input.mousePosition;
        }
    }
}
