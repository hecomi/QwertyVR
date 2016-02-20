using UnityEngine;
using System.Collections;

public class AutoDestroyParticle : MonoBehaviour 
{
	private float lifeTime_ = 0;

	void Start() 
	{
		var particle = GetComponent<ParticleSystem>();
		lifeTime_ = particle.startLifetime + particle.duration;
		Destroy(gameObject, lifeTime_);
	}
}
