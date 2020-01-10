using UnityEngine;

[System.Serializable]
public struct Skill
{
	public int id;
	public string name;
	public int[] dependencies;
	public bool unlocked;
	#region Editor
	public Vector2 position;
	#endregion
}
