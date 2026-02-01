using UnityEngine;
using NaughtyAttributes;

public class TestCallVFX : MonoBehaviour
{
    public string vfxName;
    public Transform dest;
    public float speed;
    [Button]
    void SummonVFX()
    {
        VFXManager._instance.GetVFX(vfxName, transform.position, dest, speed);
    }

    [Button]
    void SummonRandom()
    {
        Random.InitState(Random.Range(0,150));
        int hehe = Random.Range(0,5);

        switch(hehe)
        {
            case 0 :
                VFXManager._instance.GetVFX("Rage", transform.position, dest, 2);
                break;  
            case 1 :
                VFXManager._instance.GetVFX("Fear", transform.position, dest, 1);
                break;  
            case 2 :
                VFXManager._instance.GetVFX("Disgust", transform.position, dest, 1);
                break;  
            case 3 :
                VFXManager._instance.GetVFX("Sadness", transform.position, dest, 0.5f);
                break; 
            case 4 :
                VFXManager._instance.GetVFX("Joy", transform.position, dest, 0.5f);
                break; 
        }
    }
}
