using UnityEngine;
using UnityEngine.Serialization;

public class Target : MonoBehaviour
{
	[SerializeField, FormerlySerializedAs("_source")] private PowerSocket _sourceSocket;

	public int Priority
	{
		get => _sourceSocket.Charges;
		set => _sourceSocket.Charges = value;
	}
}