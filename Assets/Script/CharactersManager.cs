using UnityEngine;

public class CharactersManager : MonoBehaviour
{
	private static CharactersManager _instance;

	public static CharactersManager Instance
	{
		get {
			if (_instance == null)
				_instance = FindObjectOfType<CharactersManager>();
			return _instance;
		}
	}

	public int MaxCharactersNb = 100;
	public Rect AreaToSpawn = new Rect(0, 0, 10, 10);
	public GameObject EnnemyPrefab;
	public CharacterData[] EnnemysData;

	public Character[] SpawnEnnemmiesGroup(int PowerValue)
	{
		var nb = Random.Range(1, Mathf.Min(MaxCharactersNb,PowerValue));
		var characters = new Character[nb];

		for (int i = 0; i < nb; i++) {
			characters[i] = SpawnEnnemy(PowerValue / nb);
				}

		return characters;
	}

	private Character SpawnEnnemy(float v)
	{
		//TODO SELECT GOOD POWER VALUE
		var min = 0;
		var max = EnnemysData.Length;
		var data = EnnemysData[Random.Range(min, max)];
		var ennemy = Instantiate(EnnemyPrefab,transform).GetComponent<Character>();
		ennemy.transform.position = RandomPosition();
		ennemy.Initiate(data);
		return ennemy;
	}

	private Vector2 RandomPosition()
	{
		return new Vector2(Random.Range(AreaToSpawn.x, AreaToSpawn.xMax), Random.Range(AreaToSpawn.yMin, AreaToSpawn.yMax));
	}

	private void Start()
	{
		SpawnEnnemmiesGroup(100);
	}
}
