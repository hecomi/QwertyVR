using UnityEngine;

public class KeyboardReturn : MonoBehaviour
{
    private static KeyboardReturn instance;
    private bool isReady_ = false;
    public GameObject returnEffect;

	void Awake()
    {
        instance = this;
	}
	
    void Update()
    {
        if (isReady_ && Input.GetKeyDown(KeyCode.Return)) {
            Emit();
        }
        returnEffect.SetActive(isReady_);
    }

    public static void Emit()
    {
        instance._Emit();
        Wall.Brighten();
    }

    void _Emit()
    {
        isReady_ = false;
        foreach (var pair in Keyboard.GetAllKeys()) {
            pair.Value.Emit();
        }
        Sound.Play("return", transform.position);
        Sound.Play("return", transform.position);
        Sound.Play("return", transform.position);
        Sound.Play("return", transform.position);
    }

    static public void OnReady()
    {
        instance.isReady_ = true;
    }
}