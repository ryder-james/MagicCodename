using System.Collections;
using UnityEngine;

public class LemmingLightPip : MonoBehaviour
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
	[SerializeField, Range(0.05f, 0.1f)] private float _indicatorScale;
	[SerializeField, Range(0, 1)] private float _transitionTime = 0.3f;

	private Coroutine _stateUpdateHandle;

	public PipState State
	{
		get => _state;
		set
		{
			if (_state == value)
				return;

			_state = value;
			if (_stateUpdateHandle != null)
				StopCoroutine(_stateUpdateHandle);
			_stateUpdateHandle = StartCoroutine(UpdateVisualState());
		}
	}

	private IEnumerator UpdateVisualState()
	{
		bool turningOn = false;
		if (State != PipState.Disabled)
		{
			_indicator.color = State == PipState.Enabled ? _enabledColor : _lockedColor;
			turningOn = true;
		}

		float startSize = turningOn ? 0 : _indicatorScale;
		float endSize = turningOn ? _indicatorScale : 0;

		for (float t = 0; t < _transitionTime; t += Time.deltaTime)
		{
			_indicator.transform.localScale = Mathf.Lerp(startSize, endSize, t / _transitionTime) * Vector3.one;
			yield return new WaitForEndOfFrame();
		}

		_indicator.transform.localScale = Vector3.one * endSize;
		_stateUpdateHandle = null;
	}

#if UNITY_EDITOR
	private void OnValidate()
	{
		_enabledColor.a = 1;
		_lockedColor.a = 1;

		switch (State)
		{
		case PipState.Disabled:
			_indicator.transform.localScale = Vector3.zero;
			break;
		case PipState.Enabled:
		case PipState.Locked:
			_indicator.transform.localScale = Vector3.one * _indicatorScale;
			_indicator.color = State == PipState.Enabled ? _enabledColor : _lockedColor;
			break;
		}
	}
#endif
}
