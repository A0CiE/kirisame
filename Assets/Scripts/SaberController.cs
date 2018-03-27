using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaberController : MonoBehaviour {

	public ParticleSystem splashParticles;

	private void OnCollisionEnter (Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			EmitAtLocation (contact);
		}

		Destroy (collision.gameObject);
		//Rigidbody r = collision.gameObject.GetComponent<Rigidbody> ();
		//r.useGravity = true;
		Debug.Log ("hit");
	}

	void EmitAtLocation(ContactPoint contact)
	{
		splashParticles.transform.position = contact.point;
		splashParticles.transform.rotation = Quaternion.LookRotation (contact.normal);
		splashParticles.Emit (10);
	}
}
