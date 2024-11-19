using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

// this class controls unit movement on tile
public class UnitMover : MonoBehaviour
{   

    public GameObject unitToMove;
    [SerializeField] public float yAxis = 0;
    [SerializeField] public float speed = 10f;
    float moveThreshold = 0.1f;
    public bool isArrived = false;
    bool isFirstSearch = true;
    List<Vector3> shortestPath;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if (!isArrived)
        // {
        //     MoveUnit(unitToMove, (10,10));
        //     Debug.Log("unit move");
        // }
    }

    void TestInstanciate(GameObject unit)
    {   
        Vector3 position = new Vector3(0,yAxis,0);
        Instantiate(unit, position, Quaternion.identity);
    }

    bool isPathOccupied()
    {
        foreach (Vector3 position in shortestPath)
        {
            (int,int) nodeKey = ((int) position.x , (int) position.z);
            if (BuildManager.Instance.graph.isOccupied(nodeKey)) return true;
        }
        return false;
    }


    public void StartMoveUnit((int x, int z) targetPosition)
    {
        // MoveUnit을 코루틴으로 실행
        StartCoroutine(MoveUnit(targetPosition));
    }


    private IEnumerator MoveUnit((int x, int z) targetPosition)
    {
        isFirstSearch = true;
        isArrived = false;

        while (!isArrived)
        {
            float unitXposition = unitToMove.transform.position.x;
            float unitZposition = unitToMove.transform.position.z;
            float leftDistance = (float)Math.Sqrt(Math.Pow(unitXposition - targetPosition.x, 2) + Math.Pow(unitZposition - targetPosition.z, 2));

            // 타겟 위치까지 왔는지 확인
            if (leftDistance < moveThreshold)
            {
                isArrived = true;
                isFirstSearch = true;
                shortestPath?.Clear();
                yield break; // 코루틴 종료
            }

            // BuildManager와 graph가 올바르게 초기화되었는지 확인
            if (BuildManager.Instance == null || BuildManager.Instance.graph == null)
            {
                Debug.LogError("BuildManager or graph is not initialized!");
                yield break; // 코루틴 종료
            }

            // 연산량을 줄이기 위해 움직이기 시작했을때만 최단경로 탐색을 진행함
            if (isFirstSearch)
            {
                isFirstSearch = false;
                shortestPath = BuildManager.Instance.graph.GetShortestPath(((int)unitXposition, (int)unitZposition), targetPosition);

                if (shortestPath == null || shortestPath.Count == 0)
                {
                    Debug.LogError("Shortest path could not be found or is empty!");
                    yield break; // 코루틴 종료
                }
            }

            // 이동 도중에 path에 건물이 생긴 경우 경로를 다시 탐색함
            if (shortestPath != null && isPathOccupied())
            {
                shortestPath = BuildManager.Instance.graph.GetShortestPath(((int)unitXposition, (int)unitZposition), targetPosition);
            }

            if (shortestPath != null && shortestPath.Count > 0)
            {
                Vector3 nextPosition = shortestPath[0]; // Get the next position from the path
                nextPosition.y = yAxis;
                Vector3 moveDirection = (nextPosition - unitToMove.transform.position).normalized; // Calculate the direction to move in
                unitToMove.transform.position += moveDirection * speed * Time.deltaTime; // Move the unit

                // Check if close enough to next waypoint to consider it "reached"
                if (Vector3.Distance(unitToMove.transform.position, nextPosition) < moveThreshold)
                {
                    shortestPath.RemoveAt(0); // Remove the reached waypoint from the path
                }
            }

            yield return null; // 다음 프레임까지 대기
        }
    }
}
