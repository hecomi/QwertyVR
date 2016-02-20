using UnityEngine;
using System.Collections;

public class FirstKey : MonoBehaviour
{
    public KeyCode key;

	void Update()
    {
	    if (Input.GetKeyDown(key)) {
            Destroy(gameObject);
        }
	}
}
