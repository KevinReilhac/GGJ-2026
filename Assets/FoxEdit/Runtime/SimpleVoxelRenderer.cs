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
            _staticMaterial.SetBuffer("_Colors", VoxelSharedData.GetColorBuffer(_voxelObject.PaletteIndex));
        }

    }
}
