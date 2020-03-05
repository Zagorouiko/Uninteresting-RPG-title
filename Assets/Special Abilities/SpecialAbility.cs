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


    public abstract class SpecialAbility : ScriptableObject
    {
        [Header("Special Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab = null;
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


    }
    public interface ISpecialAbility
    {
        void Use(AbilityUseParams useParams);
    }
}

