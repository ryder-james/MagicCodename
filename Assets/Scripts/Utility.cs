using UnityEngine;

public static class Utility {
	public static bool Contains(this LayerMask mask, int layer) => mask == (mask | (1 << layer));
}