using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;

//[AlwaysSynchronizeSystem]
public class OJO_RotateTowardsSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        //Each Enemy Entity
        Entities
            .WithAll<FollowPlayerTag>()
            .WithoutBurst()
            .ForEach(
            (Entity entity, ref PhysicsVelocity vel, ref Rotation rot, in Translation trans
            , in ActiveNodeIndexData nodeData, in OJO_TargetPointData target) =>
            {
                //apply rotation towards the next pathpoint
                if (nodeData.nodeInPath > 0 && nodeData.nodeInPath < target.bufferLength)
                {
                    float3 enemyToPoint = target.targetPoint - trans.Value;
                    enemyToPoint.y = 0f;
                    enemyToPoint = math.normalize(enemyToPoint);
                    Quaternion newRotation = Quaternion.LookRotation(enemyToPoint);
                    rot.Value = math.slerp(rot.Value, newRotation, .5f * deltaTime);
                }
            }).ScheduleParallel();
    }

    public static quaternion GetWorldRotation(LocalToWorld ltw)
    {
        return quaternion.LookRotationSafe(ltw.Forward, ltw.Up);
    }

}