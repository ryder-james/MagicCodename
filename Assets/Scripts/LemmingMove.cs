using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LemmingMove : MonoBehaviour
{
	public enum LemmingState
	{
		NoLight,
		HeadingToLight,
		HeadingToBetterLight,
		AtLight
	}

	[SerializeField, Min(0.1f)] private int _speed = 5;
	[SerializeField, Min(0.1f)] private int _randomTimeMin = 2;
	[SerializeField, Min(0.2f)] private int _randomTimeMax = 5;
	[SerializeField, Min(0.1f)] private float _arrivedDistance = 0.5f;
	[SerializeField, Min(0)] private float _calmVisionDistance = 3;
	[SerializeField, Min(0)] private float _scaredVisionDistance = Mathf.Infinity;
	[SerializeField] private LayerMask _raycastBlockers;

	private Target[] _allTargets;
	private Rigidbody2D _rb;
	private Target _target;

	private float _randTime = 2.0f;
	private float _timeElapsed = 0.0f;

	public LemmingState State { get; private set; }

	private bool HasTarget => _target != null;
	private bool IsScared => !HasTarget;

	private Vector3 _moveTarget;
	private Vector3 MoveTarget
	{
		get => _target ? _target.HomingPoint : _moveTarget;
		set => _moveTarget = value;
	}

	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		MoveTarget = transform.position;
		_allTargets = FindObjectsOfType<Target>();
	}

	private void Update()
	{
		if (HasTarget && _target.Priority == 0)
			_target = null;

		_target = PickTarget(HasTarget);

		if (IsScared)
		{
			MoveTarget = new Vector3(transform.position.x + (Random.Range(-1, 2) * 100), transform.position.y + (Random.Range(-1, 2) * 100), 0);
			_randTime = Random.Range(_randomTimeMin, _randomTimeMax);
			_timeElapsed = 0.0f;
		}

		if (Vector3.Distance(transform.position, MoveTarget) > _arrivedDistance)
		{
			Vector2 dir = MoveTarget - transform.position;
			_rb.velocity = dir.normalized * _speed;
		}
		else
		{
			_rb.velocity = Vector3.zero;
			if (HasTarget)
				State = LemmingState.AtLight;
		}

		if (_timeElapsed < _randTime && IsScared)
		{
			_timeElapsed += Time.deltaTime;
		}
		else
		{
			MoveTarget = transform.position;
		}

	}

	private Target PickTarget(bool considerPriority)
	{
		float visionDistance = IsScared ? _scaredVisionDistance : _calmVisionDistance;

		Target result = _target;

		int targetPriority = HasTarget ? _target.Priority : int.MinValue;
		float distanceToTarget = HasTarget ? Vector3.Distance(transform.position, MoveTarget) : Mathf.Infinity;

		List<Target> _viableTargets = _allTargets.Where(target => ConsiderTarget(target, visionDistance, considerPriority)).ToList();

		// If there's only one viable target, shortcut out.
		if (_viableTargets.Count == 1)
		{
			result = _viableTargets[0];
		}
		else
		{
			// Filter out every target with a lower priority than our current, then
			//   find every Target which is "visible" to our Lemming (where the
			//   Light radius intersects our Vision radius)
			foreach (Target target in _viableTargets)
			{
				if (target == _target)
					continue;

				float distanceToNewTarget = Vector3.Distance(target.HomingPoint, transform.position);

				// If the target has a higher importance or if it's just closer (or if we're ignoring priority), pick it instead
				bool higherPriority = target.Priority > targetPriority;
				bool closer = distanceToNewTarget < distanceToTarget;
				if (considerPriority)
				{
					if (higherPriority || (target.Priority == targetPriority && closer))
					{
						result = target;
						distanceToTarget = distanceToNewTarget;
						targetPriority = target.Priority;
					}
				}
				else if (closer)
				{
					result = target;
					distanceToTarget = distanceToNewTarget;
					targetPriority = target.Priority;
				}
			}
		}

		if (result == null)
		{
			State = LemmingState.NoLight;
		}
		else if (_target == null)
		{
			State = LemmingState.HeadingToLight;
		}
		else if (_target == result)
		{
			State = LemmingState.HeadingToBetterLight;
		}

		return result;

		bool ConsiderTarget(Target t, float visionDistance, bool considerPriority)
		{
			bool priorityIsHighEnough = t.Priority > 0 && (!considerPriority || !HasTarget || t.Priority >= _target.Priority);
			if (!priorityIsHighEnough)
				return false;

			if (t == _target)
				return true;

			bool radiiOverlap = Utility.OverlapCircle(t.HomingPoint, t.Priority, transform.position, visionDistance);
			if (!radiiOverlap)
				return false;
			
			// Check if there's a wall or other RaycastBlocker between us and the target
			var raycastHit = Physics2D.Raycast(transform.position, t.HomingPoint - transform.position, Mathf.Infinity, _raycastBlockers);
			bool hasLOS = !raycastHit;
			if (!hasLOS)
				return false;

			return true;
		}
	}

#if UNITY_EDITOR
	private void OnDrawGizmosSelected()
	{
		float distance = (Application.IsPlaying(gameObject) && IsScared) ? _scaredVisionDistance : _calmVisionDistance;
		if (distance == Mathf.Infinity)
			return;

		Gizmos.DrawWireSphere(transform.position, distance);
	}
#endif
}
