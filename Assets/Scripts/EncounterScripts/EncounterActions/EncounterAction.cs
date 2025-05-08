using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public abstract class EncounterAction
    {
        protected Entity _actingEntity;

        public abstract IEnumerator Execute();
    }
}
