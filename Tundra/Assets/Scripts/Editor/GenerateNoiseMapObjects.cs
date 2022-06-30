
using Environment.Objects;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ObjectMapGenerator))]
    public class GenerateNoiseMapObjects : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var mapGenerator = (ObjectMapGenerator)target;

            if (DrawDefaultInspector())
            {
                if (mapGenerator.AutoUpdate)
                    mapGenerator.DrawMapInEditor();
            }

            if (GUILayout.Button("Generate"))
            {
                mapGenerator.DrawMapInEditor();
            }
        }
    }
}
