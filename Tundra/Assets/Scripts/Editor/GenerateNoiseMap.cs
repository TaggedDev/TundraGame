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
                if (mapGenerator.autoUpdate)
                    mapGenerator.GenerateMap();
            }

            if (GUILayout.Button("Generate"))
            {
                mapGenerator.GenerateMap();
            }
        }
    }
}
