using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TBS
{
    public abstract class SpecialAction
    {
        public SpecialAction(Entity actingEntity)
        {
            _actor = actingEntity;
        }

        public class TileData
        {
            public List<(Tile, Enums.TileState)> TileStates = new List<(Tile, Enums.TileState)>();
            public UnityAction<Tile> TileAction;
        }

        protected Entity _actor;

        public UnityAction<EncounterAction> OnActionInvoked;

        public abstract TileData GetActionTileData();
        protected abstract void InvokeSpecialAction(Tile tile);
    }
}
