using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class LevelData
    {
        public LevelData(int sizeX, int sizeY)
        {
            _map = new Tile[sizeX,sizeY];
        }

        private Tile[,] _map;
        // TODO: Add entities as well
    }

    [ExecuteInEditMode]
    public class LevelCreator : MonoBehaviour
    {
        [SerializeField]
        private LevelCreatorPresets _presets;
        [SerializeField]
        private string _levelSavePath;
        [SerializeField]
        private Vector2 _levelSize;
        [SerializeField]
        private string _levelName;

        private LevelData _levelData;

        public void CreateNewMap()
        {
            _levelData = new LevelData((int)_levelSize.x, (int)_levelSize.y);
            Debug.Log("New!");
        }

        public void SaveMap()
        {
            Debug.Log("Save!");
        }

        public void LoadMap()
        {
            Debug.Log("Load!");
        }
    }
}
