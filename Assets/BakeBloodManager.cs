using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeBloodManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> objectsToAddList = new List<GameObject>();
    [SerializeField] private GameObject[] objectsArray;
    private MB3_MultiMeshBaker mbd;
    [SerializeField] private bool readyToBake = true;

    //---Singleton-Pattern-----------------------
    public static BakeBloodManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BakeBloodManager in scene!");
            Destroy(this.gameObject);
            return;
        }
        instance = this;

    }
    //---Singleton-Pattern-ENDE------------------

    private void Start()
    {
        mbd = GetComponentInChildren<MB3_MultiMeshBaker>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (readyToBake && objectsToAddList.Count > 0)
        {
            objectsArray = objectsToAddList.ToArray();
            StartCoroutine(BakeLoad());
            objectsToAddList.Clear();
        }
    }

    public void AddObjectToBakePool(GameObject addThisObject)
    {
        objectsToAddList.Add(addThisObject);
    }

    IEnumerator BakeLoad()
    {
        readyToBake = false;

        //sendObjects to Meshbaker
        mbd.AddDeleteGameObjects(objectsArray, null, true);
        mbd.Apply();
        objectsArray = null;
        yield return new WaitForSeconds(1.5f);

        readyToBake = true;
    }
}
