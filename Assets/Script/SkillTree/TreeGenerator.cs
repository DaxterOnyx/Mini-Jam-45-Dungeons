using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeGenerator : MonoBehaviour
{
    public GameObject treeTile;
    public GameObject container;
    // Start is called before the first frame update
    void Start()
    {
        //newTile.SetActive(false);
        var skillTree = GetComponent<SkillTree>();
        // Create Canvas GameObject.
        //GameObject canvasGO = new GameObject();
        //canvasGO.name = "Canvas";
        //canvasGO.AddComponent<Canvas>();
        //canvasGO.AddComponent<CanvasScaler>();
        //canvasGO.AddComponent<GraphicRaycaster>();
        //GameObject viewport = new GameObject();
        //viewport.name = "viewport";

        ////canvasGO.AddComponent<ScrollRect>();

        //// Get canvas from the GameObject.
        //Canvas canvas;
        //canvas = canvasGO.GetComponent<Canvas>();
        //canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        //var newPanel = Instantiate(TreeTilePanel, new Vector3(Screen.width / 2, Screen.height / 2, 0), Quaternion.identity, canvasGO.transform);
        var ids = new int[skillTree.Data.skills.Length];
        for (int i = 0; i < ids.Length; i++)
        {
            ids[i] = skillTree.Data.skills[i].id;
        }
        Array.Sort(ids);
        // l'arbre est plein (sans interval entre les Id et ID 0 est la racine
        for (int i = 0; i < skillTree.Data.skills.Length; i++)
        {
            var newTile = Instantiate(treeTile, new Vector3(
                (i % Mathf.FloorToInt(Screen.width / 180)) * 180 - (Mathf.FloorToInt(Screen.width / 180) * 90) + 90 , //180 * (i % (Screen.width / 180)) + 90 , // X
                0 - (118 * (i / Mathf.FloorToInt(Screen.width / 180))), // Y Screen.height - 59
                0), Quaternion.identity , container.transform).GetComponent<TreeTileScript>();
            var skill = skillTree.Data.GetSkill(ids[i]);
            //var dependencies = skill.dependencies;
            //var name = skill.name;                            V
            //var unlocked = skill.unlocked;
            //var unlockable = skillTree.CanSkillBeUnlocked(i);
            //var cost = skill.cost;                            V
            //var description = skill.description;              V

            newTile.title.text = skill.name;
            newTile.descripion.text = skill.description;
            newTile.cost.text = skill.cost.ToString();
            newTile.GetComponent<Button>().interactable = skillTree.CanSkillBeUnlocked(ids[i]);
        }
        container.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; //localPosition = Vector3.zero;
        container.GetComponent<RectTransform>().sizeDelta = new Vector2(
            0, // X
            118 + 59 + (118 * (skillTree.Data.skills.Length / Mathf.FloorToInt(Screen.width / 180))) // Y
            );
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
