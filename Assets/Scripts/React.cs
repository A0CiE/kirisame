using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class React : MonoBehaviour {

	public ParticleSystem splashParticles;

	float timer;
	float effectsDisplayTime = 0.2f;
	float range = 100f;
	LineRenderer gunLine;
	bool reacting;

	void Start () {
		reacting = false;
		gunLine = GetComponent<LineRenderer> ();
	}

	void Update () {
		if (reacting)
		{
			timer += Time.deltaTime;
			if (timer > effectsDisplayTime)
			{
				timer = 0;
				gunLine.enabled = false;
				reacting = false;
			}
		}
	}

	public void TakeDamage (Vector3 hitPoint)
	{
		splashParticles.transform.position = hitPoint;
		splashParticles.transform.rotation = Quaternion.LookRotation (hitPoint);
		splashParticles.Emit (10);

		reacting = true;
		gunLine.enabled = true;
		gunLine.SetPosition (0, splashParticles.transform.position);
		gunLine.SetPosition (1, hitPoint * range);
	}
}
