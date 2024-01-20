using UnityEngine;

public class PowerBullet : MonoBehaviour {
	public LemmingLight Source { get; set; }

	private void OnCollisionEnter2D(Collision2D collision) {
		if (!collision.gameObject.TryGetComponent(out LemmingLight light)) {
			Source.RaiseIntensity();
		} else {
			light.RaiseIntensity();
		}

		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (!collision.gameObject.TryGetComponent(out LemmingLight light)) {
			Source.RaiseIntensity();
			Destroy(gameObject);
		} else if (light != Source) {
			light.RaiseIntensity();
			Destroy(gameObject);
		}
	}
}
