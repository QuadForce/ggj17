﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ImpactReceiver : MonoBehaviour
{
    float mass = 3.0F; // defines the character mass
    Vector3 impact = Vector3.zero;
    private CharacterController character;
	private FirstPersonController fpc;

    public float BulletForce = 200f;
    public float BulletMax = 100f;
    public float BulletDetectionRadius = 1.5f;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<CharacterController>();
		fpc = GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        // apply the impact force:
        if (impact.magnitude > 0.2F) character.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
        Collider[] colliders = Physics.OverlapSphere(character.transform.position, character.radius * BulletDetectionRadius);
        for(int i=0; i<colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Bullet")
            {
                Vector3 dir = colliders[i].transform.position - character.transform.position;
                float force = Mathf.Clamp(BulletForce, 0, BulletMax);
                AddImpact(dir, force);
				fpc.PlayCustomAudio("hit");
            }
            else
            {
                ImpactReceiver script = colliders[i].transform.GetComponent<ImpactReceiver>();
                if (script)
                {
                    Vector3 dir = colliders[i].transform.position - character.transform.position;
                    float force = Mathf.Clamp(60f, 0, 20);
                    script.AddImpact(dir, force);
                }
            }
        }
    }

    // call this function to add an impact force:
    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
    }
}
