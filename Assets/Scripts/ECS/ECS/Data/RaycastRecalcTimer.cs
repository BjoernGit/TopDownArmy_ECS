using UnityEngine;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct RaycastRecalcTimer : IComponentData
{
    public float timer;
}