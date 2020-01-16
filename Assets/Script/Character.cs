using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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

	public virtual float Speed => Data.SpeedAmount;

	public virtual int MaxHP => Data.HPAmount;

	public virtual Damage DamageValue
	{
		get {
			int proba = CriticRatio;
			bool isCrit = (Random.Range(0, 100) < proba);

			int value = Data.PhysicDommageAmount;
			if (isCrit)
				value *= CriticMultiplier;


			return new Damage(isCrit, value, Penetration, false, Target);
		}
	}

	public virtual int CriticRatio => Data.CriticPourcentProba;

	public virtual int CriticMultiplier => Data.CriticMultiplier;

	public virtual Damage DefineDamageComponent(ref GameObject gameObject)
	{
		var value = DamageValue;
		var dam = Damage.GetComponent(value.isCritic, value.value, value.pourcentPenetration, value.isRegen, value.target, ref gameObject);

		return dam;
	}

	public virtual float Range => Data.RangeAmount;

	public virtual float AttackSpeed => Data.AttackSpeed;

	public virtual int Armor => Data.ArmorAmount;

	public virtual int Penetration => Data.ArmorPenetrationPourcentage;

	public virtual int DodgeRatio => Data.DodgeProba;

	public virtual float KnockBackResist => Data.KnockBackResist;

	public string TargetTag => Target.tag;

	internal int HP { get; private set; }
	public bool Living { get { return HP > 0; } }

	private float TargetDistance;
	private float cooldownAttack;
	private TweenerCore<Quaternion, Vector3, QuaternionOptions> tweenRotate;

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
		LookAt(Target.transform.position);
		#endregion

		TargetDistance = Vector2.Distance(transform.position, Target.transform.position);
		if (TargetDistance > Range) {
			MoveTo();
		} else {
			if (cooldownAttack <= 0)
				if (Attack(Target))
					ResearchTarget();
		}


	}

	private void LateUpdate()
	{
		var newpos = transform.position;
		Rect areaToFight = CharactersManager.Instance.AreaToFight;
		if (!areaToFight.Contains((Vector2)transform.position)) {

			if (transform.position.x < areaToFight.x)
				newpos.x = areaToFight.x;
			if (transform.position.x > areaToFight.xMax)
				newpos.x = areaToFight.xMax;
			if (transform.position.y < areaToFight.y)
				newpos.y = areaToFight.y;
			if (transform.position.x > areaToFight.xMax)
				newpos.y = areaToFight.yMax;
			transform.position = newpos;
		}
	}

	private void LookAt(Vector2 position)
	{
		Vector3 diff = new Vector3(position.x, position.y, transform.position.z) - transform.position;
		diff.Normalize();

		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		tweenRotate = transform.DORotate(Quaternion.Euler(0f, 0f, rot_z - 90).eulerAngles, 0.1f);
	}

	private void ResearchTarget()
	{
		var characters = CharactersManager.Instance.GetCharacters();

		if (characters.Length == 1) {
			Debug.Log("I'm alone.");
			return;
		}
		int targetId = SelectTarget(characters);

		if (targetId == -1) {
			Debug.Log("I win.");
			Target = null;
			return;
		}

		DefineTarget(characters[targetId]);

	}

	protected virtual int SelectTarget(Character[] characters)
	{
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

		return targetId;
	}

	protected bool IsFriend(Character character)
	{
		return !character.CompareTag(Data.EnnemyTag);
	}

	public bool Attack(Character target)
	{
		cooldownAttack = 1 / AttackSpeed;

		animator.SetTrigger("Attack");

		Debug.Log("I'm hiting. " + gameObject.name + "->" + Target.name);

		return Hit(target);
	}

	protected virtual bool Hit(Character target)
	{
		Damage damage = DamageValue;
		bool died = target.DoDamage(damage);
		//KnockBack
		if (!damage.isRegen) {
			Damage.KnockBack(target, damage.value, transform.up);
		}
		return died;
	}


	internal void Initiate()
	{
		HP = MaxHP;
	}

	public bool DoDamage(Damage damage)
	{
		//DODGE
		if (!damage.isRegen && Random.Range(0, 100) < DodgeRatio) {
			popupManager.PopupValue("0", PopupData.Style.DODGE);
			return false;
		}


		//ARMOR
		damage.value = Mathf.Max(0, damage.value - Mathf.FloorToInt(Armor * (1 - damage.pourcentPenetration / 100)));

		//POPUP
		//TODO FACTORISER CODE
		popupManager.PopupValue(damage.value.ToString(), damage.isRegen ? PopupData.Style.HEAL : damage.pourcentPenetration > 0 ? PopupData.Style.MAGIC : damage.isCritic ? PopupData.Style.CRITIC : PopupData.Style.DAMAGE);

		//Update HP
		HP += (damage.isRegen ? 1 : -1) * damage.value;

		//REGEN Limite	
		int max = MaxHP;
		if (HP > max)
			HP = max;

		//DIE LIMIT
		if (HP <= 0) {
			Die();
		}

		//return if must change target
		return damage.isRegen ? HP == max : HP <= 0;
	}

	private void Die()
	{
		Debug.Log("I'm dying. " + gameObject.name);
		animator.SetBool("Dead", true);
		Destroy(gameObject, 0.5f);
		Destroy(this);
	}

	public void DefineTarget(Character character)
	{
		Target = character;
	}

	public void MoveTo()
	{
		if (TargetDistance > Range && Living) {

			transform.Translate(Vector3.up * Speed * Time.deltaTime);
		}
	}
}
