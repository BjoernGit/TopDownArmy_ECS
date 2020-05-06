using Unity.Entities;
using UnityEngine;

public class PlayerConversion : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager manager, GameObjectConversionSystem conversionSystem)
    {
        ECS_AppManager.instance.playerEntity = entity;
    }
}