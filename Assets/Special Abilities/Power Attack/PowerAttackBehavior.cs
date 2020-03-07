using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dragon.Character
{
    public class PowerAttackBehavior : AbilityBehavior
    {
        PowerAttackConfig config;
        public void SetConfig(PowerAttackConfig configToSet)
        {
            config = configToSet;
        } 

        public override void Use(AbilityUseParams useParams)
        {
            float damageToDeal = useParams.baseDamage + config.GetExtraDamage();
            useParams.target.TakeDamage(damageToDeal);
        }

        private void PlayParticleEffect()
        {
            var particlePrefab = config.GetParticlesPrefab();
            var prefab = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
            ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }
    }
}

