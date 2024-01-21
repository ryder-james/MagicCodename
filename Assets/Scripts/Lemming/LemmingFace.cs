using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemmingFace : MonoBehaviour
{
	[SerializeField] private Lemming _lemming;
	[SerializeField] private GameObject[] _scared, _searching, _betterLight, _content;

	private void Update()
	{
		_scared.SetActive(false);
		_searching.SetActive(false);
		_betterLight.SetActive(false);
		_content.SetActive(false);

		switch (_lemming.State)
		{
		case Lemming.LemmingState.NoLight:
			_scared.SetActive(true);
			break;
		case Lemming.LemmingState.HeadingToLight:
			_searching.SetActive(true);
			break;
		case Lemming.LemmingState.HeadingToBetterLight:
			_betterLight.SetActive(true);
			break;
		case Lemming.LemmingState.AtLight:
			_content.SetActive(true);
			break;
		}
	}
}
