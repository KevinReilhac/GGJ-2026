using FoxEdit;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VoxelTextureGenerator : EditorWindow
{
    public static VoxelTextureGenerator currentWindow { get; private set; }


    [MenuItem("FoxEdit/Texture generator")]
    public static void Open()
    {
        currentWindow = GetWindow<VoxelTextureGenerator>();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Generate"))
            GenerateTextures();
    }

    private void GenerateTextures()
    {
        FoxEditSettings foxEditSettings = FoxEditSettings.GetSettings();
        foreach (var palette in foxEditSettings.Palettes)
        {
            
        }
    }
}
