using System.Collections;
using UnityEngine;

public class LemmingLightPip : MonoBehaviour
{
	[SerializeField] private bool _enabled = true;
	[SerializeField] private GameObject _indicator;
	[SerializeField, Range(0, 1)] private float _transitionTime = 0.3f;

	private Coroutine _toggleHandle;

	public bool Enabled
	{
		get => _enabled;
		set
		{
			if (_enabled == value)
				return;
			_enabled = value;
			if (_toggleHandle != null)
				StopCoroutine(_toggleHandle);
			_toggleHandle = StartCoroutine(Toggle(_enabled));
		}
	}

	private float _enabledSize;

	private void Awake()
	{
		_enabledSize = _indicator.transform.localScale.x;
	}

	private void Start()
	{
		_indicator.transform.localScale = _enabled ? Vector3.one * _enabledSize : Vector3.zero;
	}

	[ContextMenu(nameof(Toggle))]
	public void Toggle()
	{
		Enabled = !Enabled;
	}

	private IEnumerator Toggle(bool enabled)
	{
		float startSize = _indicator.transform.localScale.x;
		float endSize = enabled ? _enabledSize : 0;
		for (float t = 0; t < _transitionTime; t += Time.deltaTime)
		{
			_indicator.transform.localScale = Mathf.Lerp(startSize, endSize, t / _transitionTime) * Vector3.one;
			yield return new WaitForEndOfFrame();
		}
		_indicator.transform.localScale = Vector3.one * endSize;
		_toggleHandle = null;
	}
}
