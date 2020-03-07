using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dragon.Core;
using System;

namespace Dragon.Character
{
    public class AreaEffectBehavior : AbilityBehavior
    {      
        public override void Use(AbilityUseParams useParams)
        {
            DealRadialDamage(useParams);
            PlayAbilitySound();
            PlayParticleEffect();
        }      

        private void DealRadialDamage(AbilityUseParams useParams)
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
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
                if (damageable != null && !hitPlayer)
                {
                    float damageToDeal = useParams.baseDamage + (config as AreaEffectConfig).GetDamage();
                    damageable.TakeDamage(damageToDeal);
                }

            }
        }
    }
}

