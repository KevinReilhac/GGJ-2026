using FoxEdit;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static log4net.Appender.ColoredConsoleAppender;

namespace FoxEdit
{
    public class SimpleVoxelRenderer : MonoBehaviour
    {
        [SerializeField] private VoxelObject _voxelObject = null;
        [SerializeField] private int _paletteIndexOverride = -1;

        private MeshFilter _filter;
        private MeshRenderer _renderer;
        private Material _staticMaterial = null;

        private Texture2D _paletteColorTexture = null;
        private Texture2D _paletteEMSTexture = null;

        private void Awake()
        {
            _filter = GetComponent<MeshFilter>();
            _filter.mesh = _voxelObject.StaticMesh;

            _renderer = GetComponent<MeshRenderer>();
            FoxEditSettings foxEditSettings = FoxEditSettings.GetSettings();
            _staticMaterial = Instantiate(foxEditSettings.Materials.staticMaterial);
            _renderer.material = _staticMaterial;
            GenerateTexture(_voxelObject.PaletteIndex);
        }

        private void Update()
        {
            //int paletteIndex = _paletteIndexOverride != -1 ? _paletteIndexOverride : _voxelObject.PaletteIndex;
            //_staticMaterial.SetBuffer("_Colors", VoxelSharedData.GetColorBuffer(paletteIndex));
        }

        public void SetPaletteIndex(int index)
        {
            _paletteIndexOverride = index;
            if (_paletteIndexOverride == -1)
                GenerateTexture(_voxelObject.PaletteIndex);
            else
                GenerateTexture(_paletteIndexOverride);
        }

        private void GenerateTexture(int index)
        {
            FoxEditSettings foxEditSettings = FoxEditSettings.GetSettings();
            VoxelColor[] colors = foxEditSettings.Palettes[index].Colors;
            _paletteColorTexture = new Texture2D(colors.Length, 1, TextureFormat.RGBA32, false);
            _paletteEMSTexture = new Texture2D(colors.Length, 1, TextureFormat.RGBA32, false);

            for (int i = 0; i < colors.Length; i++)
            {
                _paletteColorTexture.SetPixel(i, 0, colors[i].Color);
                _paletteEMSTexture.SetPixel(i, 0, new Color(colors[i].EmissiveIntensity, colors[i].Metallic, colors[i].Smoothness, 0.0f));
            }
            _paletteColorTexture.Apply();
            _paletteEMSTexture.Apply();

            _staticMaterial.SetTexture("_PaletteColorTexture", _paletteColorTexture);
            _staticMaterial.SetTexture("_PaletteEMSTexture", _paletteEMSTexture);
            _staticMaterial.SetInt("_PaletteSize", colors.Length - 1);
        }
    }
}
