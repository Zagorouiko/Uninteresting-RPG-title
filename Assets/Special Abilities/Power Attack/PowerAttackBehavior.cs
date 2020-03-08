using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dragon.Character
{
    public class PowerAttackBehavior : AbilityBehavior
    {
        public override void Use(GameObject target)
        {
            PlayAbilitySound();
            float damageToDeal = (config as PowerAttackConfig).GetExtraDamage();

            var targetHealth = target.GetComponent<HealthSystem>();
            targetHealth.TakeDamage(damageToDeal);
            PlayParticleEffect();
        }        
    }
}

