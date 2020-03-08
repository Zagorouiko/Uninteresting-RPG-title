using UnityEngine;
using Dragon.CameraUI;
using Dragon.Weapons;
using System;
using System.Collections.Generic;

namespace Dragon.Character
{
    public class PlayerAttack : MonoBehaviour
    {       
        float baseDamage = 25f;
        float lastHitTime = 0f;

        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        [SerializeField] Weapon weaponInUse = null;
        [SerializeField] AbilityConfig[] abilities;
        [Range(.1f, 1f)] [SerializeField] float criticalHitChance = .1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;

        Enemy enemy = null;
        Animator animator;
        CameraRaycaster cameraRaycaster;

        void Start()
        {         
            AttachInitialAbilities();
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();

            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            SetAttackAnimation();
        }

        private void Update()
        {
            ScanForAbilityDown();
        }

        private void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }
        }

        private void ScanForAbilityDown()
        {
            for (int keyIndex = 1; keyIndex < abilities.Length; keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    AttemptSpecialAbility(keyIndex);
                }
            }
        }

        private void OnMouseOverEnemy(Enemy enemyToSet)
        {
            enemy = enemyToSet;
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy))
            {
                DoDamage(enemy);
            }

            if (Input.GetMouseButtonDown(1) && IsTargetInRange(enemy))
            {               
                AttemptSpecialAbility(0);
            }
        }

        private void AttemptSpecialAbility(int abilityIndex)
        {
            var energyComponent = GetComponent<Energy>();
            var energyCost = abilities[abilityIndex].GetEnergyCost();
            if (energyComponent.IsEnergyAvailable(energyCost))
            {
                energyComponent.UseEnergyPoints(energyCost);
                var abilityParams = new AbilityUseParams(enemy, baseDamage);
                abilities[abilityIndex].Use(abilityParams);
            }
        }

        private void SetAttackAnimation()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["Default Attack"] = weaponInUse.GetAttackAnimation();
        }

        private bool IsTargetInRange(Enemy enemy)
        {
            float distanceToEnemy = (enemy.transform.position - transform.position).magnitude;
            return distanceToEnemy <= weaponInUse.GetAttackRange();
        }

        private void DoDamage(Enemy enemy)
        {
            SetAttackAnimation();
            if (Time.time - lastHitTime > weaponInUse.GetminTimeBetweenHits())
            {
                animator.SetTrigger("New Trigger");
                enemy.TakeDamage(CalculateDamage());
                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage()
        {
            var randomNumber = UnityEngine.Random.Range(0, 1);
            if (randomNumber <= criticalHitChance)
            {
                return (baseDamage + weaponInUse.GetAdditionalDamage()) * criticalHitMultiplier;
            } else
            {
                return (baseDamage + weaponInUse.GetAdditionalDamage());
            }
            
        }
    }
}

