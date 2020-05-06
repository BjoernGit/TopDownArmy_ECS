using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Burst;
using Unity.Collections;

//[AlwaysSynchronizeSystem]
public class OJO_MovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        //Each Enemy Entity
        Entities
            .WithAll<FollowPlayerTag>()
            .ForEach(
            (Entity entity, ref PhysicsVelocity vel, in Translation trans
            , in ActiveNodeIndexData nodeData, in OJO_TargetPointData target) =>
            {
                //apply force towards the next pathpoint
                if (nodeData.nodeInPath > 0 && nodeData.nodeInPath < target.bufferLength)
                {
                    float3 newVel = vel.Linear.xyz;
                    float3 targetDirection = math.normalize(target.targetPoint - trans.Value);
                    newVel += targetDirection * deltaTime * 6.5f;
                    vel.Linear.xyz = newVel;
                }
            }).ScheduleParallel();
    }
}