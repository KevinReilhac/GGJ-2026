using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : MonoBehaviour
{
    Pool vfxPool;
    [SerializeField] List<VisualEffectAsset> vfxList = new List<VisualEffectAsset>();
    Dictionary<string, VisualEffectAsset> vfxRefDictionnary = new Dictionary<string, VisualEffectAsset>();
    public static VFXManager _instance;

    void Awake()
    {
        if(_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < vfxList.Count; i++)
        {
            vfxRefDictionnary.Add(vfxList[i].name, vfxList[i]);
        }
        vfxPool = GetComponent<Pool>();
    }

    public void GetVFX(string vfxName, Vector3 startPos, Transform destination, float speed)
    {
        VFXObject objRef = vfxPool.GetObject().GetComponent<VFXObject>();
        objRef.SetVFX(vfxRefDictionnary[vfxName], startPos, destination, speed);
        objRef.ThrowVFX();
    }
}
