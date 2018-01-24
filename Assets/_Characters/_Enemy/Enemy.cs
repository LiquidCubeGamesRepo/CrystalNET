﻿using Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    /// <summary>
    /// Control enemy health
    /// </summary>
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] float maxHealthPoints = 100f; // max enemy hit points
        [SerializeField] GameObject blood; // blood effect
        [SerializeField] GameObject explosion; // death effect 
        [SerializeField] SoundMenager audioClips; // audio clips

        public float healthAsPercentage { get { return currentHealthPoints / (float)maxHealthPoints; } }
        public bool isDestroyed; 

        public delegate void OnEnemyDeath();
        public static event OnEnemyDeath onEnemyDeath;

        float currentHealthPoints; // current hit points

        float bloodDestroyTime = 5f;
        float explosionDestroyTime = 1f;

        void Start()
        {
            currentHealthPoints = maxHealthPoints;
        }

        public void TakeDamage(float damage, GameObject bullet)
        {
            if (isDestroyed) return; // return if its alredy destroyed

            //clamp health between 0 and maxHealthPoints
            currentHealthPoints = Mathf.Clamp (currentHealthPoints - damage, 0f, maxHealthPoints); 

            if (currentHealthPoints == 0) // if health is equeal zero....
            {
                //Spawn Blood Particle
                var offset = this.GetComponent<Collider>().bounds.extents.y; // calculate half offset of self
                blood.transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
                GameObject blood1 = Instantiate(blood);
                var particles = blood1.GetComponent<ParticleSystem>().main;
                particles.maxParticles = (int)(maxHealthPoints/5);
                Destroy(blood1, bloodDestroyTime); // and destroy it with delay

                //spawn death particle
                GameObject ex = Instantiate (explosion, transform.position + new Vector3 (0, offset), explosion.transform.rotation);
                Destroy (ex, explosionDestroyTime); // and destroy it with delay

                isDestroyed = true; //enable flag

                // play sound
                if (AudioMenager.Instance != null)
                    AudioMenager.Instance.PlayClip (audioClips.GetHitClip ());

                if(onEnemyDeath != null) onEnemyDeath (); //notify enemy death
                Destroy (gameObject); //and destroy this gameobject
            }
        }

        public bool IsDestroyed()
        {
            return isDestroyed;
        }
    }
}
