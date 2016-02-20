using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemySpawner : MonoBehaviour 
{
    static private EnemySpawner instance = null;

    private bool isActivated_ = false;

	public float randomRadius = 1f;
	public GameObject enemyPrefab;

	public float interval = 1f;
    public float intervalDecreaseFactor = 0.9f;
    public float nextLevelInterval = 5f;
    public float minInterval = 0.5f;
	private float t = 0f;

	private Vector3 randomPosition
	{
		get
		{
			var pos = transform.position;
			var distance = (Player.transform.position - pos).magnitude; 
			pos += Random.onUnitSphere * randomRadius;
			return (pos - Player.transform.position).normalized * distance;
		}
	}

	private string randomWord
	{
		get { return EnglishWords.words[Random.Range(0, EnglishWords.words.Length - 1)]; }
	}

    void Awake()
    {
        instance = this;
    }

    static public void Activate()
    {
        instance.isActivated_ = true;
    }

    IEnumerator Start()
    {
        for (;;) {
            if (!instance.isActivated_) yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(nextLevelInterval);
            NextLevel();
        }
    }

	void Update()
	{
        if (!instance.isActivated_) return;

		t += Time.deltaTime;
		while (t > interval) {
			t -= interval;
			Generate();
		}
	}

	void Generate()
	{
		var pos = randomPosition;
		if (enemyPrefab) {
			var obj = Stage.Add(enemyPrefab, pos, Quaternion.Euler(0f, 180f, 0f));
			var enemy = obj.GetComponent<Enemy>();
			enemy.angVelocity = new Vector3(0, 0, Random.Range(-5f, 5f));
            enemy.word = randomWord;
		}
	}

    void NextLevel()
    {
        interval = Mathf.Max(interval * intervalDecreaseFactor, minInterval);
    }
}
