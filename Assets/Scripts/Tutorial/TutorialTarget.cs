using UnityEngine;

public class TutorialTarget : MonoBehaviour
{
	public enum TargetType
	{
		Player,
		PowerBullet,
		Lemming
	}

	[SerializeField] private Tutorial _tutorial;
	[SerializeField] private TargetType _targetType;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		switch (_targetType)
		{
		case TargetType.Player:
			if (!collision.TryGetComponent(out Player player))
				return;

			if (_tutorial.TargetHit(this))
				Destroy(this);
			break;
		case TargetType.PowerBullet:
			if (!collision.TryGetComponent(out PowerBullet bullet))
				return;

			if (_tutorial.TargetHit(this))
				Destroy(this);
			break;
		case TargetType.Lemming:
			if (!collision.TryGetComponent(out Lemming lemming))
				return;

			if (_tutorial.TargetHit(this))
				Destroy(this);
			break;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		OnTriggerEnter2D(collision);
	}
}
