using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Physics;
using Unity.Collections;
using Unity.Transforms;


public class NavMeshCalculation : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 targetPosition;

    private NavMeshAgent agent;
    private NavMeshPath path;

    [SerializeField]
    private float elapsed;

    private float3 convertPlaceholder;
    Entity entity;
    EntityManager manager;

    bool allreadyCalled = false;

    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();

        //Entity stuff
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        entity = manager.CreateEntity();
    }

    public void CalculatePath(Entity entity)
    {
        //reading of the start and end positions and generation of the path to provide it to the 
        //entity that is related later
        Translation ballposEnemy = manager.GetComponentData<Translation>(entity);
        startPosition = ballposEnemy.Value;
        NavMeshPath path = new NavMeshPath();
        targetPosition = ECS_AppManager.instance.playerPosition;
        NavMesh.CalculatePath(startPosition, targetPosition, NavMesh.AllAreas, path);

        // set nodeindex to 0 so the Job does not run during update
        ActiveNodeIndexData nodeIndex = manager.GetComponentData<ActiveNodeIndexData>(entity);
        ActiveNodeIndexData tempNode = nodeIndex;
        tempNode.nodeInPath = 0;
        manager.SetComponentData(entity, tempNode);

        //when you do not reload the buffer directly from the entity but rather try to cach it then
        //you get spammed by errors telling you the native array has been deallocated and cant be accessed
        DynamicBuffer<Float3BufferElement> float3Buffer = manager.GetBuffer<Float3BufferElement>(entity);
        float3Buffer.Clear();

        for (int i = 0; i < path.corners.Length; i++)
        {
            convertPlaceholder = path.corners[i];
            float3Buffer.Add(new Float3BufferElement { Value = convertPlaceholder });
        }

        //reset the variables since they could have bneen changed by other operations in the mean time
        //by another script
        nodeIndex = manager.GetComponentData<ActiveNodeIndexData>(entity);
        tempNode = nodeIndex;
        tempNode.nodeInPath = 1;
        manager.SetComponentData(entity, tempNode);

        OJO_RequestRecalculationData recalc = manager.GetComponentData<OJO_RequestRecalculationData>(entity);
        recalc.requestState = MyCode.RequestState.NoRequest;
        manager.SetComponentData(entity, recalc);

        return;
    }
}