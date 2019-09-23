using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Utils
{
    public sealed class Zoom : MonoBehaviour
    {
        [SerializeField] private float _minScale = 0.5f, _maxScale = 3, _maxScaleHard = 5;

        [Range(0, 1f)]
        [SerializeField] private float _upperZoomElasticity = 0.1f;
        [SerializeField] private float _magnitudeThreshold = 40;

        private Vector3 _originScale;

        private float _originalZoomMagnitude, _currentRelativeZoomMagnitude = 1;
        private bool _zooming;

        private void Start() => _originScale = transform.localScale;

        private void FixedUpdate()
        {
            var touches = Input.touches;
            if (touches.Length == 2)
            {
                if (!_zooming)
                {
                    _originalZoomMagnitude = (touches[0].position - touches[1].position).magnitude;
                    _originScale = transform.localScale;
                    _currentRelativeZoomMagnitude = 1;
                    if (_originalZoomMagnitude < _magnitudeThreshold)
                        return; 
                    _zooming = true;
                }
                else
                {
                    var currentMagnitude = (touches[0].position - touches[1].position).magnitude;
                    _currentRelativeZoomMagnitude = currentMagnitude / _originalZoomMagnitude;
                }
            }
            else
                _zooming = false;

            if (_zooming)
                UpdateZooming();

            var currentMag = transform.localScale.magnitude;
            if (currentMag > _maxScale)
                transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale / currentMag * _maxScale, _upperZoomElasticity);
        }

        private void UpdateZooming()
        {
            var unboundScale = _originScale * _currentRelativeZoomMagnitude;
            var mag = unboundScale.magnitude;
            if (mag < _minScale)
                unboundScale = unboundScale / mag * _minScale;
            else if (mag > _maxScaleHard)
                unboundScale = unboundScale / mag * _maxScaleHard;
            transform.localScale = unboundScale;
        }
    }
}