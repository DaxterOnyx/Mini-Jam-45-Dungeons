using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterData", menuName = "Data/CharacterData")]
public class CharacterData : ScriptableObject
{
	public string EnnemyTag = "Player";
	public int HPAmount = 100;
	public int ArmorAmount = 10;
	public float SpeedAmount = 1;
	public int CriticMultiplier = 2;
	public int CriticPourcentProba = 1;
	public int DodgeProba = 1;
	public int PhysicDommageAmount = 5;
	public float RangeAmount = 1;
	public float AttackSpeed = 1;
	public float KnockBackResist = 0f;
	public float KnonkBackImpact = 0f;
	[Tooltip("Magic Dommage")]
	public int ArmorPenetrationPourcentage = 0;
	public int ManaAmount = 100;

}