using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkillTreeData", menuName = "Data/SkillTree")]
public class SkillTreeData : ScriptableObject, IComparer<Skill>
{
	// Array with all the skills in our skilltree
	/// <summary>
	/// Depreciated
	/// </summary>
	public Skill[] skills;

	// Dictionary with the skills in our skilltree
	private Dictionary<int, Skill> skillsDico;

	private void SetupDictionary()
	{
		skillsDico = new Dictionary<int, Skill>();
		foreach (var item in skills) {
			skillsDico.Add(item.id, item);
		}
	}

	public Skill GetSkill(int id)
	{
		if (skillsDico == null)
			SetupDictionary();
		Skill skill;
		if (skillsDico.TryGetValue(id, out skill))
			return skill;

		skill = new Skill();
		skill.id = -1;
		return skill;
	}


	public Skill[] GetDependancies(int id_skill)
	{
		var skill = GetSkill(id: id_skill);
		if (skill.id != -1) // The skill exists
		{
			var dependencies = new Skill[skill.dependencies.Length];
			for (int i = 0; i < skill.dependencies.Length; i++) {

				var dep = GetSkill(skill.dependencies[i]);
				if (dep.id != -1)
					dependencies[i] = dep;
				else // If one of the dependencies doesn't exist.
				{
					Debug.LogError("DEPENDENCIES " + i + " OF SKILL NOT FOUND :" + id_skill);
					return new Skill[0];
				}
			}
			return dependencies;
		} else {
			// If the skill id doesn't exist, the skill can't be unlocked
			Debug.LogError("SKILL NOT FOUND :" + id_skill);
			return new Skill[0];
		}
	}

	public int Compare(Skill x, Skill y)
	{
		return x.id - y.id;
		//throw new System.NotImplementedException();
	}
}