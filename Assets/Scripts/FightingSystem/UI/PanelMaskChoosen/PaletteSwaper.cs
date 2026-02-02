using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FoxEdit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaletteSwaper : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            foreach (SimpleVoxelRenderer voxelRenderer in GameObject.FindObjectsOfType<SimpleVoxelRenderer>())
            {
                int palette = voxelRenderer.GetPaletteIndex();

                if (palette == -1)
                    continue;
                palette++;
                if (palette >= FoxEditSettings.GetSettings().Palettes.Count())
                    palette = 0;
                voxelRenderer.SetPaletteIndex(palette);
            }
        }
    }
}
