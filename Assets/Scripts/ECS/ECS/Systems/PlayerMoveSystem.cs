using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;

public class PlayerMoveSystem : SystemBase
{

    // Update is called once per frame
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        //Each Enemy Entity
        Entities
            .WithAll<PlayerTag>()
            .ForEach(
            (Entity entity, ref PhysicsVelocity vel, ref Translation trans) =>
            {


            }).ScheduleParallel();
    }
}
