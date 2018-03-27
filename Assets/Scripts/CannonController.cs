using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {

	public Transform anchor;
	public GameObject pellet;
	public Color color;
	public float thickness = 0.002f;
	public float speedFactor = 5;
	public GameObject holder;
	public GameObject pointer;
	public float disableTime = 2f;
	public float aimingTime = 3f;
	bool isActive = false;
	bool player_hit = false;
	bool aiming = false;

	private float aim_timer;
	private float fire_timer;

	Ray shootRay = new Ray ();
	RaycastHit shootHit;
	int shootableMask;
	ParticleSystem gunParticles;
	LineRenderer gunLine;
	Light gunLight;
	float effectsDisplayTime = 0.04f;

	public float range = 100f;

	// Use this for initialization
	void Start () {
		shootableMask = LayerMask.GetMask ("Shootable");
		gunParticles = GetComponent<ParticleSystem> ();
		gunLine = GetComponent<LineRenderer> ();
		gunLight = GetComponent<Light> ();

		aiming = true;
		holder = new GameObject();
		holder.transform.parent = anchor.transform;
		holder.transform.localPosition = Vector3.zero;
		holder.transform.localRotation = Quaternion.identity;

		pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
		pointer.transform.parent = holder.transform;
		pointer.transform.localScale = new Vector3(thickness, thickness, 100f);
		pointer.transform.localPosition = new Vector3(0f, 0f, 50f);
		pointer.transform.localRotation = Quaternion.identity;
		pointer.layer = 2;

		Material newMaterial = new Material(Shader.Find("Unlit/Color"));
		newMaterial.SetColor("_Color", color);
		pointer.GetComponent<MeshRenderer>().material = newMaterial;
	}
	
	// Update is called once per frame
	void Update () {
		if (aiming) {
			aim_timer += Time.deltaTime;
		} else {
			fire_timer += Time.deltaTime;
		}

		if (aim_timer > aimingTime)
		{
			Shoot ();
			pointer.SetActive (false);
		}

		if (fire_timer > disableTime)
		{
			aiming = true;
			fire_timer = 0;
			pointer.SetActive(true);
		}

		if (aim_timer > aimingTime + effectsDisplayTime) {
			DisableEffects ();
			aim_timer = 0f;
			aiming = false;
		}

		if (!isActive)
		{
			isActive = true;
			this.transform.GetChild(0).gameObject.SetActive(true);
		}

		float dist = 100f;
		Ray raycast = new Ray(anchor.transform.position, anchor.transform.forward);
		RaycastHit hit;
		bool bHit = Physics.Raycast(raycast, out hit);
		if (bHit && hit.distance < 100f)
		{
			dist = hit.distance;
		}
		// TODO: add later
		if (player_hit)
		{
			pointer.transform.localScale = new Vector3(thickness * 5f, thickness * 5f, dist);
		}
		else
		{
			pointer.transform.localScale = new Vector3(thickness, thickness, dist);
		}
		pointer.transform.localPosition = new Vector3(0f, 0f, dist/2f);
	}

	private void Fire()
	{
		GameObject ammo;
		Transform barrel = anchor;
		ammo = Instantiate(pellet, barrel.position, barrel.rotation);
		Rigidbody r = ammo.GetComponent<Rigidbody>();
		r.velocity = ammo.transform.forward * speedFactor;
	}

	private void Shoot ()
	{

		gunLight.enabled = true;
		gunParticles.Play ();

		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);

		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;

		if (Physics.Raycast (shootRay, out shootHit, range, shootableMask)) {
			React react = shootHit.collider.GetComponent<React> ();
			if (react != null)
			{
				react.TakeDamage (shootHit.point);
			}
			gunLine.SetPosition (1, shootHit.point);

		} else {
			gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
		}
	}

	private void DisableEffects ()
	{
		gunLine.enabled = false;
		gunLight.enabled = false;
		gunParticles.Stop ();
	}
}
