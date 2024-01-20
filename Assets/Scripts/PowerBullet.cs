using UnityEngine;

public class PowerBullet : MonoBehaviour
{
	public PowerSource Source { get; set; }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!collision.gameObject.TryGetComponent(out PowerSource item))
		{
			Source.Charges++;
		}
		else
		{
			item.Charges++;
		}

		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.gameObject.TryGetComponent(out PowerSource item))
		{
			Source.Charges++;
			Destroy(gameObject);
		}
		else if (item != Source)
		{
			item.Charges++;
			Destroy(gameObject);
		}
	}
}
