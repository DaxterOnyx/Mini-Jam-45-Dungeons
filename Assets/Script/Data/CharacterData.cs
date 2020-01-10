using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterData", menuName = "Data/CharacterData")]
public class CharacterData : ScriptableObject
{
	public float DefaultHPAmount = 100;
	public float DefaultArmorAmount = 10;
	public float DefaultSpeedAmount = 1;
	public float DefaultCriticAmount = 1;
	public float DefaultDodgeAmount = 1;
	public float DefaultPhysicDommageAmount = 5;
	public float DefaultRangeAmount = 1;
	public float DefaultAttackSpeedAmount = 1;
	[Tooltip("Magic Dommage")]
	public float DefaultArmorPenetrationAmount = 25;
	public float DefaultManaAmount = 100;
}