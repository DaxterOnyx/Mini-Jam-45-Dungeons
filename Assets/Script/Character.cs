using DG.Tweening;
using UnityEngine;

public class Character : MonoBehaviour
{
	public CharacterData Data;
	private Character Target;
	public PopupManager popupManager;

	public virtual float Speed()
	{
		return Data.SpeedAmount;
	}

	public virtual int MaxHP()
	{
		return Data.HPAmount;
	}

	public virtual int Dammage()
	{
		return Data.PhysicDommageAmount;
	}

	public virtual float Range()
	{
		return Data.RangeAmount;
	}

	public virtual float AttackSpeed()
	{
		return Data.AttackSpeedAmount;
	}

	private int HP;
	private float TargetDistance;
	private float cooldownAttack;

	// Start is called before the first frame update
	void Start()
	{
		if (popupManager == null)
			popupManager = GetComponentInChildren<PopupManager>();
		if (popupManager == null)
			Debug.LogError("Character not have PopupManager");
		Initiate(Data);
	}


	// Update is called once per frame
	void Update()
	{
		cooldownAttack -= Time.deltaTime;

		if (Target == null)
			ResearchTarget();
		if (Target == null)
			return;

		#region rotate
		Vector3 diff = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z) - transform.position;
		diff.Normalize();

		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		transform.DORotate(Quaternion.Euler(0f, 0f, rot_z - 90).eulerAngles, 0.1f);
		#endregion

		TargetDistance = Vector2.Distance(transform.position, Target.transform.position);
		if (TargetDistance > Range()) {
			DefineAim(Target.transform.position);
		} else {
			transform.DOMove(transform.position, 0);
			if (cooldownAttack <= 0)
				if (Attack(Target))
					ResearchTarget();
		}


	}

	private void ResearchTarget()
	{
		var characters = FindObjectsOfType<Character>();

		if (characters.Length == 1) {
			Debug.Log("I'm alone.");
			return;
		}

		var targetId = -1;
		var dist = float.MaxValue;
		for (int i = 0; i < characters.Length; i++) {
			if (characters[i] == this) continue;
			if (IsFriend(characters[i])) continue;

			var d = Vector2.Distance(characters[i].transform.position, transform.position);
			if (d < dist) {
				dist = d;
				targetId = i;
			}
		}

		Target = characters[targetId];

	}

	protected virtual bool IsFriend(Character character)
	{
		return character.CompareTag("Ennemy");
	}

	public bool Attack(Character target)
	{
		cooldownAttack = 1 / AttackSpeed();
		//TODO Critik
		//TODO MAGIC DAMAGE
		return target.Hit(Dammage());
	}

	internal void Initiate(CharacterData data)
	{
		Data = data;
		HP = MaxHP();
	}

	private bool Hit(int damage)
	{
		//TODO DODGE
		//TODO ARMOR
		popupManager.PopupValue(damage.ToString(), PopupData.Style.DAMAGE);
		HP -= damage;
		if (HP <= 0) {
			Die();
			return true;
		}
		return false;
	}

	private void Die()
	{
		Destroy(gameObject);
	}

	public void DefineTarget(Character character)
	{
		Target = character;
		DefineAim(character.transform.position);
	}

	public void DefineAim(Vector2 aimPosition)
	{
		transform.DOMove(aimPosition, TargetDistance / Speed());
	}
}
