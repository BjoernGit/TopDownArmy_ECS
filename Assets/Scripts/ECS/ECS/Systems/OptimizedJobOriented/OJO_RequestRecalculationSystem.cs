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
using MyCode;

public class OJO_RequestRecalculationSystem : SystemBase
{
    BeginInitializationEntityCommandBufferSystem m_EntityCommandBufferSystem;

    protected override void OnCreate()
    {
        m_EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        var commandBuffer = m_EntityCommandBufferSystem.CreateCommandBuffer(); //.ToConcurrent();

        //Each Enemy Entity
        Entities
            .WithAll<FollowPlayerTag>()
            .ForEach(
            (Entity entity, ref Translation trans, ref OJO_RequestRecalculationData recalc, 
            in OJO_TargetPointData target, in ActiveNodeIndexData nodeData) =>
            {
                //recalculation of Path section
                if (nodeData.nodeInPath > 0 && nodeData.nodeInPath < target.bufferLength)
                {
                    float timeTemp = recalc.passedTime;
                    timeTemp += deltaTime;
                    recalc.passedTime = timeTemp;

                    //TODO: find a better way to calc the "recalc procedure"
                    float distanceToPlayer = math.distance(trans.Value, target.targetPoint);
                    float distanceStep = (1 / (math.pow(distanceToPlayer, 2f))) * deltaTime * 1000f;

                    float urgencyTemp = recalc.urgency;
                    urgencyTemp += distanceStep;
                    recalc.urgency = urgencyTemp;

                    //when at least 1 second passed and the urgency Value reached 100+ then recalc path
                    if (recalc.passedTime > 1f && recalc.urgency > 100f && recalc.requestState == MyCode.RequestState.NoRequest)
                    {
                        commandBuffer.AddComponent<RecalcTag>(entity);
                        recalc.requestState = RequestState.InitializeRequest;

                        recalc.passedTime = 0f;
                        recalc.urgency = 0f;
                    }
                }
            }).Schedule();
        //ScheduleParallel not supportet because of use of commandbuffer
        //creating a second entities.foreach could work since this is a systembase
    }
}