using System.Collections;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
	[System.Serializable]
	private struct DelayedSprite
	{
		public float Delay;
		public float Duration;
		public GameObject SpriteParent;
	}

	[System.Serializable]
	private struct TutorialMessage
	{
		public string Message;
		public float Duration;
		public DelayedSprite[] RelatedSprites;
		public TutorialTarget MoveTarget;
	}

	[SerializeField] private TMP_Text _textDisplay;
	[SerializeField] private float _fadeTime = 0.5f;
	[SerializeField] private TutorialMessage[] _tutorialMessages;

	private int _tutorialStep = 0;
	private TutorialTarget _nextTarget;

	private void Start()
	{
		Invoke(nameof(NextStep), _fadeTime);
	}

	public void NextStep()
	{
		StartCoroutine(ProgressStep());
	}

	public bool TargetHit(TutorialTarget target)
	{
		if (_nextTarget == null || target != _nextTarget)
			return false;

		NextStep();
		return true;
	}

	private IEnumerator ProgressStep()
	{
		if (_tutorialStep > _tutorialMessages.Length)
			yield break;

		if (_tutorialStep > 0)
			yield return ClearLastStep();

		if (_tutorialStep == _tutorialMessages.Length)
			yield break;

		TutorialMessage step = _tutorialMessages[_tutorialStep];

		_textDisplay.text = step.Message;
		StartCoroutine(DoFadeText(_textDisplay, 1, _fadeTime));
		foreach (var obj in step.RelatedSprites)
			StartCoroutine(EnableDelayedSprite(obj));

		_tutorialStep++;

		if (step.Duration > 0)
			Invoke(nameof(NextStep), step.Duration);
		else if (step.MoveTarget != null)
			_nextTarget = step.MoveTarget;
	}

	private IEnumerator ClearLastStep()
	{
		TutorialMessage lastStep = _tutorialMessages[_tutorialStep - 1];
		StartCoroutine(DoFadeText(_textDisplay, 0, _fadeTime));
		foreach (var obj in lastStep.RelatedSprites)
		{
			foreach (var sprite in obj.SpriteParent.GetComponentsInChildren<SpriteRenderer>())
				StartCoroutine(DoFadeAlpha(sprite, 0, _fadeTime));
		}

		yield return new WaitForSeconds(_fadeTime * 2);
	}

	private IEnumerator EnableDelayedSprite(DelayedSprite delayedSprite)
	{
		if (delayedSprite.Delay > 0)
			yield return new WaitForSeconds(delayedSprite.Delay);
		foreach (var sprite in delayedSprite.SpriteParent.GetComponentsInChildren<SpriteRenderer>())
			StartCoroutine(DoFadeAlpha(sprite, 1, _fadeTime));
		if (delayedSprite.Duration > 0)
		{
			yield return new WaitForSeconds(delayedSprite.Duration);
			foreach (var sprite in delayedSprite.SpriteParent.GetComponentsInChildren<SpriteRenderer>())
				StartCoroutine(DoFadeAlpha(sprite, 0, _fadeTime));
		}
		else
		{
			yield break;
		}
	}

	private IEnumerator DoFadeText(TMP_Text text, float newAlpha, float transitionTime)
	{
		Color c = text.color;
		float start = c.a;
		for (float t = 0; t < transitionTime; t += Time.deltaTime)
		{
			float normalized = t / transitionTime;
			c.a = Mathf.Lerp(start, newAlpha, normalized);
			text.color = c;
			yield return new WaitForEndOfFrame();
		}
		c.a = newAlpha;
		text.color = c;
	}

	private IEnumerator DoFadeAlpha(SpriteRenderer renderer, float newAlpha, float transitionTime)
	{
		if (renderer == null)
			yield break;

		Color c = renderer.color;
		float start = c.a;
		for (float t = 0; t < transitionTime; t += Time.deltaTime)
		{
			float normalized = t / transitionTime;
			c.a = Mathf.Lerp(start, newAlpha, normalized);
			renderer.color = c;
			yield return new WaitForEndOfFrame();
		}
		c.a = newAlpha;
		renderer.color = c;
	}
}
