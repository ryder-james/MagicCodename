using UnityEngine;

[ExecuteAlways]
public class PowerIndicator : MonoBehaviour
{
	[SerializeField] private Transform _start;
	[SerializeField] private Transform _end;
	[SerializeField] private GameObject _indicator;
	[SerializeField, Min(0.05f)] private float _indicatorMaxSize = 1;
	[SerializeField] private PowerSocket _source;
	[SerializeField] private LineRenderer _cableSleeve, _cableIndicator;

	private void Update()
	{
		if (_source == null)
			return;

		UpdateCable();
		UpdateIndicator();
	}

	private void UpdateCable()
	{
		if (_start == null || _end == null)
			return;
		if (_cableSleeve == null || _cableIndicator == null)
			return;

		_cableSleeve.positionCount = 2;
		_cableIndicator.positionCount = 2;
		_cableSleeve.SetPositions(new Vector3[] { _start.position, _end.position });
		_cableIndicator.SetPositions(new Vector3[] { _start.position, _end.position });
		_cableIndicator.enabled = _source.IsPowered;
	}

	private void UpdateIndicator()
	{
		if (_indicator == null)
			return;

		_indicator.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * _indicatorMaxSize, _source.Charges / (float) _source.MaxCharges);
	}
}
