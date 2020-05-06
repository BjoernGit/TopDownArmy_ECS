using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Burst;
using Unity.Collections;
using Unity.Physics.Systems;
using Unity.Physics.Extensions;
using System;

///------------------------------------------------
///Uses CommandBuffer and needs to stay on the Mainthread
///maybe the commandbuffer is not really necessarry and I can replace the tag with a variable?
///------------------------------------------------

[AlwaysSynchronizeSystem]
public class RecalcSystem : JobComponentSystem
{
    BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Entities
            .WithAll<RecalcTag>()
            .WithStructuralChanges()
            .ForEach((Entity entity, ref OJO_RequestRecalculationData recalc) =>
            {
                if (recalc.requestState == MyCode.RequestState.InitializeRequest)
                {
                    var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer(); //.ToConcurrent();

                    ECS_AppManager.instance.requestNewPath(entity);

                    commandBuffer.RemoveComponent<RecalcTag>(entity);
                }
            }).Run();

        m_EntityCommandBufferSystem.AddJobHandleForProducer(inputDeps);
        return default;
    }
}