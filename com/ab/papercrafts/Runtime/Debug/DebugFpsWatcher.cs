using Unity.Profiling;
using UnityEngine;

namespace Plugins.PaperCrafts.com.AB.Debug
{
    public class DebugFpsWatcher : MonoBehaviour
    {
        public float checkInterval = 0.5f;
        public int fpsThreshold = 30;
        public bool breakOnDrop = true;

        float _timer = 0f;
        int _frameCount = 0;
        ProfilerMarker _fpsDropMarker = new ProfilerMarker("⚠️ FPS Drop");

        void Update()
        {
            _frameCount++;
            _timer += Time.unscaledDeltaTime;

            if (_timer >= checkInterval)
            {
                float currentFps = _frameCount / _timer;

                if (currentFps < fpsThreshold)
                {
                    UnityEngine.Debug.LogWarning($"[FPSWatcher] FPS dropped to {currentFps:0.0}");

                    _fpsDropMarker.Begin();
                    _fpsDropMarker.End();

                    if (breakOnDrop)
                    {
                        UnityEngine.Debug.Break();
                    }
                }

                _frameCount = 0;
                _timer = 0f;
            }
        }
    }
}