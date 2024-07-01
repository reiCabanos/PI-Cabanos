using UnityEditor;
using UnityEditor.Rendering;

using UnityEngine;

namespace OccaSoftware.GaussianBlur.Editor
{
    public class GaussianBlurEditorGUI : ShaderGUI
    {
        public override void OnGUI(MaterialEditor e, MaterialProperty[] properties)
        {
            Material t = e.target as Material;

            MaterialProperty blurRadius = FindProperty("_BlurRadius", properties);
            MaterialProperty tint = FindProperty("_Color", properties);

            MinIntegerField(blurRadius, new GUIContent("Blur Radius (px)"), 0);
            e.ShaderProperty(tint, new GUIContent("Tint"));
        }

        private void MinIntegerField(MaterialProperty prop, GUIContent c, int min)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = prop.hasMixedValue;
            float blur = EditorGUILayout.IntField(c, (int)prop.floatValue);
            if (EditorGUI.EndChangeCheck())
            {
                prop.floatValue = Mathf.Max(blur, 0);
            }
            EditorGUI.showMixedValue = false;
        }
    }
}
