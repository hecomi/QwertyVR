using UnityEngine;
using System.Collections;

public class LogoCreator : MonoBehaviour
{
    public string word = "VISUAL.IO";

    void Start()
    {
        var enemy = GetComponent<Enemy>();
        enemy.word = word;
    }

    void OnDestroy()
    {
        EnemySpawner.Activate();
        Bgm.Play();
        Wall.Activate();
    }
}
