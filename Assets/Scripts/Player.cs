using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {
	[SerializeField] private float _speed = 100;
	[SerializeField] private float _movingDrag = 0.5f;
	[SerializeField] private float _stoppingDrag = 2f;
	[SerializeField] private float _maxSpeed = 10;
	[SerializeField] private float _pipOrbitRadius = 0.3f;
	[SerializeField] private GameObject _pipPrefab;
	[SerializeField] private Transform _pipParent;
	[SerializeField] private float _pipRotationSpeed = 180;
	[SerializeField] private LineRenderer _aimLine;

	private int _carriedPips;
	private LemmingLight _nearestLight;
	private Vector2 _movement;
	private Rigidbody2D _rb;
	private List<GameObject> _pips = new();
	private float _pipPhase;

	private void Awake() {
		_rb = GetComponent<Rigidbody2D>();
	}

	private void Update() {
		_pipPhase += _pipRotationSpeed * Time.deltaTime;
		_pipPhase %= 360;
		_pipParent.rotation = Quaternion.Euler(0, 0, _pipPhase);
		if (_carriedPips > 0) {
			_aimLine.enabled = true;
			_aimLine.positionCount = 2;

			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = 0;

			var hit = Physics2D.Raycast(transform.position, mousePos - transform.position, Mathf.Infinity, ~LayerMask.NameToLayer("Player"));
			Debug.Log(hit.point);
			_aimLine.SetPosition(0, transform.position);
			_aimLine.SetPosition(1, hit.point);
		} else {
			_aimLine.enabled = false;
		}
	}

	private void FixedUpdate() {
		_rb.drag = _movement == Vector2.zero ? _stoppingDrag : _movingDrag;

		_rb.AddForce(_speed * Time.deltaTime * _movement);
		_rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxSpeed);
	}

	private void AddPip() {
		_pips.Add(Instantiate(_pipPrefab, _pipParent));
		_carriedPips++;
		ResetPipPositions();
	}

	private void RemovePip() {
		_carriedPips--;
		var oldPip = _pips[_carriedPips];
		Destroy(oldPip);
		_pips.Remove(oldPip);
		ResetPipPositions();
	}

	private void ResetPipPositions() {
		for (int i = 0; i < _carriedPips; i++) {
			float theta = (2 * Mathf.PI / _carriedPips) * i;
			_pips[i].transform.localPosition = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)) * _pipOrbitRadius;
		}
	}

	public void Move(InputAction.CallbackContext ctx) {
		Vector2 input = ctx.ReadValue<Vector2>();
		_movement = input.normalized;
	}

	public void Fire(InputAction.CallbackContext ctx) {
		if (ctx.phase == InputActionPhase.Performed && _nearestLight) {
			if (_carriedPips > 0 && _nearestLight.RaiseIntensity()) {
				RemovePip();
			}
		}
	}

	public void Steal(InputAction.CallbackContext ctx) {
		if (ctx.phase == InputActionPhase.Performed) {
			if (_nearestLight && _nearestLight.LowerIntensity()) {
				AddPip();
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (!other.TryGetComponent(out LemmingLight light))
			return;

		if (_nearestLight != null) {
			if (Vector3.Distance(transform.position, light.transform.position) < Vector3.Distance(transform.position, _nearestLight.transform.position)) {
				_nearestLight = light;
			}
		} else {
			_nearestLight = light;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (!other.TryGetComponent(out LemmingLight light))
			return;

		if (_nearestLight == null || _nearestLight != light)
			return;

		_nearestLight = null;
	}
}