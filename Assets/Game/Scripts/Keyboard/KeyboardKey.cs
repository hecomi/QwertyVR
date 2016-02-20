using UnityEngine;
using System.Collections.Generic;

public class KeyboardKey : MonoBehaviour
{
    public KeyCode key;
    public float downDistance = 0.03f;
    public GameObject pressEffectPrefab;
    public GameObject alphabetEffectPrefab;
    public GameObject enemyDetectParticlePrefab;
    public GameObject enteredKeyParticlePrefab;
    public GameObject keyAssistArrowPrefab;

    private Vector3 initialPos_ = Vector3.zero;
    private bool isPressing_ = false;
    private GameObject enemyDetectParticle;
    private GameObject enteredKeyParticle;
    private GameObject keyAssistArrow;

    private List<GameObject> bullets_ = new List<GameObject>();
    public GameObject bulletPrefab;

    void Start()
    {
        initialPos_ = transform.localPosition;
    }

	void Update()
    {
        if (Input.GetKey(key)) {
            if (!isPressing_) {
                isPressing_ = true;
                GenerateEffect();
            }
            transform.localPosition = initialPos_ + Vector3.down * downDistance;
        } else {
            isPressing_ = false;
            transform.localPosition = initialPos_;
        }

        if (!enemyDetectParticle) {
            enemyDetectParticle = Instantiate(enemyDetectParticlePrefab) as GameObject;
            enemyDetectParticle.transform.parent = transform;
            enemyDetectParticle.transform.localPosition = Vector3.zero;
            enemyDetectParticle.transform.localRotation = enemyDetectParticle.transform.rotation; 
        }

        var enemy = InputManager.GetCurrentLockingEnemy();
        enemyDetectParticle.SetActive(enemy && enemy.word.Contains(this.name));

        if (!keyAssistArrow) {
            keyAssistArrow = Instantiate(keyAssistArrowPrefab) as GameObject;
            keyAssistArrow.transform.parent = transform;
            keyAssistArrow.transform.localPosition = keyAssistArrow.transform.position;
        }

        keyAssistArrow.SetActive(enemy && enemy.nextAlphabet == key.ToString());
        if (key == KeyCode.Return && enemy && enemy.isAllLocked) {
            keyAssistArrow.SetActive(true);
        }

        if (!enteredKeyParticle) {
            enteredKeyParticle = Instantiate(enteredKeyParticlePrefab) as GameObject;
            enteredKeyParticle.transform.parent = transform;
            enteredKeyParticle.transform.localPosition = Vector3.zero;
            enteredKeyParticle.transform.localRotation = enteredKeyParticle.transform.rotation; 
        }

        enteredKeyParticle.SetActive(enemy && enemy.word.Contains(this.name) && enemy.isAllLocked);
	}

    void GenerateEffect()
    {
        GeneratePressEffect();
    }

    void GeneratePressEffect()
    {
        var effect = Instantiate<GameObject>(pressEffectPrefab);
        effect.transform.parent = transform;
        effect.transform.localPosition = Vector3.zero;
        effect.transform.localRotation = effect.transform.rotation;
        effect.transform.localScale = Vector3.one;

        var renderer = effect.GetComponent<ParticleSystemRenderer>();
        renderer.mesh = GetComponent<MeshFilter>().sharedMesh;
    }

    void GenerateAlphabetEffect()
    {
        /*
        var alphabet = Resources.Load("Alphabets/" + name) as GameObject;
        if (!alphabet) return;
        */

        var effect = Instantiate<GameObject>(alphabetEffectPrefab);
        effect.transform.parent = transform;
        effect.transform.localPosition = Vector3.zero;
        effect.transform.localRotation = effect.transform.rotation;
        effect.transform.localScale = Vector3.one;

        /*
        var renderer = effect.GetComponent<ParticleSystemRenderer>();
        renderer.mesh = alphabet.GetComponent<MeshFilter>().sharedMesh;
        */
    }

    public Bullet QueryBullet(Enemy enemy)
    {
        enemy.LockOn();
        var alphabet = enemy.OnAttack(key.ToString());

        if (alphabet) {
            Sound.Play("press1", transform.position);
            var obj = Stage.Add(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
            bullets_.Add(obj);
            obj.transform.parent = transform;
            var bullet = obj.GetComponent<Bullet>();
            bullet.target = alphabet.transform;
            obj.SetActive(false);
            return bullet;
        } else {
            Sound.Play("press5", transform.position);
        }

        return null;
    }

    public void Emit()
    {
        foreach (var bullet in bullets_) {
            if (bullet) bullet.transform.parent = null;
            bullet.gameObject.SetActive(true);
            GeneratePressEffect();
            GenerateAlphabetEffect();
        }
        bullets_.Clear();
    }
}
