using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {
	[SerializeField] private float _speed = 100;
	[SerializeField] private float _movingDrag = 0.5f;
	[SerializeField] private float _stoppingDrag = 2f;
	[SerializeField] private float _maxSpeed = 10;

	private Vector2 _movement;
	private Rigidbody2D _rb;

	private void Awake() {
		_rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate() {
		_rb.drag = _movement == Vector2.zero ? _stoppingDrag : _movingDrag;

		_rb.AddForce(_speed * Time.deltaTime * _movement);
		_rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxSpeed);
	}

	public void Move(InputAction.CallbackContext ctx) {
		Vector2 input = ctx.ReadValue<Vector2>();
		_movement = input.normalized;
	}

	public void Fire(InputAction.CallbackContext ctx) {
		if (ctx.phase == InputActionPhase.Performed) {
			FindObjectOfType<LemmingLight>().RaiseIntensity();
		}
	}

	public void Steal(InputAction.CallbackContext ctx) {
		if (ctx.phase == InputActionPhase.Performed) {
			FindObjectOfType<LemmingLight>().LowerIntensity();
		}
	}
}
