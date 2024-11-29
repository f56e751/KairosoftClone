using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class GridTest : MonoBehaviour
{   
    public GameObject cube, blockPrefab;
    public Grid grid;
    public GridTestInput gridInput;
    public NavMeshSurface navMeshSurface;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 selectedPosition = gridInput.GetSelectedMapPosition();
        
        Vector3Int cellPosition = grid.WorldToCell(selectedPosition);
        cube.transform.position = grid.GetCellCenterWorld(cellPosition);

        if (gridInput.GetPlacementInput())
        {   
            // Debug.Log(selectedPosition);
            // // Instantiate(blockPrefab, cube.transform.position, Quaternion.identity);
            // Instantiate(blockPrefab, grid.GetCellCenterWorld(cellPosition), Quaternion.identity);
            // navMeshSurface.BuildNavMesh();

            // BuildNavMeshDelayed(cellPosition);
            StartCoroutine(BuildNavMeshDelayed(cellPosition));
            
        }
    }

    IEnumerator BuildNavMeshDelayed(Vector3Int cellPosition)
    {
        // Instantiate the blockPrefab and store the reference
        GameObject newBlock = Instantiate(blockPrefab, grid.GetCellCenterWorld(cellPosition), Quaternion.identity);

        // Wait until next frame
        yield return null;

        // Now build the NavMesh
        navMeshSurface.BuildNavMesh();
    }
}
