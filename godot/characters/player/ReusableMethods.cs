using System;
using Godot;

public static class ReusableMethods{
	public static Vector3 LerpVector3 (Vector3 from, Vector3 to, float weight)
	{
		return new Vector3(Mathf.Lerp(from.X, to.X, weight), Mathf.Lerp(from.Y, to.Y, weight), Mathf.Lerp(from.Z, to.Z, weight));
	}

    public static Vector3 LerpVector3WithDifferentWeightForY (Vector3 from, Vector3 to, float weight, float jumpWeight)
	{
		return new Vector3(Mathf.Lerp(from.X, to.X, weight), Mathf.Lerp(from.Y, to.Y, jumpWeight), Mathf.Lerp(from.Z, to.Z, weight));
	}
}
