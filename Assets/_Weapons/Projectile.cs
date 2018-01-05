﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons { 

    /// <summary>
    /// Spawning projectile.
    /// </summary>
    public class Projectile : MonoBehaviour {

        [SerializeField] protected GameObject hitParticleEffect;

        protected GameObject shooter; 
        protected float damageAmount;
        protected float destroyRange;
        protected int INTERACTIVE_OBJECT_LAYER = 11;

        public void SetDamage(float damage){
		    damageAmount = damage;
	    }
        public void SetShooter(GameObject shooter){
            this.shooter = shooter;
        }
        public GameObject GetShooter()
        {
            return shooter;
        }
        public void SetDestroyRange(float range){
            destroyRange = range;
        }

	    public void OnTriggerEnter(Collider other)
	    {
            if (other.gameObject.layer == shooter.layer || other.gameObject.layer == this.gameObject.layer || other.gameObject == null)
                return; // friend fire off
        
            Component destroyable = other.GetComponent (typeof (IDamageable));
            
            if (destroyable){ // if target is destroy able
                (destroyable as IDamageable).TakeDamage (damageAmount, this.gameObject);//hit him
            }

            if (other.gameObject.layer != INTERACTIVE_OBJECT_LAYER)
                Destroy(this.gameObject);
        }

        private void Update()
        {
            if (shooter == null)
            {
                Destroy (gameObject);
                return;
            }
            //Check distance to shooter and destroy if above
            float distance =  (this.transform.position - shooter.transform.position).magnitude;
            if (distance > destroyRange)
                Destroy (gameObject);
        }

    }
}
