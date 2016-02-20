using UnityEngine;
using System.Collections;

public class Bgm : MonoBehaviour
{
    static public Bgm instance = null;
    
    void Awake()
    {
        instance = this;
    }

    static public void Play()
    {
        instance.GetComponent<AudioSource>().Play();
    }
}
