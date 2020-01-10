using UnityEngine;

[CreateAssetMenu(fileName = "New ClassData", menuName = "Data/ClassData")]
public class ClassData: ScriptableObject
{
	public SkillTreeData SkillTree;
	public string Name;
	public float HPRatio = 1;
	public float ArmorRatio = 1;
	public float SpeedRatio = 1;
	public float CriticRatio = 1;
	public float DodgeRatio = 1;
	public float PhysicDommageRatio = 1;
	public float RangeRatio = 1;
	public float AttackSpeedRatio = 1;
	public float ArmorPenetrationRatio = 1;
	public float ManaRatio = 1;
	//TODO HEALER SPEC
}