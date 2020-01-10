using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkillTreeData", menuName = "Data/SkillTree")]
public class SkillTreeData : ScriptableObject
{
	public string JSonPath = "Asset/Data/Skill Trees/";

	// Array with all the skills in our skilltree
	private Skill[] _skillTree;

	// Dictionary with the skills in our skilltree
	private Dictionary<int, Skill> _skills;

	// Variable for caching the currently being inspected skill
	private Skill _skillInspected;

	// Use this for initialization of the skill tree
	void SetUpSkillTree()
	{
		_skills = new Dictionary<int, Skill>();

		LoadSkillTree();
	}

	private void LoadSkillTree()
	{
		string dataAsJson;
		if (File.Exists(JSonPath)) {
			// Read the json from the file into a string
			dataAsJson = File.ReadAllText(JSonPath);

			// Pass the json to JsonUtility, and tell it to create a SkillTree object from it
			SkillTree loadedData = JsonUtility.FromJson<SkillTree>(dataAsJson);

			// Store the SkillTree as an array of Skill
			_skillTree = new Skill[loadedData.skills.Length];
			_skillTree = loadedData.skills;

			// Populate a dictionary with the skill id and the skill data itself
			for (int i = 0; i < _skillTree.Length; ++i) {
				_skills.Add(_skillTree[i].id, _skillTree[i]);
			}
		} else {
			Debug.LogError("Cannot load game data!");
		}
	}

	public bool IsSkillUnlocked(int id_skill)
	{
		if (_skills.TryGetValue(id_skill, out _skillInspected)) {
			return _skillInspected.unlocked;
		} else {
			return false;
		}
	}

	public bool CanSkillBeUnlocked(int id_skill)
	{
		bool canUnlock = true;
		if (_skills.TryGetValue(id_skill, out _skillInspected)) // The skill exists
		{
			int[] dependencies = _skillInspected.dependencies;
			for (int i = 0; i < dependencies.Length; ++i) {
				if (_skills.TryGetValue(dependencies[i], out _skillInspected)) {
					if (!_skillInspected.unlocked) {
						canUnlock = false;
						break;
					}
				} else // If one of the dependencies doesn't exist, the skill can't be unlocked.
				  {
					return false;
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
		if (_skills.TryGetValue(id_Skill, out _skillInspected)) {
			_skillInspected.unlocked = true;

			// We replace the entry on the dictionary with the new one (already unlocked)
			_skills.Remove(id_Skill);
			_skills.Add(id_Skill, _skillInspected);

			return true;
		} else {
			return false;   // The skill doesn't exist
		}
	}
}
