using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;



public class GridTestInput: MonoBehaviour 
{
    public Camera sceneCamera;
    private Vector3 lastPosition;
    public LayerMask groundLayerMask;
    
    private float maxDistance = 100;
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance, groundLayerMask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }

    public bool GetPlacementInput()
    {
        return Input.GetMouseButtonDown(0);
    }
}