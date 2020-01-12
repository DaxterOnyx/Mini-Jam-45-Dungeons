using System;
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

	public override Damage DamageValue()
	{
		Damage damage = base.DamageValue();
		damage.value = (int)(damage.value * Class.PhysicDommageRatio);
		return damage;
	}

	public override float Range()
	{
		return base.Range() * Class.RangeRatio;
	}
	
	public override float AttackSpeed()
	{
		return (base.AttackSpeed() * Class.AttackSpeedRatio);
	}

	protected override bool IsFriend(Character character)
	{
		return character.CompareTag("Player");
	}

	protected override bool Hit(Character target)
	{
		if (Class.LaunchProjectile)
			return LaunchProjectile(target);
		else
			return base.Hit(target);
	}

	private bool LaunchProjectile(Character target)
	{
		var projectile = Instantiate(Class.Projectile,SpawnPoint.position,Quaternion.identity);
		var dam = DamageValue(ref projectile);

		dam.SetTarget(target);

		return !target.Survive(dam);
	}
}
