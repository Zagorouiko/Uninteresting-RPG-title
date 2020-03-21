using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dragon.Core;
using System;

namespace Dragon.Character
{
    public class AreaEffectBehavior : AbilityBehavior
    {      
        public override void Use(GameObject target)
        {
            DealRadialDamage(target);
            PlayAbilitySound();
            PlayAbilityAnimation();
            PlayParticleEffect();
        }      

        private void DealRadialDamage(GameObject target)
        {
            float radius = (config as AreaEffectConfig).GetRadius();
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position, //how is this the origin???
                radius,
                Vector3.up,
                radius
                );

            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<HealthSystem>();
                bool hitPlayer = hit.collider.gameObject.GetComponent<PlayerMovement>();
                if (damageable != null && !hitPlayer)
                {
                    float damageToDeal = (config as AreaEffectConfig).GetDamage();
                    damageable.TakeDamage(damageToDeal);
                }

            }
        }
    }
}

