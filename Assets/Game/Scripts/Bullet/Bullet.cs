using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	public Transform target;

	private float factor_;
	private Vector3 velocity_;
	private Vector3 scale_;

	public Vector3 initialVelocity = new Vector3(0, 1, 0);
	public float factorStep = 0.005f;
	public float maxSpeed = 2f;

	public GameObject destructionEffectPrefab;

	private Vector3 angVelocity_;

	void Start() 
	{
		velocity_ = initialVelocity;
		angVelocity_ = (Vector3.one * 2 + Random.onUnitSphere) * 360;
		scale_ = transform.localScale;
		transform.localScale = Vector3.zero;
        Sound.Play("emit", transform.position);
	}

	void Update()
	{
		transform.rotation *= Quaternion.Euler(angVelocity_ * Time.deltaTime);
		if (!target) Destroy(gameObject);
	}
	
	void LateUpdate() 
	{
		if (!target) return;

		factor_ = Mathf.Min(factor_ + factorStep, 1f);
		var to = target.position - transform.position;
		var targetVelocity = to.normalized * maxSpeed;
		velocity_ += (targetVelocity - velocity_) * factor_;
		transform.localScale += (scale_ - transform.localScale) * factor_;

		transform.position += velocity_ * Time.deltaTime;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform != target) return;
		Destroy(collision.gameObject);
		if (destructionEffectPrefab) {
			Stage.Add(destructionEffectPrefab, collision.transform.position, collision.transform.rotation);
		}

		var renderer = GetComponent<Renderer>();
		renderer.enabled = false;

		float time = 0f;
		var trail = GetComponent<TrailRenderer>();
		if (trail) time = trail.time;
		Destroy(gameObject, time);

        Sound.Play("hit", transform.position);
	}
}
