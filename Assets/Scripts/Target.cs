using UnityEngine;

public class Target : MonoBehaviour
{
	[SerializeField] private PowerSource _source;

	public int Priority
	{
		get => _source.Charges;
		set => _source.Charges = value;
	}
}