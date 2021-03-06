﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeTileScript : MonoBehaviour
{
    [SerializeField]
    private Text title;
    [SerializeField]
    private Text description;
    [SerializeField]
    private Text cost;
    //private int id;
    private SkillTree skillTree;
    [SerializeField]
    private Button button;
    [SerializeField]
    private GameObject tooltip;
    private BankDataScript bankData;
    private Text counterText;
    private TreeGenerator treeGenerator;

    internal int id{ get; private set; }
    internal Vector2 position { get; private set; }

    public void Start()
    {
        tooltip.SetActive(false);
    }
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

    public void initiate(string title, string description, string cost, SkillTree skillTree, int id, bool interactable, Text text, TreeGenerator treeGenerator, Vector2 position)
    {
        this.bankData = BankDataScript.Instance;
        this.skillTree = skillTree;
        this.title.text = title;
        this.description.text = description;
        this.cost.text = cost;
        this.id = id;
        this.button.interactable = interactable;
        this.counterText = text;
        this.treeGenerator = treeGenerator;
        this.position = position;
    }

    public void setInteractable()
    {
        this.button.interactable = true;
    }

    public void mouseOverIn()
    {
        tooltip.SetActive(true);
    }

    public void mouseOverOut()
    {
        tooltip.SetActive(false);
    }
}
