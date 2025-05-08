using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public abstract class EntityBehaviour : ScriptableObject
    {
        public abstract EncounterAction GetAction(Entity actor);
    }
}
