using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using NTC.Global.Cache;
using UnityEngine;
namespace Enjine
{
    public class ParallaxBehaviour : MonoCache
    {
        [SerializeField, StatusIcon] private Transform _followingTarget;
        [SerializeField, Range(0f, 1f)] private float _parallaxStrengthHorizontal = 0.1f;
        [SerializeField, Range(0f, 1f)] private float _parallaxStrengthVertical = 0.1f;


        private Vector3 _targetPreviousPosition;

        private void Start()
        {
            if (!_followingTarget)
                _followingTarget = Camera.main.transform;

            _targetPreviousPosition = _followingTarget.position;
        }

        protected override void FixedRun()
        {
            Vector3 delta = _followingTarget.position - _targetPreviousPosition;

            transform.position += new Vector3(delta.x * _parallaxStrengthHorizontal, delta.y * _parallaxStrengthVertical, 0);
            _targetPreviousPosition = _followingTarget.position;
        }
    }
}