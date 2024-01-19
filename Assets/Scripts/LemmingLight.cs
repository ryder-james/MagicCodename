using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LemmingLight : MonoBehaviour {
    private const int MinIntensity = 0;
    private const int MaxIntensity = 5;

    [SerializeField] private Light2D _light;
    [SerializeField, Range(MinIntensity, MaxIntensity)] private int _intensity = 3;

    private Coroutine _lightLerpHandle;

    public void UpdateIntensity(int newIntensity) {
        _intensity = Mathf.Clamp(newIntensity, MinIntensity, MaxIntensity);
        if (_lightLerpHandle != null)
            StopCoroutine(_lightLerpHandle);
        _lightLerpHandle = StartCoroutine(LerpIntensity(newIntensity, 0.5f));
    }

    private IEnumerator LerpIntensity(int newIntensity, float transitionTime) {
        float start = _light.pointLightOuterRadius;
        float diff = newIntensity - start;
        for (float t = 0; t < transitionTime; t += Time.deltaTime) {
            _light.pointLightOuterRadius = start + (diff * Mathf.InverseLerp(0, transitionTime, t));
            yield return new WaitForEndOfFrame();
        }
        _light.pointLightOuterRadius = newIntensity;
        _lightLerpHandle = null;
	}

#if UNITY_EDITOR
    private void OnValidate() {
		if (!_light)
			return;

		_light.pointLightOuterRadius = _intensity;
	}
#endif
}
