using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Rendering;

public class TestCallVFX : MonoBehaviour
{
    public Transform dest;
    public float speed;
    public enum vfxType {Rage, Fear, Disgust, Sadness, Joy};
    public vfxType chooseType;
    
    [Button]
    void PlaySpecificFX()
    {
        VFXManager._instance.UseVFX(chooseType.ToString());
    }

    [Button]
    void SummonRandom()
    {
        int hehe = Random.Range(0,5);

        switch(hehe)
        {
            case 0 :
                VFXManager._instance.UseVFX("Rage");
                break;  
            case 1 :
                VFXManager._instance.UseVFX("Fear");
                break;  
            case 2 :
                VFXManager._instance.UseVFX("Disgust");
                break;  
            case 3 :
                VFXManager._instance.UseVFX("Sadness");
                break; 
            case 4 :
                VFXManager._instance.UseVFX("Joy");
                break; 
        }
    }
}

