using Unity.Entities;

[GenerateAuthoringComponent]
public struct ActiveNodeIndexData : IComponentData
{
    public int nodeInPath;
    public int pathIndex;
}