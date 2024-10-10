using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Building: MonoBehaviour
{   

    private static int nextId = 0;  // 빌딩의 고유 ID를 생성하기 위한 정적 변수
    public int Id { get; private set; }  // 고유 ID
    public int XLength { get; protected set; }
    public int YLength { get; protected set; }
    public int ZLength { get; protected set; }
    public float BaseCost { get; protected set; }
    public float MaintenanceCost { get; protected set; }
    public int centerX => (XLength - 1) / 2; // local coordinate
    public int centerZ => (YLength - 1) / 2; // local coordinate

    protected Building(int xLength, int yLength, int zLength, float baseCost, float maintenanceCost)
    {
        Id = nextId++;  // 고유 ID 할당 후 증가
        // Id = BuildManager.Instance.totalBuildingsConstructedNum;
        XLength = xLength;
        YLength = yLength;
        ZLength = zLength;
        BaseCost = baseCost;
        MaintenanceCost = maintenanceCost;
    }

    public void setId(int id)
    {
        Id = id;
    }

    public int GetXLength()
    {
        return XLength;
    }

    public int GetYLength()
    {
        return YLength;
    }

    public int GetZLength()
    {
        return ZLength;
    }
}
