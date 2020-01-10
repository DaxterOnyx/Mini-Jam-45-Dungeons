public class Player : Character
{
	public ClassData Class;

	public override float Speed { get { return base.Speed * Class.SpeedRatio; } }

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
