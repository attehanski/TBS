using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TBS
{
    public class TileStateIndicator : MonoBehaviour
    {
        [System.Serializable]
        private class IndicatorColors
        {
            public Color DefaultColor;
            public Color PressedColor;
        }

        [SerializeField] private IndicatorColors _defaultColors;
        [SerializeField] private IndicatorColors _highlightedColors;
        [SerializeField] private IndicatorColors _unavailableColors;

        private Material _indicatorMaterial;
        private UnityEvent _onClickEvent = new UnityEvent();
        private IndicatorColors _currentColors;

        public UnityEvent OnClickEvent => _onClickEvent;

        public void SetIndicatorState(Enums.TileState state)
        {
            _currentColors = state switch
            {
                Enums.TileState.Highlighted => _highlightedColors,
                Enums.TileState.Unavailable => _unavailableColors,
                _ => _defaultColors
            };
            _indicatorMaterial.color = _currentColors.DefaultColor;
        }

        private void Start()
        {
            _indicatorMaterial = GetComponent<Renderer>().material;
            _currentColors = _defaultColors;
        }

        private void OnMouseDown()
        {
            _indicatorMaterial.color = _currentColors.PressedColor;
            _onClickEvent.Invoke();
        }

        private void OnMouseUp()
        {
            _indicatorMaterial.color = _currentColors.DefaultColor;
        }
    }
}
