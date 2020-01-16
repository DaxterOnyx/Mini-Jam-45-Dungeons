using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TreeEditor;

public class TreeGeneratorCircle : TreeGenerator {
    //public GameObject treeTile;
    //public Image beam;
    //public RectTransform tileContainer;
    //public RectTransform brancheContainer;
    //public Text gemText;

    private List<TreeTileScript> tileList;
    private SkillTree skillTree;

    // Start is called before the first frame update
    void Start() {
        //newTile.SetActive(false);
        tileList = new List<TreeTileScript>();
        skillTree = GetComponent<SkillTree>();
        tileContainer.anchoredPosition = Vector2.zero; //localPosition = Vector3.zero;
        brancheContainer.anchoredPosition = Vector2.zero;
        var ids = new int[skillTree.Data.skills.Length];
        for (int i = 0; i < ids.Length; i++) {
            ids[i] = skillTree.Data.skills[i].id;
        }
        Array.Sort(ids);
        int numberOfLevel = Mathf.FloorToInt(ids[ids.Length - 1] / 100) + 1;
        int[][] levels = new int[numberOfLevel][];

        for (int i = 0; i < numberOfLevel; i++) {
            levels[i] = Array.FindAll(ids, c => (c >= (i * 100) && c < ((i+1) * 100)));
            // this as to be replaced with Array.Copy methode (better performance)
        }

        // l'arbre est contien des id, chaque 100 vaut un niveau
        int radiusCircle = 160; //(int)((80 * levels[0].Length) / Mathf.PI); // 80 = 160(size bubble) / 2 cause P=2PI*r
        //int firstCircle = radiusCircle;
        //int rawCounter = 0, gapCounter = 0;
        int defaultHeight = 89 + 59; //, minHeight = 0, rawHeight = 118, gapHeight = 59;
        //int totalOfRaw = 0;
        //foreach (int[] i in levels)
        //{
        //    totalOfRaw += Mathf.CeilToInt((float)i.Length / itemsInRaw);
        //}
        // definition du cadre avant d'y inserer les elements;
        //var sizeContainers = defaultHeight + (gapHeight * numberOfLevel) + (118 * totalOfRaw);
        //tileContainer.sizeDelta = new Vector2(0, sizeContainers);
        //brancheContainer.sizeDelta = new Vector2(0, sizeContainers);

        //Debug.Log("total of raws: " + totalOfRaw + "; total of levels: " + numberOfLevel);
        foreach (int[] i in levels) {
            Array.Sort(i);
            
            if (radiusCircle < ((80 * i.Length) / Mathf.PI + 80))
                radiusCircle = Mathf.FloorToInt((80 * i.Length) / Mathf.PI + 80);
            
            for (int j = 0; j < i.Length; j++) {
                float angle = (360f / i.Length) * j;
                //rawCounter += j != 0 && j % itemsInRaw == 0  ? 1 : 0; // add line when 1 elem in raw (exept 1st of the level)
                //minHeight = defaultHeight + gapCounter * gapHeight;
                //Debug.Log("calc l'angle " + angle.ToString() + " from mathf {" + (Mathf.Cos(angle * Mathf.Deg2Rad) * radiusCircle) + ";" + (Mathf.Sin(angle * Mathf.Deg2Rad) * radiusCircle) + "}");
                //itemsInCurrentRaw = (j >= (itemsInRaw * Mathf.FloorToInt(i.Length / itemsInRaw))) ? i.Length - (Mathf.FloorToInt(i.Length / itemsInRaw) * itemsInRaw) : itemsInRaw;
                Vector2Int position = new Vector2Int(
                    Mathf.FloorToInt(Mathf.Cos(angle * Mathf.Deg2Rad) * radiusCircle), // X
                    Mathf.FloorToInt(Mathf.Sin(angle * Mathf.Deg2Rad) * radiusCircle)); // Y
                var newTile = Instantiate(treeTile, new Vector3(position.x, position.y, 0), Quaternion.identity , tileContainer.transform).GetComponent<TreeTileScript>();
                var skill = skillTree.Data.GetSkill(i[j]);


                newTile.initiate(
                    skill.name,
                    skill.description,
                    skill.cost.ToString(),
                    skillTree,
                    i[j],
                    skillTree.CanSkillBeUnlocked(i[j]),
                    gemText,
                    this,
                    position);
                tileList.Add(newTile);
                foreach (int k in skill.dependencies) {
                    Vector2 positionRoot = tileList.Find(c => c.id == k).position;
                    Vector2 positionBeam = new Vector2((position.x + positionRoot.x) / 2, (position.y + positionRoot.y) /2);
                    float rotation = Mathf.Rad2Deg * Mathf.Atan2(positionRoot.y - position.y, positionRoot.x - position.x);//Vector2.Angle(Vector2.zero, new Vector2(positionRoot.x - position.x, positionRoot.y - position.y));
                    RectTransform beamRect = Instantiate(
                        beam, 
                        new Vector3(positionBeam.x, positionBeam.y, 1),
                        //Quaternion.FromToRotation(
                        //    new Vector3(positionRoot.x, positionRoot.y),
                        //    new Vector3(positionBeam.x, positionBeam.y)),
                        Quaternion.AngleAxis(- rotation, Vector3.back),
                        brancheContainer.transform).GetComponent<RectTransform>();
                    beamRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Vector2.Distance(positionRoot, position));
                    Debug.Log("beam of " + newTile.id + " from " + position.ToString() + " to " + positionBeam.ToString() + " with rotation: " + rotation);
                }
            }
            radiusCircle += 160;
            //gapCounter++;
            //rawCounter++;
        }
        tileContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, radiusCircle + defaultHeight);
        tileContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, radiusCircle + defaultHeight);

    }

    //public void updateTree(int idSkill)
    //{
    //    // var rootTile = tileList.Find(t => t.id == idSkill);
    //    //var dependecies = skillTree.Data.skills[idSkill].dependencies;
    //    foreach (Skill skill in skillTree.Data.skills)
    //    {
    //        int founded = Array.Find(skill.dependencies, d => d.ToString() == idSkill.ToString());
    //        if (founded != 0)
    //        {
    //            tileList.Find(t => t.id == skill.id)?.setInteractable();
    //        }
    //    }
    //}
    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
