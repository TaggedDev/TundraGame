using Environment;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(MapGenerator))]
    public class GenerateNoiseMap : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var mapGenerator = (MapGenerator)target;

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
