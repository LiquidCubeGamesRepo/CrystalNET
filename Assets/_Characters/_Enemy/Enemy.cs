﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable {

    [HideInInspector] public Animator animatorControler;

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] GameObject blood;

    public float healthAsPercentage { get { return currentHealthPoints / (float)maxHealthPoints; } }
    public bool isDestroyed;

    public delegate void OnEnemyDeath();
    public static event OnEnemyDeath onEnemyDeath;
    
    float currentHealthPoints;
    float stiffDestroyTime = 20f;

    void Start()
	{
		currentHealthPoints = maxHealthPoints;
        animatorControler = GetComponent<Animator> ();
    }

	public void TakeDamage(float damage)
	{
        if (isDestroyed) return;

		currentHealthPoints = Mathf.Clamp (currentHealthPoints - damage, 0f, maxHealthPoints);

        var offset = this.GetComponent<Collider> ().bounds.extents.y;
        GameObject blood1 = Instantiate (blood, transform.position + new Vector3(0,offset), blood.transform.rotation) as GameObject;
        Destroy (blood1, 5f);

        if (currentHealthPoints == 0)
        {
            isDestroyed = true;
            animatorControler.SetTrigger ("Death");
            GetComponent<Collider> ().enabled = false;

            onEnemyDeath ();
			Destroy (gameObject);
        }
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
    }
}
