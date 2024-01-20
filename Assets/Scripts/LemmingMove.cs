using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LemmingMove : MonoBehaviour
{
	[SerializeField] private int _speed = 5;
	[SerializeField] private int _randomTimeMin = 2;
	[SerializeField] private int _randomTimeMax = 5;
	[SerializeField] private LayerMask _raycastIgnore;

	private Rigidbody2D _rb;
	private Vector3 _currentTarget = Vector3.zero;
	private float _randTime = 2.0f;
	private float _timeElapsed = 0.0f;
	private int _targetPriority = 0;
	private bool _isScared = true;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		_currentTarget = transform.position;
	}

	private void Update()
	{
		foreach (Target target in FindObjectsOfType<Target>()/*.Select(t => t.transform.position)*/)
		{
			if (target.Priority < _targetPriority)
				continue;

			float disToNewTarget = Vector3.Distance(transform.position, target.transform.position);
			//check if lemming can see target
			if (disToNewTarget > target.Priority)
				continue;

			RaycastHit2D hit = Physics2D.Raycast(transform.position, (target.transform.position - transform.position), Mathf.Infinity, ~_raycastIgnore);
			//check if there is a obstical between lemming and target
			if (hit.collider.gameObject != target.gameObject)
				continue;

			//checks to see if new target has a higher priority or if it is closer
			if (target.Priority > _targetPriority)
			{
				_currentTarget = target.transform.position;
				_targetPriority = target.Priority;
				_isScared = false;
			}
			else
			{
				float disToCurTarget = Vector3.Distance(transform.position, _currentTarget);
				if (disToNewTarget >= disToCurTarget)
					continue;

				_currentTarget = target.transform.position;
				_targetPriority = target.Priority;
				_isScared = false;
			}
		}
		//end foreach
		//move lemming

		if (_currentTarget == transform.position)
		{
			_currentTarget = new Vector3(transform.position.x + (Random.Range(-1, 2) * 100), transform.position.y + (Random.Range(-1, 2) * 100), 0);
			_randTime = Random.Range(_randomTimeMin, _randomTimeMax);
			_timeElapsed = 0.0f;
			_isScared = true;
		}

		Vector2 dir = new Vector2(_currentTarget.x - transform.position.x, _currentTarget.y - transform.position.y);
		_rb.velocity = dir.normalized * _speed;

		if (_timeElapsed < _randTime && _isScared)
		{
			_timeElapsed += Time.deltaTime;
		}
		else
		{
			_currentTarget = transform.position;
			_targetPriority = 0;
		}

	}
}
