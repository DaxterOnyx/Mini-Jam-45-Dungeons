using UnityEngine;

public class Player : Character
{
	public ClassData Class;

	public override float Speed => base.Speed * Class.SpeedRatio;

	public override int MaxHP => (int)(base.MaxHP * Class.HPRatio);

	public override int Penetration => Mathf.Min(100, (int)(base.Penetration * Class.ArmorPenetrationRatio));

	public override Damage DamageValue
	{
		get {
			Damage damage = base.DamageValue;
			damage.value = (int)(damage.value * Class.PhysicDommageRatio);
			damage.isRegen = Class.isHealer;
			damage.pourcentPenetration = Penetration;
			return damage;
		}
	}

	public override float Range => base.Range * Class.RangeRatio;

	public override float AttackSpeed => (base.AttackSpeed * Class.AttackSpeedRatio);

	public override int Armor => Mathf.CeilToInt(base.Armor * Class.ArmorRatio);

	public override int CriticMultiplier => Mathf.CeilToInt(base.CriticMultiplier * Class.CriticMultiRatio);

	public override int CriticRatio => Mathf.CeilToInt(base.CriticRatio * Class.CriticProbaRatio);

	public override int DodgeRatio => Mathf.CeilToInt(base.DodgeRatio * Class.DodgeRatio);

	protected override bool Hit(Character target)
	{
		// TODO MANA USE
		if (Class.LaunchProjectile)
			return LaunchProjectile(target);
		else
			return base.Hit(target);
	}

	private bool LaunchProjectile(Character target)
	{
		var projectile = Instantiate(Class.Projectile, SpawnPoint.position, Quaternion.identity);
		var dam = DefineDamageComponent(ref projectile);

		dam.SetTarget(target);
		
		return Class.isHealer || !target.Survive(dam);
	}

	protected override int SelectTarget(Character[] characters)
	{
		var targetId = -1;
		var hp = 1f;
		var dist = float.MaxValue;
		for (int i = 0; i < characters.Length; i++) {
			if (characters[i] == this) continue;
			if (Class.isHealer) {
				if (IsFriend(characters[i])) {
					float h = (characters[i].HP / (float)characters[i].MaxHP);
					if (h < hp) {
						hp = h;
						targetId = i;
					}
				}
			} else {
				if (IsFriend(characters[i])) continue;

				var d = Vector2.Distance(characters[i].transform.position, transform.position);
				if (d < dist) {
					dist = d;
					targetId = i;
				}
			}
		}

		return targetId;
	}
}
