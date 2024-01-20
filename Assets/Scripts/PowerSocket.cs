using UnityEngine;

public class PowerSocket : MonoBehaviour
{
	[SerializeField, Min(0)] private int _minCharges = 0;
	[SerializeField] private int _maxCharges = 5;
	[SerializeField, DynamicRange(nameof(_minCharges), nameof(_maxCharges))] private int _initialCharges = 3;

	protected int MinCharges => _minCharges;
	protected int MaxCharges => _maxCharges;
	protected int InitialCharges => _initialCharges;

	private int _charges;
	public virtual int Charges
	{
		get => _charges;
		set
		{
			if (_charges == value)
				return;
			_charges = Mathf.Clamp(value, MinCharges, MaxCharges);
			OnChargesUpdated(_charges);
		}
	}

	private void Awake()
	{
		Charges = InitialCharges;
	}

	public bool AddCharge()
	{
		if (Charges == MaxCharges)
			return false;
		Charges++;
		return true;
	}

	public bool TakeCharge()
	{
		if (Charges == MinCharges)
			return false;
		Charges--;
		return true;
	}

	protected virtual void OnChargesUpdated(int charges)
	{

	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		_initialCharges = Mathf.Clamp(_initialCharges, _minCharges, _maxCharges);
		_maxCharges = Mathf.Max(_maxCharges, _minCharges + 1);
		OnValidate_Internal();
	}

	protected virtual void OnValidate_Internal()
	{

	}
#endif
}
