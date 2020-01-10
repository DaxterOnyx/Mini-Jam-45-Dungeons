using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public CharacterData Data;
	private Character Target;

	public float Speed { get { return Data.SpeedAmount; }  }

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	public void DefineTarget(Character character)
	{
		Target = character;
		//TODO FOLLOW CHARACTER MOVEMEMENT
		var dis = Vector2.Distance(transform.position,character.transform.position);
		transform.DOMove(character.transform.position,dis/Speed) ;
	}

}
