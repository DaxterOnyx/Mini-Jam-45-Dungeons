using DG.Tweening;
using UnityEngine;

public class Character : MonoBehaviour
{
	public CharacterData Data;
	private Character Target;
	[SerializeField]
	private PopupManager popupManager;
	[SerializeField]
	private Animator animator;
	[SerializeField]
	protected Transform SpawnPoint;

	public virtual float Speed()
	{
		return Data.SpeedAmount;
	}

	public virtual int MaxHP()
	{
		return Data.HPAmount;
	}

	public virtual Damage DamageValue()
	{
		return new Damage(false, Data.PhysicDommageAmount, 0f, false, "Ennemy");
	}

	public virtual Damage DamageValue(ref GameObject gameObject)
	{
		var value = DamageValue();
		var dam = Damage.GetComponent(value.isCritic, value.value, value.pourcentPenetration, value.isRegen, value.targetTag, ref gameObject);

		return dam;
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
		Initiate();
	}

	internal bool Survive(Damage dam)
	{
		return HP - dam.value > 0;
	}


	// Update is called once per frame
	void Update()
	{
		cooldownAttack -= Time.deltaTime;

		if (Target == null)
			ResearchTarget();
		if (Target == null) {
			transform.DOKill();
			return;
		}

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
		var characters = CharactersManager.Instance.GetCharacters();

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

		if (targetId == -1) {
			Debug.Log("I win.");
			return;
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

		animator.SetTrigger("Attack");

		//TODO Critik
		//TODO MAGIC DAMAGE
		Debug.Log("I'm hiting. " + gameObject.name + "->" + Target.name);

		return Hit(target);
	}

	protected virtual bool Hit(Character target)
	{
		return target.DoDamage(DamageValue());
	}

	internal void Initiate()
	{
		HP = MaxHP();
	}

	public bool DoDamage(Damage damage)
	{
		Debug.Log("I'm hited. " + gameObject.name + "<-" + Target.name);
		//TODO DODGE
		//TODO ARMOR
		popupManager.PopupValue(damage.value.ToString(), damage.isRegen ? PopupData.Style.HEAL : damage.pourcentPenetration > 0 ? PopupData.Style.MAGIC : damage.isCritic ? PopupData.Style.CRITIC : PopupData.Style.DAMAGE);
		HP += (damage.isRegen ? 1 : -1) * damage.value;
		if (HP <= 0) {
			Die();
		}
		int max = MaxHP();
		if (HP > max)
			HP = max;
		var b = damage.isRegen ? HP == max : HP <= 0;
		return b;
	}

	private void Die()
	{
		Debug.Log("I'm dying. " + gameObject.name);
		animator.SetBool("Dead", true);
		Destroy(gameObject, 0.1f);
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
