using UnityEngine;

public class SkillTree
{
	public SkillTreeData Data;

	public bool CanSkillBeUnlocked(int id_skill)
	{
		bool canUnlock = true;
		var skill = Data.GetSkill(id: id_skill);
		if (skill.id != -1) // The skill exists
		{
			if (skill.unlocked == true)
				return false;

			Skill[] dependencies = Data.GetDependancies(id_skill);
			for (int i = 0; i < dependencies.Length; ++i) {
				if (!dependencies[i].unlocked) {
					canUnlock = false;
					break;
				}
			}
		} else {
			// If the skill id doesn't exist, the skill can't be unlocked
			Debug.LogError("SKILL NOT FOUND :" + id_skill);
			return false;
		}
		return canUnlock;
	}
	public bool UnlockSkill(int id_Skill)
	{
		var skill = Data.GetSkill(id_Skill);
		if (skill.id != -1) {
			skill.unlocked = true;
			return true;
		} else {
			return false;   // The skill doesn't exist
		}
	}
}
