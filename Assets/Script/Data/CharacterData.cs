using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterData", menuName = "Data/CharacterData")]
public class CharacterData : ScriptableObject
{
	public PopupManager PopupManager;

	public int HPAmount = 100;
	public int ArmorAmount = 10;
	public float SpeedAmount = 1;
	public float CriticAmount = 1;
	public float CriticRange = 1;
	public float DodgeAmount = 1;
	public int PhysicDommageAmount = 5;
	public float RangeAmount = 1;
	public float AttackSpeedAmount = 1;
	[Tooltip("Magic Dommage")]
	public float ArmorPenetrationAmount = 25;
	public int ManaAmount = 100;
}