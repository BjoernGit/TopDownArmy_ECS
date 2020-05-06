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

///------------------------------------------------
///Uses Buffer and needs to stay on the Mainthread
///the loading of the buffer could be done maybe twice a second and ont every frame
///This System should be split into EntityJunks to work better its to much for one frame
///------------------------------------------------


//[AlwaysSynchronizeSystem]
public class OJO_LoadBufferSystem : SystemBase
{
    private float timerToRunSystem;

    protected override void OnCreate()
    {
        EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        //Run the actual system only a few times a second
        if (timerToRunSystem > 0.3f)
        {
            //Each Enemy Entity
            Entities
                .WithAll<FollowPlayerTag>()
                .WithoutBurst()
                .ForEach((Entity entity, ref ActiveNodeIndexData nodeData, ref OJO_TargetPointData target) =>
                {
                    //load path that is calculated by NavMeshCalculation
                    BufferFromEntity<Float3BufferElement> buffer = GetBufferFromEntity<Float3BufferElement>();
                    DynamicBuffer<Float3BufferElement> enemyBuffer = buffer[entity];
                    if (nodeData.nodeInPath > 0 && nodeData.nodeInPath < enemyBuffer.Length)
                    {

                        target.bufferLength = enemyBuffer.Length;
                        target.targetPoint = enemyBuffer[nodeData.nodeInPath].Value;

                        //as long as the buffer length is longer than the next desired point position in the buffer
                        //the next position should be assigned
                        if (target.bufferLength > nodeData.nodeInPath + 1)
                        {
                            target.nextTargetPoint = enemyBuffer[nodeData.nodeInPath + 1].Value;
                        }
                        else
                        {
                            target.nextTargetPoint = enemyBuffer[nodeData.nodeInPath].Value;
                        }
                    }
                }).Run();
            timerToRunSystem = 0f;
        }
        else
        {
            timerToRunSystem += deltaTime;
        }

    }
}