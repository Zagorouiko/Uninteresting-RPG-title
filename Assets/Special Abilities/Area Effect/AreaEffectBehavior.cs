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
        ParticleSystem myParticleSystem;


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
            myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);                 
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
                bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
                if (damageable != null && !hitPlayer)
                {
                    float damageToDeal = useParams.baseDamage + config.GetDamage();
                    damageable.TakeDamage(damageToDeal);
                }

            }
        }
    }
}

