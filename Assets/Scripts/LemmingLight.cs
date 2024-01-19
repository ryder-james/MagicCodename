using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LemmingLight : MonoBehaviour {
    private const int MinIntensity = 0;
    private const int MaxIntensity = 5;

    [SerializeField] private Light2D _light;
    [SerializeField, Range(MinIntensity, MaxIntensity)] private int _intensity = 3;
    [SerializeField] private GameObject _intensityPipPrefab;

    private Coroutine _lightLerpHandle;
    private List<LemmingLightPip> _lightPips = new();

    private void Start() {
        for (int i = 0; i < MaxIntensity; i++) {
            Vector3 pos = new Vector3(-0.2f + (0.1f * i), -0.3f, 0);
            var newLight = Instantiate(_intensityPipPrefab, transform).GetComponent<LemmingLightPip>();
            newLight.transform.localPosition = pos;
            newLight.Enabled = i < _intensity;
            _lightPips.Add(newLight);
        }
    }

    private void UpdateIntensity(int newIntensity) {
        _intensity = Mathf.Clamp(newIntensity, MinIntensity, MaxIntensity);
        if (_lightLerpHandle != null) {
            StopCoroutine(_lightLerpHandle);
            StopCoroutine(nameof(UpdatePips));
        }
        _lightLerpHandle = StartCoroutine(LerpIntensity(_intensity, 0.5f));
        StartCoroutine(nameof(UpdatePips));
    }

    public bool LowerIntensity() {
        if (_intensity == MinIntensity)
            return false;
        UpdateIntensity(_intensity - 1);
        return true;
    }

    public bool RaiseIntensity() {
		if (_intensity == MaxIntensity)
			return false;
		UpdateIntensity(_intensity + 1);
        return true;
    }

    private IEnumerator LerpIntensity(int newIntensity, float transitionTime) {
        float start = _light.pointLightOuterRadius;
        float diff = newIntensity - start;
        if (diff == 0) {
            _lightLerpHandle = null;
            yield break;
        }
        for (float t = 0; t < transitionTime; t += Time.deltaTime) {
            _light.pointLightOuterRadius = start + (diff * Mathf.InverseLerp(0, transitionTime, t));
            yield return new WaitForEndOfFrame();
        }
        _light.pointLightOuterRadius = newIntensity;
        _lightLerpHandle = null;
	}

    private IEnumerator UpdatePips() {
		for (int i = MaxIntensity - 1; i >= 0; i--) {
			_lightPips[i].Enabled = i < _intensity;
            yield return new WaitForSeconds(0.2f);
		}
	}

#if UNITY_EDITOR
    private void OnValidate() {
		if (!_light)
			return;

		_light.pointLightOuterRadius = _intensity;
	}
#endif
}
