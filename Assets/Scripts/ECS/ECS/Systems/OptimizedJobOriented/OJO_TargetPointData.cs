using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct OJO_TargetPointData : IComponentData
{
    public float3 targetPoint;
    public float3 nextTargetPoint;
    public int bufferLength;

}