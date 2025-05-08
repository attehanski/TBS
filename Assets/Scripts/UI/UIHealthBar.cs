using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace TBS
{
    public class UIHealthBar : MonoBehaviour
    {
        [Header("Values")]
        [SerializeField]
        protected float _lossBarDelay;

        [Header("References")]
        [SerializeField]
        protected Image _healthBar;
        [SerializeField]
        protected Image _lossBar;

        /// <summary>
        /// Sets health bar value
        /// </summary>
        /// <param name="newValue">Value of health bar as a percentage</param>
        public void SetHealthBarValue(float newValue)
        {
            _lossBar.fillAmount = newValue;
            _healthBar.fillAmount = newValue;
        }
    }
}
