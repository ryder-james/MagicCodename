using UnityEngine;

[ExecuteAlways]
public class IndicatorCable : Indicator
{
	[SerializeField] private Transform _start;
	[SerializeField] private Transform _end;
	[SerializeField] private LineRenderer _cableSleeve, _cableIndicator;

	protected override void UpdateIndicator()
	{
		if (_start == null || _end == null)
			return;
		if (_cableSleeve == null || _cableIndicator == null)
			return;

		_cableSleeve.positionCount = 2;
		_cableIndicator.positionCount = 2;
		_cableSleeve.SetPositions(new Vector3[] { _start.position, _end.position });
		_cableIndicator.SetPositions(new Vector3[] { _start.position, _end.position });
		_cableIndicator.enabled = Source.IsPowered;
	}
}
