using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LemmingLight : PowerSocket
{
	[SerializeField] private Light2D _light;
	[SerializeField] private GameObject _intensityPipPrefab;

	private Coroutine _lightLerpHandle;
	private List<LemmingLightPip> _lightPips = new();
	private bool _initializing = true;

	private void Start()
	{
		for (int i = 0; i < MaxCharges; i++)
		{
			Vector3 pos = new(-0.2f + (0.1f * i), -0.3f, 0);
			var newLight = Instantiate(_intensityPipPrefab, transform).GetComponent<LemmingLightPip>();
			newLight.transform.localPosition = pos;
			newLight.Enabled = i < InitialCharges;
			_lightPips.Add(newLight);
		}

		_initializing = false;
	}

	protected override void OnChargesUpdated(int newCharges)
	{
		if (_lightLerpHandle != null)
		{
			StopCoroutine(_lightLerpHandle);
			StopCoroutine(nameof(UpdatePips));
		}
		_lightLerpHandle = StartCoroutine(LerpIntensity(Charges, 0.5f));
		StartCoroutine(nameof(UpdatePips));
	}

	private IEnumerator LerpIntensity(int newIntensity, float transitionTime)
	{
		yield return new WaitUntil(() => !_initializing);

		float start = _light.pointLightOuterRadius;
		float diff = newIntensity - start;
		if (diff == 0)
		{
			_lightLerpHandle = null;
			yield break;
		}
		for (float t = 0; t < transitionTime; t += Time.deltaTime)
		{
			_light.pointLightOuterRadius = start + (diff * Mathf.InverseLerp(0, transitionTime, t));
			yield return new WaitForEndOfFrame();
		}
		_light.pointLightOuterRadius = newIntensity;
		_lightLerpHandle = null;
	}

	private IEnumerator UpdatePips()
	{
		yield return new WaitUntil(() => !_initializing);

		for (int i = MaxCharges - 1; i >= 0; i--)
		{
			_lightPips[i].Enabled = i < Charges;
			yield return new WaitForSeconds(0.1f);
		}
	}

#if UNITY_EDITOR
	protected override void OnValidate_Internal()
	{
		if (!_light)
			return;

		_light.pointLightOuterRadius = InitialCharges;
	}
#endif
}
