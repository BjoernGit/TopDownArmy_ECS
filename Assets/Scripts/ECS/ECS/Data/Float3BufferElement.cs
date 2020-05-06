using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
[InternalBufferCapacity(20)]
public struct Float3BufferElement : IBufferElementData
{
    public float3 Value;
}