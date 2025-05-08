using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace TBS
{
    [RequireComponent(typeof(Camera))]
    [ExecuteAlways]
    public class CurvedRenderingManager : MonoBehaviour
    {
        [SerializeField]
        private bool _useCurvedRendering = true;
        [SerializeField] [Range(-0.03f, 0.03f)]
        private float _curveAmount = 0.01f;

        private const string CURVED_RENDERING = "_ENABLECURVE";
        private const string CURVE_AMOUNT = "_CurveAmount";

        private Camera _camera;

        private void Awake()
        {
            if (Application.isPlaying && _useCurvedRendering)
            {
                Shader.EnableKeyword(CURVED_RENDERING);
                Shader.SetGlobalFloat(CURVE_AMOUNT, _curveAmount);
            }
            else
                Shader.DisableKeyword(CURVED_RENDERING);
        }

        private void Start()
        {
            if (Application.isPlaying)
            {
                _camera = GetComponent<Camera>();
                OnBeginCameraRendering();
            }
        }

        private void OnBeginCameraRendering()
        {
            _camera.cullingMatrix = Matrix4x4.Ortho(-99, 99, -99, 99, 0.001f, 99) * _camera.worldToCameraMatrix;
        }
    }
}
