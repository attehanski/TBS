using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    [RequireComponent(typeof(Camera))]
    public class MainCamera : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _cameraSpeedByDistance;
        [SerializeField] private Vector3 _cameraPositionOffset;
        // TODO: Add acceleration?

        [Header("Camera Shake")]
        [SerializeField] private float _cameraShakeAmplitude = 0.1f;
        [SerializeField] private float _cameraShakeDuration = 0.15f;

        [Header("Combat camera variables")]
        [SerializeField] private float _targetSwapTime = 1f;
        [SerializeField] private float _combatZoomBase = 15;
        [SerializeField] private float _combatZoomMultiplier = 2f;

        private Transform _playerFollowTarget;
        private Transform _combatFollowTarget;
        private Transform _currentFollowTarget;
        private bool _cameraShakeOngoing = false;
        private float _defaultZoom;
        private float _cameraShakeTime = 0f;
        private Camera _camera;
        //private ParallaxBackground _parallax;

        private GameEvents _events => Game.Instance.GameEvents;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            _defaultZoom = _camera.fieldOfView;
            GameObject combatFollowTarget = new GameObject();
            combatFollowTarget.name = "CameraCombatFollowTarget";
            _combatFollowTarget = combatFollowTarget.transform;
            Game.Instance.GlobalUpdate.AddListener(DoUpdate);
            SetPlayerCameraTarget(FindAnyObjectByType<Player>().CameraFollowTarget);
            _events.CombatActionStarted.AddListener(OnCombatActionStarted);
            _events.ActionQueueFinished.AddListener(ReturnCameraToDefault);

            //_parallax = GetComponent<ParallaxBackground>();
            //_parallax.InitParallaxBackground(transform.position);
        }

        private void OnDestroy()
        {
            Game.Instance.GlobalUpdate.RemoveListener(DoUpdate);
        }

        private void DoUpdate()
        {
            if (!_currentFollowTarget)
                return;

            if (_currentFollowTarget)
                MoveTowardsFollowTarget();
            DoCameraShake(); // TODO: Check that this actually works with follow target move
        }

        private void MoveTowardsFollowTarget()
        {
            float distanceToTarget = Vector3.Distance(transform.position, _currentFollowTarget.position + _cameraPositionOffset);
            SetPosition(Vector3.MoveTowards(transform.position, _currentFollowTarget.position + _cameraPositionOffset, _cameraSpeedByDistance.Evaluate(distanceToTarget)));
        }

        public void SetPlayerCameraTarget(Transform target)
        {
            _playerFollowTarget = target;
            _currentFollowTarget = _playerFollowTarget;
        }

        public void UpdatePositionToTarget()
        {
            SetPosition(_currentFollowTarget.position + _cameraPositionOffset);
        }

        private void SetPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
            //_parallax.UpdateParallaxLevels(transform.position);
        }

        private void OnCombatActionStarted(Entity entity1, Entity entity2)
        {
            Entity southernmostEntity = entity1.Tile.Coordinates.y == entity2.Tile.Coordinates.y ? 
                (entity1.Tile.Coordinates.x < entity2.Tile.Coordinates.x ? entity1 : entity2) : 
                (entity1.Tile.Coordinates.y < entity2.Tile.Coordinates.y ? entity1 : entity2);
            float distance = Vector3.Distance(entity1.transform.position, entity2.transform.position);
            Vector3 direction = ((southernmostEntity == entity1 ? entity2.transform.position : entity1.transform.position) - 
                (southernmostEntity == entity1 ? entity1.transform.position : entity2.transform.position)).normalized;
            Vector3 followTargetPosition = southernmostEntity.transform.position + distance * 0.33f * direction;
            _combatFollowTarget.position = followTargetPosition;

            float zoomAmount = _combatZoomBase + _combatZoomMultiplier * distance;

            StartCoroutine(SwapCameraTarget(_combatFollowTarget, followTargetPosition));
            StartCoroutine(ZoomCameraToFollowTarget(zoomAmount));
        }

        private void ReturnCameraToDefault()
        {
            if (_currentFollowTarget == _playerFollowTarget)
            {
                _events.CameraTargetChanged.Invoke();
                return;
            }

            StartCoroutine(SwapCameraTarget(_playerFollowTarget, _playerFollowTarget.position));
            StartCoroutine(ZoomCameraToFollowTarget(_defaultZoom));
        }

        private IEnumerator SwapCameraTarget(Transform newTarget, Vector3 cameraTargetPosition)
        {
            Vector3 startPos = _currentFollowTarget.position;
            newTarget.position = _currentFollowTarget.position;
            _currentFollowTarget = newTarget;

            float timeElapsed = 0f;
            while (timeElapsed < _targetSwapTime)
            {
                yield return null;
                timeElapsed += Time.deltaTime;
                _currentFollowTarget.position = Vector3.Lerp(startPos, cameraTargetPosition, timeElapsed / _targetSwapTime);
            }

            _events.CameraTargetChanged.Invoke();
        }

        private IEnumerator ZoomCameraToFollowTarget(float zoom)
        {
            float startFOV = _camera.fieldOfView;
            float timeElapsed = 0f;
            while (timeElapsed < _targetSwapTime)
            {
                yield return null;
                timeElapsed += Time.deltaTime;
                _camera.fieldOfView = Mathf.Lerp(startFOV, zoom, timeElapsed / _targetSwapTime);
            }
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !_currentFollowTarget)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_currentFollowTarget.position, 0.05f);
        }

        #region CAMERASHAKE
        public void StartCameraShake()
        {
            _cameraShakeOngoing = true;
        }

        private void DoCameraShake()
        {
            if (!_cameraShakeOngoing)
                return;

            if (_cameraShakeTime >= _cameraShakeDuration)
            {
                EndCameraShake();
                return;
            }

            transform.position = transform.position + (Vector3)Random.insideUnitCircle * _cameraShakeAmplitude;
            _cameraShakeTime += Time.deltaTime;
        }

        public void EndCameraShake()
        {
            _cameraShakeOngoing = false;
            _cameraShakeTime = 0f;
        }
        #endregion
    }
}
