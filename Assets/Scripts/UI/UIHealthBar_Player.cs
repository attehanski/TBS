using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TBS
{
    public class UIHealthBar_Player : UIHealthBar
    {
        [SerializeField]
        protected Image _energyBar;
        [SerializeField]
        protected Image _energyLossBar;


        protected GameEvents _events => Game.Instance.GameEvents;

        /// <summary>
        /// Sets energy bar value
        /// </summary>
        /// <param name="newValue">Value of energy bar as a percentage</param>
        public void SetEnergyBarValue(float newValue)
        {
            _energyLossBar.fillAmount = newValue;
            _energyBar.fillAmount = newValue;
        }

        protected void OnNotEnoughEnergy()
        {
            StartCoroutine(FlashEnergyBarError());
        }

        protected IEnumerator FlashEnergyBarError()
        {
            // TODO: Add functionality
            yield return null;
        }

        void Awake()
        {
            _events.PlayerHealthChanged.AddListener(SetHealthBarValue);
            _events.PlayerEnergyChanged.AddListener(SetEnergyBarValue);
            _events.NotEnoughEnergy.AddListener(OnNotEnoughEnergy);
        }
    }
}
