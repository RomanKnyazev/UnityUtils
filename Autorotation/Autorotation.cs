using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Utils
{
    public sealed class Autorotation : MonoBehaviour
    {
        public static event System.Action<ScreenOrientation> onOrientationChanged;

        public bool _portrait, _portraitReversed, _landscape, _landscapeReversed;

        private void OnEnable() => Set();

        public void Set() => StartCoroutine(SetInternal());

        private IEnumerator SetInternal()
        {
            if (_portrait)
                Screen.orientation = ScreenOrientation.Portrait;
            else if (_portraitReversed)
                Screen.orientation = ScreenOrientation.PortraitUpsideDown;
            else if (_landscape)
                Screen.orientation = ScreenOrientation.LandscapeLeft;
            else if (_landscapeReversed)
                Screen.orientation = ScreenOrientation.LandscapeRight;
            
            var e = onOrientationChanged;
            if (e != null)
                e(Screen.orientation);

            yield return null;

            Screen.autorotateToPortrait = _portrait;
            Screen.autorotateToPortraitUpsideDown = _portraitReversed;
            Screen.autorotateToLandscapeLeft = _landscape;
            Screen.autorotateToLandscapeRight = _landscapeReversed;
            Screen.orientation = ScreenOrientation.AutoRotation;
        }
    }   
}