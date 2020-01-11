using UnityEngine;

[CreateAssetMenu(fileName = "New PopupData", menuName = "Data/PopupData")]
public class PopupData : ScriptableObject
{
	public enum Style
	{
		DAMAGE,
		CRITIC,
		MAGIC,
		DODGE,
		HEAL
	}
	public int DefaultSize = 50;
	public float DefaultLifeTime = 0.2f;
	[System.Serializable]
	public struct UIStyle
	{
		public Color Color;
		public float Size;
		public float LifeTime;
	}

	public UIStyle[] styles = new UIStyle[5];
	public GameObject PopupPrefab;
}