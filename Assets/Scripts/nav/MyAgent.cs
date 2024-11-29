using UnityEngine;
using UnityEngine.AI;

public class MyAgent : MonoBehaviour
{
    NavMeshAgent agent;
    public LayerMask walkableLayer;  // 사용자가 정의한 walkable layer
    private NavMeshQueryFilter filter;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        filter = new NavMeshQueryFilter();
        filter.areaMask = walkableLayer.value;  // 이동 가능 영역 마스크 설정
        filter.agentTypeID = agent.agentTypeID;  // Agent Type 설정
        filter.areaMask = NavMesh.AllAreas;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;  // RaycastHit를 hitInfo로 변경하여 중복을 피함

            Debug.Log("clicked");
            if (Physics.Raycast(ray, out hitInfo))
            {   
                Debug.Log("raycast ok");
                Vector3 targetPosition = hitInfo.point;  // 클릭한 위치를 목표 위치로 설정
                NavMeshHit navMeshHit;  // NavMeshHit을 navMeshHit로 명명
                agent.SetDestination(targetPosition);

                // // SamplePosition 호출 시 사용할 변수 이름과 함수 시그니처를 올바르게 맞춤
                // if (NavMesh.SamplePosition(targetPosition, out navMeshHit, 1.0f, filter))
                // {
                //     agent.SetDestination(navMeshHit.position);  // 올바른 위치를 설정
                // }
            }
        }
    }
}
