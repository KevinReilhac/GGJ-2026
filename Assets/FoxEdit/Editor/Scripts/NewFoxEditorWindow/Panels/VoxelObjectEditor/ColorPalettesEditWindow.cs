using System;
using UnityEditor;
using UnityEngine;

namespace FoxEdit.WindowPanels
{
    internal class EditColorPaletteWindow : EditorWindow
    {
        private static VoxelPalette targetVoxelPalette;
        private static Action<VoxelPalette> OnUpdateVoxelPaletteCallback;
        private Editor editor;

        public static void OpenColorPalette(VoxelPalette voxelPalette, Action<VoxelPalette> updatedVoxelPaletteCallback)
        {
            targetVoxelPalette = voxelPalette;
            
            GetWindow<EditColorPaletteWindow>();
            OnUpdateVoxelPaletteCallback = updatedVoxelPaletteCallback;
        }

        private void OnEnable()
        {
            editor = Editor.CreateEditor(targetVoxelPalette);
        }

        void OnDisable()
        {
            DestroyImmediate(editor);
            OnUpdateVoxelPaletteCallback?.Invoke(targetVoxelPalette);
        }

        private void OnGUI()
        {
            if (editor != null)
            {
                editor.OnInspectorGUI();
            }

        }

        public override void DiscardChanges()
        {
            base.DiscardChanges();
        }
    }
}