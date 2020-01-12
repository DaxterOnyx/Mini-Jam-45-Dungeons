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
	public GameObject[] Ennemys;

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
		var max = Ennemys.Length;
		var data = Ennemys[Random.Range(min, max)];
		var ennemy = Instantiate(data,transform).GetComponent<Character>();
		ennemy.transform.position = RandomPosition();
		ennemy.Initiate();
		return ennemy;
	}

	private Vector2 RandomPosition()
	{
		return new Vector2(Random.Range(AreaToSpawn.x, AreaToSpawn.xMax), Random.Range(AreaToSpawn.yMin, AreaToSpawn.yMax));
	}

	private void Start()
	{
		//TODO POWER LEVEL BY Wave
		SpawnEnnemmiesGroup(100);
	}

	internal Character[] GetCharacters()
	{
		//TODO Set in Cache
		return GetComponentsInChildren<Character>();
	}
}
