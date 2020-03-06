using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dragon.Core;

namespace Dragon.Character
{ 
    public struct AbilityUseParams
    {
        public IDamageable target;
        public float baseDamage;

        public AbilityUseParams(IDamageable target, float baseDamage)
        {
            this.target = target;
            this.baseDamage = baseDamage;
        }
    }


    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab = null;
        [SerializeField] AudioClip audioClip;

        protected ISpecialAbility behavior;

        abstract public void AttachComponentTo(GameObject gameObjectToAttachTo);

        public void Use(AbilityUseParams useParams)
        {
            behavior.Use(useParams);
        }

        public float GetEnergyCost()
        {
            return energyCost;
        }

        public GameObject GetParticlesPrefab()
        {
            return particlePrefab;
        }

        public AudioClip GetAudioClip()
        {
            return audioClip;
        }


    }
    public interface ISpecialAbility
    {
        void Use(AbilityUseParams useParams);
    }
}

