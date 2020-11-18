using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dragon.Core;

namespace Dragon.Weapons {
    public class Projectile : MonoBehaviour
    {
        float damageCaused;
        public float projectileSpeed = 3f;
        [SerializeField] GameObject shooter;

        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }

        public void SetdamagedCaused(float damage)
        {
            damageCaused = damage;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer != shooter.layer)
            {
                //DamageIfDamagables(other);
            }
        }

        //private void DamageIfDamagables(Collision other)
        //{
        //    Component damageableComponent = other.gameObject.GetComponent(typeof(IDamageable));
        //    if (damageableComponent)
        //    {
        //        (damageableComponent as IDamageable).TakeDamage(damageCaused);
        //    }
        //}

        private void OnCollisionExit(Collision other)
        {
            Destroy(gameObject);
        }
    }
}

