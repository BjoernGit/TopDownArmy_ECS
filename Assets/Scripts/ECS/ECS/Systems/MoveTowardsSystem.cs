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

/// <summary>
/// Uses random numbers and needs to stay on the mainthread
/// to make this more performant, I have to implement another way of creating a random number
/// for each entity
/// </summary>

#region Working Version

public class MoveTowardsSystem : SystemBase
{

    protected override void OnCreate()
    {
        EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        CollisionWorld collisionWorld;
        PhysicsWorld physicsWorldSystem;
        physicsWorldSystem = World.GetExistingSystem<BuildPhysicsWorld>().PhysicsWorld;
        collisionWorld = physicsWorldSystem.CollisionWorld;

        float deltaTime = Time.DeltaTime;
        //Each Enemy Entity
        Entities
            .WithAll<FollowPlayerTag>()
            .WithNone<RecalcTag>()
            .ForEach(
            (Entity entity, ref PhysicsVelocity vel, ref Translation trans, ref ActiveNodeIndexData nodeData
            , ref RaycastRecalcTimer rayRecalc, in OJO_TargetPointData target) =>
            {
                ///--------------------------------------------------------------------------------
                //Section with raycasting to check if next point is allready in sight and if a
                //switch to the next point should happen

                //Raycasttimer check to keep the raycast from running every cycle
                rayRecalc.timer += deltaTime;

                if (rayRecalc.timer > 2f)
                {
                    rayRecalc.timer = UnityEngine.Random.Range(0.00f, 1.00f);
                    if (nodeData.nodeInPath > 0 && nodeData.nodeInPath < target.bufferLength - 1)
                    {
                        float3 EndUp = target.nextTargetPoint;
                        EndUp.y += 0.1f;

                        Unity.Physics.RaycastHit hit;
                        RaycastInput rayIn = new RaycastInput()
                        {

                            Start = trans.Value,
                            End = EndUp,
                            Filter = new CollisionFilter()
                            {
                                BelongsTo = ~(1u << 0),
                                CollidesWith = ~(1u << 0),
                                GroupIndex = 0
                            }
                        };
                        //draw a blue line to the point you are heading
                        Debug.DrawLine(trans.Value, target.targetPoint, Color.blue, 2f);

                        //when there is a hit then draw me a red line
                        bool haveHit = collisionWorld.CastRay(rayIn, out hit);
                        if (haveHit)
                        {
                            float3 hitUp = hit.Position;
                            hitUp.y++;

                            //Debug.DrawLine(hit.Position, hitUp, Color.red, 2f);
                        }
                        if (!haveHit && nodeData.nodeInPath < target.bufferLength)
                        {
                            int tempValue = nodeData.nodeInPath;
                            tempValue += 1;
                            nodeData.nodeInPath = tempValue;
                        }
                    }
                }

            }).Run(); //ScheduleParallel();

        //return default;
    }
}

#endregion
