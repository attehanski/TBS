using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class NPC : Character
    {
        protected Player _player;

        // NOTE: This might be better to place in Game as multiple places need to access Player
        public Player Player
        {
            get
            {
                if (!_player)
                    _player = FindObjectOfType<Player>();

                return _player;
            }
        }
    }
}
