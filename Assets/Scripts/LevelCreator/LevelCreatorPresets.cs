using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    [CreateAssetMenu(menuName = "TBS/LevelCreatorPresets")]
    [System.Serializable]
    public class LevelCreatorPresets : ScriptableObject
    {
        public GameObject[] Tiles;
        public GameObject[] Entities;
    }
}
