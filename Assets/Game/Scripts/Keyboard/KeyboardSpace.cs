using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyboardSpace : MonoBehaviour
{
    public int consume = 10;
    public GameObject bulletPrefab;
	
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            int n = 0;
            foreach (var enemy in EnemyManager.GetAllEnemies()) {
                for (int i = 0; i < enemy.word.Length; ++i) {
                    if (Score.GetScore() <= 0 || Score.GetScore() < consume * n) break;
                    var alphabet = enemy.OnAttack(enemy.word[i].ToString());
                    if (alphabet) {
                        AddBullet(alphabet, n++ * 0.05f);
                    }
                }
            }
            if (n > 0) {
                Score.AddScore(-consume * n);
                KeyboardReturn.Emit();
            }
        }
    }

    void AddBullet(Alphabet alphabet, float delay)
    {
        StartCoroutine(_AddBullet(alphabet, delay));
    }

    IEnumerator _AddBullet(Alphabet alphabet, float delay)
    {
        yield return new WaitForSeconds(delay);
        var obj = Stage.Add(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
        var bullet = obj.GetComponent<Bullet>();
        bullet.target = alphabet.transform;
    }
}