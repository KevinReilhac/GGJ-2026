using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : MonoBehaviour
{
    Pool vfxPool;
    [SerializeField] List<VisualEffectAsset> vfxList = new List<VisualEffectAsset>();
    Dictionary<string, VisualEffectAsset> vfxRefDictionnary = new Dictionary<string, VisualEffectAsset>();
    Dictionary<string, string> translator = new Dictionary<string, string>();
    public static VFXManager _instance;
    [SerializeField] Transform startPos;
    [SerializeField] Transform destination;
    [SerializeField] Transform shieldPosition;
    float speed;

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

        translator.Add("Sad", "Sadness");
        translator.Add("Joy", "Joy");
        translator.Add("Angry", "Rage");
        translator.Add("Disgust", "Disgust");
        translator.Add("Scare", "Fear");
    }

    public void GetVFX(List<EmotionStat> emotion, bool bounce)
    {
        UseVFX(ParseAttack(emotion), bounce);
    }

    public void UseVFX(string vfxName, bool bounce = false)
    {
        SetSpeed(vfxName);

        VFXObject objRef = vfxPool.GetObject().GetComponent<VFXObject>();
        objRef.SetVFX(vfxRefDictionnary[vfxName], startPos.position, destination, speed);
        
        objRef.ThrowVFX(bounce);
    }

    public void UseShield()
    {
        VFXObject objRef = vfxPool.GetObject().GetComponent<VFXObject>();
        objRef.SetVFX(vfxRefDictionnary["Bounce"], shieldPosition.position, shieldPosition, speed, false);
        
        objRef.ThrowVFX(false);
    }

    string ParseAttack(List<EmotionStat> emotion)
    {
        string highestStatName = "";
        int highestStat = 0;
        for(int i = 0; i< emotion.Count; i++)
        {
            if(emotion[i].stat > highestStat)
            {
                highestStat = emotion[i].stat;
                highestStatName = emotion[i].emotionType.ToString();
            }
        }
        highestStatName = translator[highestStatName];

        return highestStatName;
    }

    void SetSpeed(string newName)
    {
        switch(newName)
        {
            case "Rage" :
                speed = 2;
                break;  
            case "Fear" :
                speed= 1;
                break;  
            case "Disgust" :
                speed= 1;
                break;  
            case "Sadness" :
                speed= 0.5f;
                break; 
            case "Joy" :
                speed= 0.5f;
                break; 
            default :
                speed = 1;
                break;
        }
    }
}
