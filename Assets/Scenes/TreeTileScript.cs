using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeTileScript : MonoBehaviour
{
    [SerializeField]
    private Text title;
    [SerializeField]
    private Text descripion;
    [SerializeField]
    private Text cost;
    private int id;
    private SkillTree skillTree;
    [SerializeField]
    private Button button;

    public void Click()
    {
        skillTree.UnlockSkill(id);
        button.interactable = false;
    }

    public void initiate(string title, string description, string cost, SkillTree skillTree, int id, bool interactable)
    {
        this.skillTree = skillTree;
        this.title.text = title;
        this.descripion.text = description;
        this.cost.text = cost;
        this.id = id;
        this.button.interactable = interactable;
    }
}
