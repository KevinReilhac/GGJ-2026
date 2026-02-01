using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct SFXPair
{
    public AudioClip sfxFlare;
    public AudioClip sfxExplosion;
    public SFXPair (AudioClip clip1, AudioClip clip2)
    {
        sfxFlare = clip1;
        sfxExplosion = clip2;
    }
}

public class SFXPlayer : MonoBehaviour
{
    Pool sourcePool;
    [Header("Attack sounds")]
    [SerializeField] List<SFXPair> SFXPairs = new List<SFXPair>();
    [SerializeField] List<string> SFXPairsName = new List<string>();
    Dictionary<string, SFXPair> sfxBank = new Dictionary<string, SFXPair>();
    public static SFXPlayer _instance;

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
        sourcePool = GetComponent<Pool>();

        for(int i = 0; i < SFXPairsName.Count; i++)
        {
            sfxBank.Add(SFXPairsName[i], new SFXPair(SFXPairs[i].sfxFlare, SFXPairs[i].sfxExplosion));
        }
    }

    public void MakeAttackSound(string attackName, bool isFlare, Vector3 position)
    {
        AudioSource source = sourcePool.GetObject().GetComponent<AudioSource>();

        if(isFlare)
            source.clip = sfxBank[attackName].sfxFlare;
        else
            source.clip = sfxBank[attackName].sfxExplosion;

        source.transform.position = position;
        source.gameObject.SetActive(true);
        source.Play();
    }
}
