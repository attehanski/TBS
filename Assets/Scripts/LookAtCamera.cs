using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TBS
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField]
        private bool _invert;

        private Transform _target;

        void Start()
        {
            _target = Camera.main.transform;
        }

        void Update()
        {
            Vector3 lookDirection = _invert ? transform.position - _target.position : _target.position - transform.position;

            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }
}
