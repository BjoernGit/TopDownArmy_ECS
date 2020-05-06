using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using System.Collections;

public class EnemyConversion : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem)
    {
        World.DefaultGameObjectInjectionWorld.EntityManager.AddBuffer<Float3BufferElement>(entity);
        World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<ActiveNodeIndexData>(entity);
        World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<RecalcTag>(entity);
        World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<RaycastRecalcTimer>(entity);
        World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<OJO_TargetPointData>(entity);
        World.DefaultGameObjectInjectionWorld.EntityManager.AddComponent<OJO_RequestRecalculationData>(entity);

        OJO_RequestRecalculationData recalc = manager.GetComponentData<OJO_RequestRecalculationData>(entity);
        RaycastRecalcTimer rayRecalc = manager.GetComponentData<RaycastRecalcTimer>(entity);

        recalc.requestState = MyCode.RequestState.InitializeRequest;
        manager.SetComponentData(entity, recalc);

        rayRecalc.timer = UnityEngine.Random.Range(0.00f, 2.00f);
        manager.SetComponentData(entity, rayRecalc);

    }
}