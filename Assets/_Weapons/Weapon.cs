﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dragon.Weapons
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class Weapon : ScriptableObject
    {
        public Transform gripTransform;
        [SerializeField] float minTimeBetweenHits;
        [SerializeField] float attackRange;
        [SerializeField] float additionalDamage;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;

        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }

        public AnimationClip GetAttackAnimation()
        {
            RemoveAnimationEvents();
            return attackAnimation;
        }

        public float GetminTimeBetweenHits()
        {
            return minTimeBetweenHits;
        }

        public float GetAttackRange()
        {
            return attackRange;
        }

        public float GetAdditionalDamage()
        {
            return additionalDamage;
        }

        private void RemoveAnimationEvents()
        {
            attackAnimation.events = new AnimationEvent[0];
        }
    }
}

