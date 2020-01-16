using UnityEngine;

public class Damage : MonoBehaviour
{
	public ProjectileData Data;
	internal bool isCritic;
	internal int value;
	internal float pourcentPenetration;
	internal bool isRegen;
	internal Character target;

	public Damage(bool isCritic, int value, float pourcentPenetration, bool isRegen, Character target)
	{
		this.isCritic = isCritic;
		this.value = value;
		this.pourcentPenetration = pourcentPenetration;
		this.isRegen = isRegen;
		this.target = target;
	}

	public static Damage GetComponent(bool isCritic, int value, float pourcentPenetration, bool isRegen, Character target, ref GameObject gameObject)
	{
		var dam = gameObject.GetComponent<Damage>();
		dam.isCritic = isCritic;
		dam.value = value;
		dam.pourcentPenetration = pourcentPenetration;
		dam.isRegen = isRegen;
		dam.target = target;
		return dam;
	}

	private void Update()
	{
		if (target == null || !target.isActiveAndEnabled)
			Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var c = collision.transform.GetComponent<Character>();
		if (c != null && ((isRegen && c == target) || (!isRegen && c.CompareTag(target.tag)))) {
			if (c.DoDamage(this)) {

				//KnockBack
				if (!isRegen) {
					KnockBack(c, value, transform.up);
				}
			}
			Destroy(gameObject);
		}
	}

	internal void SetTarget(Character target)
	{
		var diff = target.transform.position - transform.position;
		diff.Normalize();

		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
		GetComponent<Rigidbody2D>().velocity = diff * Data.Velocity;
	}

	public static void KnockBack(Character target, float damage, Vector2 dir)
	{
		dir.Normalize();
		float knockbackValue = damage * 0.1f * (1f - target.KnockBackResist);
		Vector2 position = new Vector2(target.transform.position.x, target.transform.position.y);
		target.transform.Translate((dir * knockbackValue), Space.World);
	}
}