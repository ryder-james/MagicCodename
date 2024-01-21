using UnityEngine;

[ExecuteAlways]
public class IndicatorScaledObject : Indicator
{
	[SerializeField] private GameObject _indicator;
	[SerializeField, Min(0.05f)] private float _indicatorMaxSize = 1;

	protected override void UpdateIndicator()
	{
		if (_indicator == null)
			return;

		_indicator.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * _indicatorMaxSize, ChargePercentage);
	}
}
