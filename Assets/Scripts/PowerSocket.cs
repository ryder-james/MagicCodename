using UnityEngine;

public class PowerSocket : MonoBehaviour
{
	[SerializeField, Min(0)] private int _minCharges = 0;
	[SerializeField] private int _maxCharges = 5;
	[SerializeField, DynamicRange(nameof(_minCharges), nameof(_maxCharges))] private int _charges = 3;

	protected int MinCharges => _minCharges;
	protected int MaxCharges => _maxCharges;

	public virtual int Charges
	{
		get => _charges;
		set
		{
			int oldCharges = _charges;
			_charges = Mathf.Clamp(value, MinCharges, MaxCharges);
			OnChargesUpdated(oldCharges, _charges);
		}
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

	protected virtual void OnChargesUpdated(int oldCharges, int newCharges)
	{

	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		if (Application.IsPlaying(gameObject))
			Charges = _charges;

		_maxCharges = Mathf.Max(_maxCharges, _minCharges + 1);
		OnValidate_Internal();
	}

	protected virtual void OnValidate_Internal()
	{

	}
#endif
}
