using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dragon.Weapons;

namespace Dragon.Character
{
    public class WeaponSystem : MonoBehaviour
    {
        float lastHitTime = 0f;

        [SerializeField] float baseDamage = 25f;
        [SerializeField] WeaponConfig currentWeaponConfig;
        [SerializeField] WeaponConfig weaponInUse;
        [SerializeField] GameObject weaponSocket;

        [Range(.1f, 1f)]
        [SerializeField] float criticalHitChance = .1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;

        GameObject weaponObject;
        GameObject target;
        Animator animator;
        Character character;

        void Start()
        {
            character = GetComponent<Character>();
            SetAttackAnimation();
            PutWeaponInHand(currentWeaponConfig);
        }

        public WeaponConfig GetCurrentWeaponConfig()
        {
            return currentWeaponConfig;
        }

        private void SetAttackAnimation()
        {
            var animatorOverrideController = character.GetAnimatorController();
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["Default Attack"] = weaponInUse.GetAttackAnimation();
        }

        private void AttackTarget(EnemyAI enemy)
        {
            SetAttackAnimation();
            if (Time.time - lastHitTime > weaponInUse.GetminTimeBetweenHits())
            {
                animator.SetTrigger("New Trigger");
                enemy.TakeDamage(CalculateDamage());
                lastHitTime = Time.time;
            }
        }

        public void AttackTarget(GameObject targetToAttack)
        {
            target = targetToAttack;
        }

        private float CalculateDamage()
        {
            var randomNumber = Random.Range(0, 1);
            if (randomNumber <= criticalHitChance)
            {
                return (baseDamage + weaponInUse.GetAdditionalDamage()) * criticalHitMultiplier;
            }
            else
            {
                return (baseDamage + weaponInUse.GetAdditionalDamage());
            }
        }

        public void PutWeaponInHand(WeaponConfig weaponToUse)
        {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            Destroy(weaponObject);
            weaponObject = Instantiate(weaponPrefab, weaponSocket.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
        }
    }

}
