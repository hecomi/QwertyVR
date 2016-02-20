using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour 
{
	static private EnemyManager instance;
	private List<Enemy> enemies_ = new List<Enemy>();

	void Awake()
	{
		instance = this;
	}

	static public void Add(Enemy enemy)
	{
		instance._Add(enemy);
	}

	static public void Remove(Enemy enemy)
	{
		instance._Remove(enemy);
	}

	static public Enemy GetNonLockedNearestEnemyWithFirstAlphabet(string alphabet)
	{
		return instance._GetNonLockedNearestEnemyWithFirstAlphabet(alphabet);
	}

	void _Add(Enemy enemy) 
	{
        enemies_.Add(enemy);
	}

	void _Remove(Enemy enemy)
	{
        enemies_.Remove(enemy);
	}

    static public List<Enemy> GetAllEnemies()
    {
        return instance.enemies_;
    }

	Enemy _GetNonLockedNearestEnemyWithFirstAlphabet(string alphabet)
	{
		Enemy target = null;
		float minDistance = float.MaxValue;
		foreach (var enemy in enemies_) {
            if (enemy.isLockedOn || enemy.word[0].ToString() != alphabet) continue;
			var distance = (transform.position - enemy.transform.position).magnitude; 
			if (distance < minDistance) {
				minDistance = distance;
				target = enemy;
			}
		}

		return target;
	}
}
