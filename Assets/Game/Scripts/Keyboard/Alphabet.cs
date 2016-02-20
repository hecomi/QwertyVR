using UnityEngine;
using System.Collections.Generic;

public class Alphabet : MonoBehaviour 
{
	static private Dictionary<string, Mesh> alphabets = new Dictionary<string, Mesh>();
	private Mesh GetMesh(string alphabet)
	{
		if (!alphabets.ContainsKey(alphabet)) {
			var obj = Resources.Load<GameObject>("Alphabets/" + alphabet);
            if (!obj) return null;
			alphabets.Add(alphabet, obj.GetComponent<MeshFilter>().sharedMesh);
		}
		return alphabets[alphabet];
	}

	private string alphabet_ = "A";
	public string alphabet 
	{
		get { return alphabet_; }
		set 
		{
			alphabet_ = value;
			GetComponent<MeshFilter>().mesh = GetMesh(value);
		}
	}

	public Material lockOnMaterial;

    public void LockOn()
    {
        GetComponent<Renderer>().material = lockOnMaterial;
    }
}
