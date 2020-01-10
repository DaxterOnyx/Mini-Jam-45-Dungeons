using UnityEngine;

[System.Serializable]
public struct Skill
{
	public int id;
	public string name;
	public bool unlocked;
	public int cost;
	public int[] dependencies;
	#region Editor
	public Vector2 editor_position;
	#endregion
}
