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
    //private int id;
    private SkillTree skillTree;
    [SerializeField]
    private Button button;
    private BankDataScript bankData;
    private Text counterText;
    private TreeGenerator treeGenerator;

    internal int id{ get; private set; }
    public void Click()
    {
        if (bankData.payGems(int.Parse(cost.text)))
        {
            skillTree.UnlockSkill(this.id);
            button.interactable = false;
            counterText.text = bankData.gems.ToString();
            treeGenerator.updateTree(this.id);
        }
    }

    public void initiate(string title, string description, string cost, SkillTree skillTree, int id, bool interactable, Text text, TreeGenerator treeGenerator)
    {
        this.bankData = BankDataScript.Instance;
        this.skillTree = skillTree;
        this.title.text = title;
        this.descripion.text = description;
        this.cost.text = cost;
        this.id = id;
        this.button.interactable = interactable;
        this.counterText = text;
        this.treeGenerator = treeGenerator;
    }

    public void setInteractable()
    {
        this.button.interactable = true;
    }
}
