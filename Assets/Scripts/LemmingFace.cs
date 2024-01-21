using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemmingFace : MonoBehaviour
{
	[SerializeField] private LemmingMove _lemming;
	[SerializeField] private GameObject[] _scared, _searching, _betterLight, _content;

	private void Update()
	{
		_scared.SetActive(false);
		_searching.SetActive(false);
		_betterLight.SetActive(false);
		_content.SetActive(false);

		switch (_lemming.State)
		{
		case LemmingMove.LemmingState.NoLight:
			_scared.SetActive(true);
			break;
		case LemmingMove.LemmingState.HeadingToLight:
			_searching.SetActive(true);
			break;
		case LemmingMove.LemmingState.HeadingToBetterLight:
			_betterLight.SetActive(true);
			break;
		case LemmingMove.LemmingState.AtLight:
			_content.SetActive(true);
			break;
		}
	}
}
