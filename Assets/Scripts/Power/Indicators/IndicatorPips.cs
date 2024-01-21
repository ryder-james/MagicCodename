using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorPips : Indicator
{
	[SerializeField] private Transform _pipStart, _pipEnd;
	[SerializeField] private PowerPip _pipPrefab;

	private List<PowerPip> _powerPips = new();
	private Coroutine _pipUpdateHandle;
	private int _knownCharges;

	private void Start()
	{
		for (int i = 0; i < Source.MaxCharges; i++)
		{
			Vector3 pos = Vector3.Lerp(_pipStart.position, _pipEnd.position, i / (float) (Source.MaxCharges - 1));
			var newPip = Instantiate(_pipPrefab, transform);
			newPip.transform.position = pos;
			_powerPips.Add(newPip);
		}
		_knownCharges = 0;
		OnChargesUpdated(0, Source.Charges);
	}

	protected override void OnChargesUpdated(int oldCharges, int newCharges)
	{
		if (Source.MaxCharges != _powerPips.Count)
		{
			Debug.Log("charges changed");
			foreach (var powerPip in _powerPips)
				Destroy(powerPip.gameObject);
			_powerPips.Clear();
			Start();
		}

		if (_pipUpdateHandle != null)
			StopCoroutine(_pipUpdateHandle);
		_pipUpdateHandle = StartCoroutine(UpdatePips(_knownCharges < newCharges));
	}

	private IEnumerator UpdatePips(bool chargesIncreasing)
	{
		if (chargesIncreasing)
		{
			for (int i = 0; i < Source.MaxCharges; i++)
			{
				if (i < Source.Charges)
					_knownCharges = i;
				_powerPips[i].State = GetState(i);
				yield return new WaitForSeconds(0.1f);
			}
		}
		else
		{
			for (int i = Source.MaxCharges - 1; i >= 0; i--)
			{
				if (i < Source.Charges)
					_knownCharges = i;
				_powerPips[i].State = GetState(i);
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

	private PowerPip.PipState GetState(int pipIndex)
	{
		return pipIndex < Source.MinCharges ? PowerPip.PipState.Locked
				: pipIndex < Source.Charges ? PowerPip.PipState.Enabled
					: PowerPip.PipState.Disabled;
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if (_pipStart == null || _pipEnd == null)
			return;

		Gizmos.DrawLine(_pipStart.position, _pipEnd.position);

		if (Source == null)
			return;

		for (int i = 0; i < Source.MaxCharges; i++)
		{
			Vector3 pos = Vector3.Lerp(_pipStart.position, _pipEnd.position, i / (float) (Source.MaxCharges - 1));
			Gizmos.color = i < Source.MinCharges ? Color.blue : i < Source.Charges ? Color.yellow : Color.gray; 
			Gizmos.DrawSphere(pos, 0.05f);
		}
	}
#endif
}
