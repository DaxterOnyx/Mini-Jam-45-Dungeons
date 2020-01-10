using UnityEngine;

public class Player : Character
{
	public ClassData Class;

	public override float Speed()
	{
		return base.Speed() * Class.SpeedRatio;
	}

	public override int MaxHP()
	{
		return (int) (base.MaxHP() * Class.HPRatio);
	}

	public override int Dammage()
	{
		return (int)(base.Dammage() * Class.PhysicDommageRatio);
	}

	public override float Range()
	{
		return base.Dammage() * Class.RangeRatio;
	}
	
	public override float AttackSpeed()
	{
		return (base.AttackSpeed() * Class.AttackSpeedRatio);
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
