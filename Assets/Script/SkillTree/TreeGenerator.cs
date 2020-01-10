using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeGenerator : MonoBehaviour
{
    public GameObject TreeTile;
    // Start is called before the first frame update
    void Start()
    {
        //newTile.SetActive(false);
        var skillTree = GetComponent<SkillTree>();
        // Create Canvas GameObject.
        GameObject canvasGO = new GameObject();
        canvasGO.name = "Canvas";
        canvasGO.AddComponent<Canvas>();
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Get canvas from the GameObject.
        Canvas canvas;
        canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        Debug.Log(skillTree.Data.skills.Length);

        // l'arbre est plein (sans interval entre les Id et ID 0 est la racine
        for (int i = 0; i < skillTree.Data.skills.Length; i++)
        {
            var newTile = Instantiate(TreeTile, new Vector3(
                180 * (i % 4) + 90, // X
                Screen.height - (118 * (i / 4) + 59), // Y
                0), Quaternion.identity , canvasGO.transform).GetComponent<TreeTileScript>();
            var skill = skillTree.Data.GetSkill(i);
            //var dependencies = skill.dependencies;
            //var name = skill.name;                            V
            //var unlocked = skill.unlocked;
            //var unlockable = skillTree.CanSkillBeUnlocked(i);
            //var cost = skill.cost;                            V
            //var description = skill.description;              V

            newTile.title.text = skill.name;
            newTile.descripion.text = skill.description;
            newTile.cost.text = skill.cost.ToString();
            newTile.GetComponent<Button>().interactable = skillTree.CanSkillBeUnlocked(i);
        }
        //foreach (var skill in skillTree.Data.skills)
        //{
        //    skill.dependencies
        //}
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
