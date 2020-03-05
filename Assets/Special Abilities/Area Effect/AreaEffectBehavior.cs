using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dragon.Core;
using System;

namespace Dragon.Character
{
    public class AreaEffectBehavior : MonoBehaviour, ISpecialAbility
    {
        AreaEffectConfig config;


        public void SetConfig(AreaEffectConfig configToSet)
        {
            config = configToSet;
        }

        public void Use(AbilityUseParams useParams)
        {
            DealRadialDamage(useParams);
            PlayParticleEffect();
        }

        private void PlayParticleEffect()
        {
            GameObject prefab = Instantiate(config.GetParticlesPrefab(), transform.position, Quaternion.identity);
            var particleSystem = prefab.GetComponent<ParticleSystem>();
            particleSystem.Play();
            Destroy(prefab, particleSystem.main.duration);                 
        }

        private void DealRadialDamage(AbilityUseParams useParams)
        {
            float radius = config.GetRadius();
            RaycastHit[] hits = Physics.SphereCastAll(
                transform.position, //how is this the origin???
                radius,
                Vector3.up,
                radius
                );

            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    float damageToDeal = useParams.baseDamage + config.GetDamage();
                    damageable.TakeDamage(damageToDeal);
                }

            }
        }
    }
}

