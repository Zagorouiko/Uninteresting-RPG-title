﻿using System.Collections;
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
        [SerializeField] AudioClip[] audioClips= null;

        protected AbilityBehavior behavior;

        public void AttachAbilityTo(GameObject gameObjectToAttachTo)
        {
            AbilityBehavior behaviorComponent = GetBehaviorComponent(gameObjectToAttachTo);
            behaviorComponent.SetConfig(this);
            behavior = behaviorComponent;
        }

        public abstract AbilityBehavior GetBehaviorComponent(GameObject gameObjectToAttachTo);
        
       
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

        public AudioClip GetRandomAbilitySound()
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }
    }
}

