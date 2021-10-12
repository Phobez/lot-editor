using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{
    public int objectID;
    public GameObject objectToPool;
}

public class ObjectPooler : MonoBehaviour
{
    [SerializeField]
    private List<ObjectPoolItem> itemsToPool = new List<ObjectPoolItem>();
    
    public static ObjectPooler Instance { get; private set; }

    private List<GameObject> pooledObjects = null;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        pooledObjects = new List<GameObject>();
    }

    public GameObject GetPooledObject(int ID)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].GetComponent<BuildObject>().GetObjID() == ID) return pooledObjects[i];
        }

        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.objectID == ID)
            {
                GameObject obj = Instantiate(item.objectToPool);
                obj.SetActive(true);
                pooledObjects.Add(obj);
                return obj;
            }
        }

        return null;
    }
}
