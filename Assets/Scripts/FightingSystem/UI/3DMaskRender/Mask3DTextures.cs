
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Mask3DTextures", menuName = "ScriptableObjects/Mask3DTextures")]
public class Mask3DTextures : ScriptableObject
{
    private static Mask3DTextures instance = null;
    public static Mask3DTextures Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load<Mask3DTextures>("Mask3DTextures");
            return instance;
        }
    }

    [System.Serializable]
    private class Mask3DTextureItem
    {
        public EEmotion EmotionType;
        public RenderTexture Texture;
    }
    [SerializeField] private List<Mask3DTextureItem> textures;

    private Dictionary<EEmotion, RenderTexture> texturesDict;
    private Dictionary<EEmotion, RenderTexture> TexturesDict
    {
        get
        {
            if (texturesDict == null)
                texturesDict = textures.ToDictionary(item => item.EmotionType, item => item.Texture);
            return texturesDict;
        }
    }

    public RenderTexture GetTexture(EEmotion emotionType)
    {
        return TexturesDict.GetValueOrDefault(emotionType, null);
    }
}