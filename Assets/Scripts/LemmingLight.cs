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
			Vector3 pos = Vector3.Lerp(new Vector3(-0.2f, -0.5f, 0), new Vector3(0.2f, -0.5f, 0), (i + 0.5f) / MaxCharges);
			var newLight = Instantiate(_intensityPipPrefab, transform).GetComponent<LemmingLightPip>();
			newLight.transform.localPosition = pos;
			newLight.State = i < MinCharges ? LemmingLightPip.PipState.Locked : i < Charges ? LemmingLightPip.PipState.Enabled : LemmingLightPip.PipState.Disabled;
			_lightPips.Add(newLight);
		}

		_initializing = false;
	}

	protected override void OnChargesUpdated(int oldCharges, int newCharges)
	{
		if (_lightLerpHandle != null)
		{
			StopCoroutine(_lightLerpHandle);
			StopCoroutine(nameof(UpdatePips));
		}
		_lightLerpHandle = StartCoroutine(LerpIntensity(Charges, 0.5f));
		StartCoroutine(UpdatePips(newCharges > oldCharges));
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
			float normalized = t / transitionTime;
			_light.intensity = 1 + (Charges / (float) MaxCharges * normalized);
			_light.pointLightOuterRadius = start + (diff * normalized);
			_light.pointLightInnerRadius = _light.pointLightOuterRadius * 0.5f;
			yield return new WaitForEndOfFrame();
		}
		_light.pointLightOuterRadius = newIntensity;
		_light.pointLightInnerRadius = _light.pointLightOuterRadius * 0.5f;
		_lightLerpHandle = null;
	}

	private IEnumerator UpdatePips(bool chargesIncreasing)
	{
		yield return new WaitUntil(() => !_initializing);

		if (chargesIncreasing)
		{
			for (int i = 0; i < MaxCharges; i++)
			{
				_lightPips[i].State = i < MinCharges ? LemmingLightPip.PipState.Locked : i < Charges ? LemmingLightPip.PipState.Enabled : LemmingLightPip.PipState.Disabled;
				yield return new WaitForSeconds(0.1f);
			}
		}
		else
		{
			for (int i = MaxCharges - 1; i >= 0; i--)
			{
				_lightPips[i].State = i < MinCharges ? LemmingLightPip.PipState.Locked : i < Charges ? LemmingLightPip.PipState.Enabled : LemmingLightPip.PipState.Disabled;
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

#if UNITY_EDITOR
	protected override void OnValidate_Internal()
	{
		if (!_light)
			return;

		_light.intensity = 1 + (Charges / (float) MaxCharges);
		_light.pointLightOuterRadius = Charges;
		_light.pointLightInnerRadius = _light.pointLightOuterRadius * 0.5f;
	}
#endif
}
