using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
	[SerializeField] private bool _useAutoDimensions = true;
	[SerializeField] private Vector2 _outerDimensions = new Vector2(20, 4);
	[SerializeField, Min(0.25f)] private float _wallThickness = 0.5f;
	[SerializeField] private GameObject _backdrop;
	[SerializeField] private GameObject[] _outerWalls = new GameObject[4];

#if UNITY_EDITOR
	private void OnValidate()
	{
		if (!_useAutoDimensions)
			return;

		if (_outerWalls.Any(obj => obj == null) || _outerWalls.Length != 4)
			return;

		_outerDimensions.x = Mathf.Max(_outerDimensions.x, 1);
		_outerDimensions.y = Mathf.Max(_outerDimensions.y, 1);

		float outsideX = (_outerDimensions.x * 0.5f) - (_wallThickness * 0.5f);
		float outsideY = (_outerDimensions.y * 0.5f) - (_wallThickness * 0.5f);

		float width = _outerDimensions.x;
		float height = _outerDimensions.y;

		_outerWalls[0].transform.localPosition = new Vector3(0, outsideY, 0);
		_outerWalls[0].transform.rotation = Quaternion.identity;
		_outerWalls[0].transform.localScale = new Vector3(width, _wallThickness, 1);

		_outerWalls[1].transform.localPosition = new Vector3(outsideX, 0, 0);
		_outerWalls[1].transform.rotation = Quaternion.identity;
		_outerWalls[1].transform.localScale = new Vector3(_wallThickness, height, 1);

		_outerWalls[2].transform.localPosition = new Vector3(0, -outsideY, 0);
		_outerWalls[2].transform.rotation = Quaternion.identity;
		_outerWalls[2].transform.localScale = new Vector3(width, _wallThickness, 1);

		_outerWalls[3].transform.localPosition = new Vector3(-outsideX, 0, 0);
		_outerWalls[3].transform.rotation = Quaternion.identity;
		_outerWalls[3].transform.localScale = new Vector3(_wallThickness, height, 1);

		if (_backdrop == null)
			return;

		_backdrop.transform.localPosition = Vector3.zero;
		_backdrop.transform.rotation = Quaternion.identity;
		_backdrop.transform.localScale = new Vector3(_outerDimensions.x, _outerDimensions.y, 1);
	}
#endif
}
