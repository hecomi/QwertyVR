using UnityEngine;
using System.Collections.Generic;

public class Keyboard : MonoBehaviour
{
    static private Keyboard instance;

    private Dictionary<KeyCode, KeyboardKey> keys_ = new Dictionary<KeyCode, KeyboardKey>();
    public float keyDownDistance = 0.01f;
    public GameObject pressEffectPrefab;
    public GameObject alphabetEffectPrefab;
    public GameObject enemyDetectParticlePrefab;
    public GameObject enteredKeyParticlePrefab;
    public GameObject bulletPrefab;
    public GameObject keyAssistArrowPrefab;

    void Awake()
    {
        instance = this;
    }

	void Start()
    {
        for (var i = 0; i < transform.childCount; ++i) {
            var line = transform.GetChild(i);
            for (var j = 0; j < line.childCount; ++j) {
                var key = line.GetChild(j);
                if (key.name.IndexOf("_not_used_") == 0) continue;
                var code = KeyCode.None;
                try {
                    code = (KeyCode)System.Enum.Parse(typeof(KeyCode), key.name);
                } catch (System.Exception e) {
                    Debug.LogError(e);
                    continue;
                }
                var component = key.gameObject.AddComponent<KeyboardKey>();
                component.key = code;
                component.downDistance = keyDownDistance;
                component.pressEffectPrefab = pressEffectPrefab;
                component.alphabetEffectPrefab = alphabetEffectPrefab;
                component.enemyDetectParticlePrefab = enemyDetectParticlePrefab;
                component.enteredKeyParticlePrefab = enteredKeyParticlePrefab;
                component.bulletPrefab = bulletPrefab;
                component.keyAssistArrowPrefab = keyAssistArrowPrefab;
                keys_.Add(code, component);
            }
        }
	}

    KeyboardKey _GetKey(KeyCode key)
    {
        if (keys_.ContainsKey(key)) {
            return keys_[key];
        }
        return null;
    }

    static public KeyboardKey GetKey(KeyCode key)
    {
        return instance._GetKey(key);
    }

    static public Dictionary<KeyCode, KeyboardKey> GetAllKeys()
    {
        return instance.keys_;
    }
}
