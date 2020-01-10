using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterData", menuName = "Data/CharacterData")]
public class CharacterData : ScriptableObject
{
	public float HPAmount = 100;
	public float ArmorAmount = 10;
	public float SpeedAmount = 1;
	public float CriticAmount = 1;
	public float DodgeAmount = 1;
	public float PhysicDommageAmount = 5;
	public float RangeAmount = 1;
	public float AttackSpeedAmount = 1;
	[Tooltip("Magic Dommage")]
	public float ArmorPenetrationAmount = 25;
	public float ManaAmount = 100;
}