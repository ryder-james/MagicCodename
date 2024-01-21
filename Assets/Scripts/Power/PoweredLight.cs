using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PoweredLight : PoweredItem
{
	[SerializeField] private Light2D _light;

	private Coroutine _lightLerpHandle;

	protected override void UpdateCharges(int oldCharges, int newCharges)
	{
		if (_lightLerpHandle != null)
			StopCoroutine(_lightLerpHandle);
		_lightLerpHandle = StartCoroutine(LerpIntensity(Charges, 0.5f));
	}

	private IEnumerator LerpIntensity(int newIntensity, float transitionTime)
	{
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
