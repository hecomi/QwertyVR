using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{
    static private InputManager instance;
    private Enemy currentLockingEnemy_;
	private KeyCode[] keys_;
    private bool isAttacking_ = false;
    private float startTime_ = 0;

    void Awake()
    {
        instance = this;
    }

	void Start() 
	{
		keys_ = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
			.Select(x => (KeyCode)System.Enum.Parse(typeof(KeyCode), x.ToString()))
			.ToArray();
	}
	
	void Update() 
	{
        if (currentLockingEnemy_ && isAttacking_) {
            AttackCurrentEnemy();
        } else {
            AttackNewEnemy();
        }

        if (Input.anyKeyDown) {
            Sound.Play("press2", transform.position);
        }
	}

    void AttackCurrentEnemy()
    {
		foreach (var key in keys_) {
			if (Input.GetKeyDown(key)) {
                QueryBullet(currentLockingEnemy_, key);
                if (currentLockingEnemy_.isAllLocked) {
                    int score = (currentLockingEnemy_.word.Length == 1) ? 30 :
                        (int)(currentLockingEnemy_.word.Length / (Time.time - startTime_) * currentLockingEnemy_.word.Length);
                    currentLockingEnemy_.score = score;
                    KeyboardReturn.OnReady();
                    isAttacking_ = false;
                }
			}
		}
    }

    void AttackNewEnemy()
    {
		foreach (var key in keys_) {
			if (Input.GetKeyDown(key)) {
				var enemy = EnemyManager.GetNonLockedNearestEnemyWithFirstAlphabet(key.ToString());
				if (enemy) {
                    currentLockingEnemy_ = enemy;
                    startTime_ = Time.time;
                    isAttacking_ = true;
                    AttackCurrentEnemy();
				}
			}
		}
    }

    void QueryBullet(Enemy enemy, KeyCode key)
    {
        Keyboard.GetKey(key).QueryBullet(enemy);
    }

    static public Enemy GetCurrentLockingEnemy()
    {
        return instance.currentLockingEnemy_;
    }
}
