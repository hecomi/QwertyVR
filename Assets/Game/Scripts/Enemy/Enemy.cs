using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    private List<Alphabet> alphabets = new List<Alphabet>();
    [SerializeField]
    private string word_ = "HOGE";
    public string word
    {
        get { return word_; }
        set
        {
            word_ = value;
            DestroyAllChildren();
            CreateChildren();
        }
    }

    public string nextAlphabet
    {
        get
        {
            var inputLen = inputWord_.Length;
            if (inputLen >= word.Length) return "";
            return word[inputLen].ToString();
        }
    }

    private string inputWord_ = "";

    public bool isLockedOn = false;

    private bool isAllLocked_ = false;
    public bool isAllLocked
    {
        get { return isAllLocked_; }
        private set { isAllLocked_ = value; }
    }

    public GameObject alphabetPrefab;
    public float margin = 0.25f;

	public float speed = 1f;
	public Vector3 angVelocity = Vector3.zero;

    public GameObject attackParticlePrefab;
    public GameObject hitEffectPrefab;

    public int score { get; set; }

    void Start()
    {
        EnemyManager.Add(this);
    }

    void OnDestroy()
    {
        EnemyManager.Remove(this);
    }

	void Update() 
	{
		var to = Player.transform.position - transform.position;
		var velocity = to.normalized * speed;
		transform.position += velocity * Time.deltaTime;
		transform.rotation *= Quaternion.Euler(angVelocity * Time.deltaTime);

		if (to.magnitude < 0.5f)
		{
            Instantiate(attackParticlePrefab, transform.position, transform.rotation);
            Instantiate(hitEffectPrefab, transform.position, transform.rotation);
            Sound.Play("enemyAttack", transform.position);
			Destroy(gameObject);
		}

        if (transform.childCount == 0) {
            Destroy(gameObject);
            Score.AddScore(score);
        }
	}

    void DestroyAllChildren()
    {
        inputWord_ = "";
        alphabets.Clear();
        for (int i = 0; i < transform.childCount; ++i) {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void CreateChildren()
    {
        var n = word_.Length;
        for (int i = 0; i < n; ++i) {
            var go = Instantiate(alphabetPrefab);
            go.transform.parent = transform;
            go.transform.localPosition = Vector3.left * (i - n / 2 + 0.5f) * margin;
            go.transform.localRotation = Quaternion.identity;
            var alphabet = go.GetComponent<Alphabet>();
            alphabet.alphabet = word[i].ToString();
            alphabets.Add(alphabet);
        }
    }

    public void LockOn()
    {
        isLockedOn = true;
    }

    public Alphabet OnAttack(string input)
    {
        if (isAllLocked) return null;

        var inputLen = inputWord_.Length;
        var target = word[inputLen].ToString();

        if (input != target) return null;

        inputWord_ += input;
        var alphabet = alphabets[inputLen];
        alphabet.LockOn();

        if (inputWord_.Length >= word.Length) {
            isAllLocked = true;
        }

        return alphabet;
    }
}
