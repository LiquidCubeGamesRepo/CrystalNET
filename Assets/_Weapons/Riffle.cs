﻿using UnityEngine;

namespace Weapons { 

    /// <summary>
    /// Riffle weapon.
    /// </summary>
    public class Riffle : Weapon
    {
        public override void Shoot()
        {
            AddDispersion ();

            MuzzleEffect ();

            GameObject bulet = Instantiate (Bullet, gunEndPoint.position, Quaternion.identity) as GameObject;
            var proj = bulet.GetComponent<Projectile> ();
            proj.SetDamage (Damage);
            proj.SetDestroyRange (Range);
            proj.SetShooter (this.gameObject);
            proj.GetComponent<Rigidbody> ().velocity = gunEndPoint.forward * BulletSpeed;

        }

        private void AddDispersion()
        {
            //ADD Dispersion
            gunEndPoint.localRotation = Quaternion.identity; //reset transform
            int n = UnityEngine.Random.Range (-Dispersion, Dispersion); //random dispersion for weapon
            gunEndPoint.RotateAround (gunEndPoint.position, gunEndPoint.up, n);
        }

        private void MuzzleEffect()
        {
            GameObject muzzle2 = Instantiate (Muzzle, gunEndPoint) as GameObject;
            Destroy (muzzle2, 0.15f);
        }
    }
}
