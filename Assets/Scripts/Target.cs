using UnityEngine;
using UnityEngine.Serialization;

public class Target : MonoBehaviour
{
	[SerializeField] private Transform _homingPoint;
	[SerializeField, FormerlySerializedAs("_source")] private PowerSocket _sourceSocket;

	public Vector3 HomingPoint => _homingPoint.position;

	public int Priority
	{
		get => _sourceSocket.Charges;
		set => _sourceSocket.Charges = value;
	}
}