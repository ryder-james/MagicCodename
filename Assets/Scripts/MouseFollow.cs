using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFollow : MonoBehaviour
{
	private Vector3 _aim;

	private void Update()
	{
		transform.position = _aim;
	}

	public void Aim(InputAction.CallbackContext ctx)
	{
		Vector2 mousePos = Mouse.current.position.ReadValue();
		Vector3 adjustedMousePos = new Vector3(mousePos.x, mousePos.y, 10);
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(adjustedMousePos);
		_aim = worldPos;
	}
}
