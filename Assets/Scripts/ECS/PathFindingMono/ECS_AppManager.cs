using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class ECS_AppManager : MonoBehaviour
{
    public NavMeshCalculation NMCalc;

    public Vector3 playerPosition;
    public Vector3 nextPoint;
    public Vector3 enemyPosition;

    public Entity playerEntity;
    public Entity enemyEntity;

    public List<int> requestingEntities = new List<int>();

    private EntityManager manager;


    //---Singleton-Pattern-----------------------
    public static ECS_AppManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one ECS_AppManager in scene!");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        //---Singleton-Pattern-ENDE------------------

        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerEntity == null)
        {
            //return;
        }
        else
        {
            Translation ballpos = manager.GetComponentData<Translation>(playerEntity);
            playerPosition = ballpos.Value;
        }

    }

    public void requestNewPath(Entity entity)
    {
        NMCalc.CalculatePath(entity);
    }

}