using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dragon.Weapons;
using System;

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
            animator = GetComponent<Animator>();
            SetAttackAnimation();
            PutWeaponInHand(currentWeaponConfig);
        }

        private void Update()
        {
            bool targetIsDead;
            bool targetIsOutOfRange;
            if (target == null)
            {
                targetIsDead = false;
                targetIsOutOfRange = false;
            }

            else
            {
                var targetHealth = target.GetComponent<HealthSystem>().healthAsPercentage;
                targetIsDead = targetHealth <= Mathf.Epsilon;

                var distance = Vector3.Distance(transform.position, target.transform.position) <= currentWeaponConfig.GetAttackRange();
                targetIsOutOfRange = distance;             
            }

            float characterHealth = GetComponent<HealthSystem>().healthAsPercentage;
            bool characterIsDead = (characterHealth <= Mathf.Epsilon);

            if (characterIsDead || targetIsOutOfRange || targetIsDead)
            {
                StopAllCoroutines();
            }
        }

        public WeaponConfig GetCurrentWeaponConfig()
        {
            return currentWeaponConfig;
        }

        private void SetAttackAnimation()
        {
            if (!character.GetAnimatorController())
            {
                Debug.Break();
                Debug.LogAssertion("Please provide " + gameObject + " with an animator override controller.");
            } else
            {
                var animatorOverrideController = character.GetAnimatorController();
                animator.runtimeAnimatorController = animatorOverrideController;
                animatorOverrideController["Default Attack"] = currentWeaponConfig.GetAttackAnimation();
            }            
        }

        private void AttackTarget(EnemyAI enemy)
        {
            SetAttackAnimation();
            
        }
        public void StopAttacking()
        {
            StopAllCoroutines();
        }

        public void AttackTarget(GameObject targetToAttack)
        {
            target = targetToAttack;
            //StartCoroutine(AttackTargetRepeatedly());
        }

        IEnumerator AttackTargetRepeatedly() //crashing here
        {
            bool attackerStillAlive = GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            bool targetStillAlive = target.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;

            while (attackerStillAlive && targetStillAlive)
            {
                
                if (Time.time - lastHitTime > currentWeaponConfig.GetminTimeBetweenHits())
                {
                    float weaponHitPeriod = currentWeaponConfig.GetminTimeBetweenHits();
                    float timeToWait = weaponHitPeriod * character.GetAnimSpeedMultiplier();

                    bool isTimeToHitAgain = Time.time - lastHitTime > timeToWait;

                    if (isTimeToHitAgain)
                    {
                        AttackTargetOnce();
                        lastHitTime = Time.time;
                    }                  
                    yield return new WaitForSeconds(timeToWait);
                }
            }
        }

        private void AttackTargetOnce()
        {
            transform.LookAt(target.transform);
            
            animator.SetTrigger("New Trigger");
            SetAttackAnimation();

            float damageDelay = 1f;
            StartCoroutine(DamageAfterDelay(damageDelay));          
        }

        IEnumerator DamageAfterDelay(float damageDelay)
        {
            yield return new WaitForSeconds(damageDelay);
            var enemyHealth = target.GetComponent<HealthSystem>();
            enemyHealth.TakeDamage(CalculateDamage());        
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
