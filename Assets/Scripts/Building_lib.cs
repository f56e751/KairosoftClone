using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public class Building_lib : Building
{
    // 상수로 고정된 값들을 정의
    private static readonly int FixedXLength = 8;
    private static readonly int FixedYLength = 3;
    private static readonly int FixedZLength = 3;
    private static readonly float FixedBaseCost = 5000f;
    private static readonly float FixedMaintenanceCost = 500.0f;

    // 생성자에서 상수 값들을 사용
    public Building_lib()
        : base(FixedXLength, FixedYLength, FixedZLength, FixedBaseCost, FixedMaintenanceCost)
    {
    }
}