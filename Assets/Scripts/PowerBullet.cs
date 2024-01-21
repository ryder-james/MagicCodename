using UnityEngine;

public class PowerBullet : MonoBehaviour
{
	[SerializeField] private LayerMask _collisionIgnore;

	public PowerSocket SourceSocket { get; set; }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (_collisionIgnore.Contains(collision.gameObject.layer))
			return;

		if (!collision.gameObject.TryGetComponent(out PowerSocket socket))
		{
			SourceSocket.Charges++;
		}
		else
		{
			socket.Charges++;
		}

		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (_collisionIgnore.Contains(collision.gameObject.layer))
			return;

		if (!collision.gameObject.TryGetComponent(out PowerSocket socket))
		{
			SourceSocket.Charges++;
			Destroy(gameObject);
		}
		else if (socket != SourceSocket)
		{
			if (!socket.AddCharge())
				SourceSocket.Charges++;
			Destroy(gameObject);
		}
	}
}
