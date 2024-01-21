using System.Collections.Generic;
using UnityEngine;

public static class Utility {
	public static bool Contains(this LayerMask mask, int layer) => mask == (mask | (1 << layer));

	public static bool OverlapCircle(Vector2 pos1, float r1, Vector2 pos2, float r2)
	{
		// If either circle has infinite radius, the equation is always true (but might fail anyway)
		if (r1 == Mathf.Infinity || r2 == Mathf.Infinity)
			return true;

		// Two circles intersect if, and only if, the distance between their centers is
		//   between the sum and difference of their radii.
		// Use square distances to save some processing time.

		float rDiff = Mathf.Abs(r1 - r2);
		float rSum = r1 + r2;
		float dist = Vector2.Distance(pos1, pos2);

		return dist <= rDiff || dist <= rSum;
	}

	public static void SetActive(this IEnumerable<GameObject> gameObjects, bool active)
	{
		foreach (GameObject obj in gameObjects)
			obj.SetActive(active);
	}
}