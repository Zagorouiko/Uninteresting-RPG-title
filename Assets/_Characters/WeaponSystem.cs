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
            animatorOverrideController["Default Attack"] = currentWeaponConfig.GetAttackAnimation();
        }

        private void AttackTarget(EnemyAI enemy)
        {
            SetAttackAnimation();
            if (Time.time - lastHitTime > currentWeaponConfig.GetminTimeBetweenHits())
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
            return baseDamage + currentWeaponConfig.GetAdditionalDamage();
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
