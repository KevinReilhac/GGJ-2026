using FoxEdit;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FoxEdit
{
    public class SimpleVoxelRenderer : MonoBehaviour
    {
        [SerializeField] private VoxelObject _voxelObject = null;
        [SerializeField] private int _paletteIndexOverride = -1;

        private MeshFilter _filter;
        private MeshRenderer _renderer;
        private Material _staticMaterial = null;

        private void Awake()
        {
            _filter = GetComponent<MeshFilter>();
            _filter.mesh = _voxelObject.StaticMesh;

            _renderer = GetComponent<MeshRenderer>();
            FoxEditSettings foxEditSettings = FoxEditSettings.GetSettings();
            _staticMaterial = Instantiate(foxEditSettings.Materials.staticMaterial);
            _renderer.material = _staticMaterial;
        }

        private void Update()
        {
            int paletteIndex = _paletteIndexOverride != -1 ? _paletteIndexOverride : _voxelObject.PaletteIndex;
            _staticMaterial.SetBuffer("_Colors", VoxelSharedData.GetColorBuffer(paletteIndex));
        }

        public void SetPaletteIndex(int index)
        {
            _paletteIndexOverride = index;
        }
    }
}
