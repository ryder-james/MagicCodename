using UnityEngine;

public abstract class Indicator : MonoBehaviour
{
	[SerializeField] private PoweredItem _source;

	protected PoweredItem Source => _source;
	protected float ChargePercentage => _source.Charges / (float) _source.MaxCharges;

	private void OnEnable()
	{
		if (Source == null)
			return;

		Source.OnChargesUpdated += OnChargesUpdated;
	}

	private void OnDisable()
	{
		if (Source == null) 
			return;

		Source.OnChargesUpdated -= OnChargesUpdated;
	}

	private void Update()
	{
		if (_source == null)
			return;

		UpdateIndicator();
	}

	protected virtual void UpdateIndicator()
	{

	}

	protected virtual void OnChargesUpdated(int oldCharge, int newCharges)
	{

	}
}
