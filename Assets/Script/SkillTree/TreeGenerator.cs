using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeGenerator : MonoBehaviour
{
    public GameObject treeTile;
    public RectTransform container;

    // Start is called before the first frame update
    void Start()
    {
        //newTile.SetActive(false);
        var skillTree = GetComponent<SkillTree>();
        container.anchoredPosition = Vector2.zero; //localPosition = Vector3.zero;
        container.sizeDelta = new Vector2(
            0, // X
            118 + 59  + 207 + (118 * (skillTree.Data.skills.Length / Mathf.FloorToInt(Screen.width / 180))) // Y
            );
        var ids = new int[skillTree.Data.skills.Length];
        for (int i = 0; i < ids.Length; i++)
        {
            ids[i] = skillTree.Data.skills[i].id;
        }
        Array.Sort(ids);
        int[] bases = Array.FindAll(ids, c => c<100 );
        int[] firstlevel = Array.FindAll(ids, c => c > 100 && c < 200);

        int[][] levels = { bases, firstlevel };
        // l'arbre est plein (sans interval entre les Id et ID 0 est la racine
        int itemsInRaw = Mathf.FloorToInt(Screen.width / 180);
        int itemsInCurrentRaw = itemsInRaw;
        int  rawCounter = 0, gapCounter = 0;
        int defaultHeight = 89 + 59, minHeight = 0, rawHeight = 118, gapHeight = 59;
        foreach (int[] i in levels)
        {

            for (int j = 0; j < i.Length; j++)
            {
                rawCounter += j != 0 && j % itemsInRaw == 0  ? 1 : 0;
                minHeight = defaultHeight + gapCounter * gapHeight;

                itemsInCurrentRaw = (j >= (itemsInRaw * Mathf.FloorToInt(i.Length / itemsInRaw))) ? i.Length - (Mathf.FloorToInt(i.Length / itemsInRaw) * itemsInRaw) : itemsInRaw;

                var newTile = Instantiate(treeTile, new Vector3(
                    (j % itemsInCurrentRaw) * 180 - (itemsInCurrentRaw * 90) + 90 , //180 * (i % (Screen.width / 180)) + 90 , // X
                    0 - (rawHeight * rawCounter) - minHeight, // Y Screen.height
                    0), Quaternion.identity , container.transform).GetComponent<TreeTileScript>();
                var skill = skillTree.Data.GetSkill(i[j]);


                newTile.initiate(
                    skill.name,
                    skill.description,
                    skill.cost.ToString(),
                    skillTree,
                    i[j],
                    skillTree.CanSkillBeUnlocked(i[j]));
            }
            gapCounter++;
            rawCounter++;
        }

    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
