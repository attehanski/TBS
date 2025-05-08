using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TBS
{
    public class GameEvents : MonoBehaviour
    {
        [HideInInspector] public UnityEvent ResetLevel;
        [HideInInspector] public UnityEvent<bool> PauseStateChanged;
        [HideInInspector] public UnityEvent CameraTargetChanged;

        [HideInInspector] public UnityEvent<Entity> EntityDestroyed;
        [HideInInspector] public UnityEvent<float> PlayerHealthChanged;
        [HideInInspector] public UnityEvent<float> PlayerEnergyChanged;
        [HideInInspector] public UnityEvent NotEnoughEnergy;
        [HideInInspector] public UnityEvent OnPlayerDeath;

        [HideInInspector] public UnityEvent EncounterStarted;
        [HideInInspector] public UnityEvent<Entity, Entity> CombatActionStarted;
        [HideInInspector] public UnityEvent ActionQueueFinished;


        // TESTING EVENTS
        [HideInInspector] public UnityEvent EncounterGoalReached;
    }
}
