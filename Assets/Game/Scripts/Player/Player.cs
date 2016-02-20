using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	static private Player instance;

	public static new Transform transform
	{
		get { return instance.gameObject.transform; }
	}

    public int life = 3;

	void Awake()
	{
		instance = this;
	}

    void Update()
    {
        if (life <= 0) {
            
        }
    }
}
