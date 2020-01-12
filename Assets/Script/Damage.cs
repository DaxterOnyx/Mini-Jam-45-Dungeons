using DG.Tweening;
using System;
using UnityEngine;

public class Damage : MonoBehaviour
{
	public ProjectileData Data;
	internal bool isCritic;
	internal int value;
	internal float pourcentPenetration;
	internal bool isRegen;
	internal string targetTag;

	public Damage(bool isCritic, int value, float pourcentPenetration, bool isRegen, string targetTag)
	{
		this.isCritic = isCritic;
		this.value = value;
		this.pourcentPenetration = pourcentPenetration;
		this.isRegen = isRegen;
		this.targetTag = targetTag;
	}

	public static Damage GetComponent(bool isCritic, int value, float pourcentPenetration, bool isRegen, string targetTag, ref GameObject gameObject)
	{
		var dam = gameObject.GetComponent<Damage>();
		dam.isCritic = isCritic;
		dam.value = value;
		dam.pourcentPenetration = pourcentPenetration;
		dam.isRegen = isRegen;
		dam.targetTag = targetTag;
		return dam;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var c = collision.transform.GetComponent<Character>();
		if (c != null && c.CompareTag(targetTag)) {
			c.DoDamage(this);
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
}