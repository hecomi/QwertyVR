using UnityEngine;
using System.Collections;

public class EnemyAttackEffect : MonoBehaviour
{
    public float offset = 0.2f;
    public float noise = 0.05f;

    private Renderer renderer_;
    private Transform camera_;

    void Start()
    {
        renderer_ = GetComponent<Renderer>();
        camera_ = GameObject.Find("OvrvisionProCamera").transform.FindChild("LeftCamera");
    }

	void Update()
    {
        var cameraPos = camera_.position;
        var cameraFwd = camera_.forward;
        var cameraRight = camera_.right;
        var cameraUp = camera_.up;
        var effectPos = cameraPos + cameraFwd * 0.2f;
        var effectRot = Quaternion.LookRotation(-cameraFwd) * Quaternion.Euler(-90f, 0f, 0f);
        effectPos += cameraRight * Random.Range(0f, noise) + cameraUp * Random.Range(0f, noise);

        transform.position = effectPos;
        transform.rotation = effectRot;

        Debug.Log("hoge");

        var color = renderer_.material.GetColor("_TintColor");
        color.a *= 0.9f;
        renderer_.material.SetColor("_TintColor", color);

        if (color.a < 0.001f) {
            Destroy(gameObject);
        }
	}
}
