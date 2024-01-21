using UnityEngine;

public class PowerPip : MonoBehaviour
{
	public enum PipState
	{
		Disabled,
		Enabled,
		Locked
	}

	[SerializeField] private PipState _state = PipState.Disabled;
	[SerializeField] private SpriteRenderer _indicator;
	[SerializeField, ColorUsage(false)] private Color _enabledColor, _lockedColor;

	public PipState State
	{
		get => _state;
		set
		{
			if (_state == value)
				return;

			_state = value;

			switch (_state)
			{
			case PipState.Disabled:
				_indicator.gameObject.SetActive(false);
				break;
			case PipState.Enabled:
			case PipState.Locked:
				_indicator.gameObject.SetActive(true);
				_indicator.color = State == PipState.Enabled ? _enabledColor : _lockedColor;
				break;
			}
		}
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		_enabledColor.a = 1;
		_lockedColor.a = 1;
	}
#endif
}
