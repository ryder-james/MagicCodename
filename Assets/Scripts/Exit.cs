using UnityEngine;

public class Exit : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.TryGetComponent(out Lemming lemming))
			return;

		Destroy(lemming.gameObject);
		Debug.Log("Win");
	}
}
