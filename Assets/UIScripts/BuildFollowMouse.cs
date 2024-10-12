using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // IPointerClickHandler�� ����ϱ� ���� ���ӽ����̽�

public class BuildFollowMouse : MonoBehaviour, IPointerClickHandler
{
    public Button movableButton; // ������ UIButton
    private bool isMoving = false; // ���콺 ���� �������� ����

    void Update()
    {
        if (isMoving)
        {
            // ���콺 ��ġ�� ĵ���� ��ǥ�� ��ȯ
            Vector3 mousePosition = Input.mousePosition;

            // RectTransformUtility�� ����Ͽ� ���콺 ��ġ�� ĵ���� ��ǥ�� ��ȯ
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)movableButton.transform.parent, // �θ� RectTransform
                mousePosition,
                null, // ī�޶� ���ٸ� null
                out Vector2 localPoint
            );

            movableButton.transform.localPosition = localPoint; // UIButton�� ���� ��ġ ����
        }
    }

    public void StartMoving()
    {
        Debug.Log("���콺�� ���� �����̱� ����");
        isMoving = true; // ���콺�� ���� �����̱� ����
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ���콺 Ŭ�� �� UIButton ����
        if (isMoving)
        {
            isMoving = false; // ������ ����
            movableButton.gameObject.SetActive(false); // ������ ��ư ��Ȱ��ȭ
            Debug.Log("��ư�� ��Ȱ��ȭ�Ǿ����ϴ�.");
        }
    }

    private void OnEnable()
    {
        StartMoving(); // ���콺�� ���� �����̱� ����
    }

    private void OnDisable()
    {
        // Canvas ������Ʈ ã��
        GameObject canvasObject = GameObject.Find("Canvas"); // Canvas �̸��� �°� ����

        if (canvasObject != null)
        {
            // Canvas�� �ڽ� �� BuildScene(Clone) ã��
            Transform buildSceneTransform = canvasObject.transform.Find("BuildScene(Clone)");
            if (buildSceneTransform != null)
            {
                buildSceneTransform.gameObject.SetActive(true); // BuildScene(Clone) Ȱ��ȭ
                Debug.Log("BuildScene(Clone)�� Ȱ��ȭ�Ǿ����ϴ�.");
            }
            else
            {
                Debug.LogError("BuildScene(Clone)�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("Canvas�� ã�� �� �����ϴ�.");
        }

        // ��ư�� ��Ȱ��ȭ�� �� isMoving ���� �ʱ�ȭ
        isMoving = false; // ��Ȱ��ȭ �� �̵� ���� �ʱ�ȭ
    }

}
