using Unity.Entities;
using MyCode;

[GenerateAuthoringComponent]
public struct OJO_RequestRecalculationData : IComponentData
{
    public RequestState requestState; //RequestProcessing ended up unused ... could be deleted
    public float passedTime;
    public float urgency;
}