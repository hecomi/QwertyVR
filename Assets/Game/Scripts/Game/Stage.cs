using UnityEngine;
using System.Collections;

public class Stage : MonoBehaviour 
{
	static private Stage instance;

	void Awake()
	{
		instance = this;
	}

	static public GameObject Add(GameObject prefab, Vector3 pos, Quaternion rot)
	{
		if (!prefab) return null;
		var obj = Instantiate(prefab, pos, rot) as GameObject;
		obj.transform.parent = instance.transform;
		return obj;
	}
}
