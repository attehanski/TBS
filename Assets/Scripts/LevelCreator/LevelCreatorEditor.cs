using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace TBS
{
    [CustomEditor(typeof(LevelCreator))]
    public class LevelCreatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            LevelCreator levelCreator = target as LevelCreator;
            // TODO: Add values and buttons here
            base.OnInspectorGUI();

            if (GUILayout.Button("New"))
                levelCreator.CreateNewMap();
            if (GUILayout.Button("Save"))
                levelCreator.SaveMap();
            if (GUILayout.Button("Load"))
                levelCreator.LoadMap();
        }

        public void OnSceneGUI()
        {
            // TODO: Add placing, raycasting etc scene functionality here
        }
    }
}
