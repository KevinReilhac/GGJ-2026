using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] int objectCount;
    [SerializeField] GameObject prefabObject;
    List<GameObject> objList = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        MakeAllObjects();
    }

    GameObject MakeInstance()
    {
        GameObject obj = Instantiate(prefabObject,transform);
        obj.GetComponent<PoolObject>().SetPool(this);

        obj.SetActive(false);
        return obj;
    }

    void MakeAllObjects()
    {
        for(int i = 0; i < objectCount ; i++)
        {
            objList.Add(MakeInstance());
        }
    }

    public GameObject GetObject()
    {
        for(int i = 0;  i < objList.Count; i++)
        {
            if(!objList[i].activeSelf)
                return objList[i];
        }

        objList.Add(MakeInstance());
        return objList[objList.Count - 1];
    }
}
