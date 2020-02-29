using UnityEngine;
using Dragon.CameraUI;
using Dragon.Weapons;
using System;

namespace Dragon.Character
{
    public class PlayerAttack : MonoBehaviour
    {       
        float damagerPerHit = 10f;     
        float lastHitTime = 0f;

        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        [SerializeField] Weapon weaponInUse = null;
        //[SerializeField] SpecialAbilityConfig[] abilities;

        Animator animator;
        Energy energyComponent;
        AICharacterControl aiCharacterControl;
        CameraRaycaster cameraRaycaster;

        void Start()
        {
            //abilities[0].AddComponent(gameObject);
            energyComponent = GetComponent<Energy>();           
            aiCharacterControl = GetComponent<AICharacterControl>();
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();

            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            SetupRuntimeAnimator();
        }

        private void OnMouseOverEnemy(Enemy enemy)
        {
            var enemyGameObject = enemy.gameObject;
            if (Input.GetMouseButton(0) && IsTargetInRange(enemyGameObject))
            {
                aiCharacterControl.SetTarget(enemy.transform);
                DoDamage(enemyGameObject);
            }

            //if (Input.GetMouseButtonDown(1) && IsTargetInRange(enemyGameObject))
            //{
            //    AttemptSpecialAbility(0, enemy);
            //}
        }

        //private void AttemptSpecialAbility(int abilityIndex, Enemy enemy)
        //{
        //    if (energyComponent.isEnergyAvailable(abilities[abilityIndex].energyCost))
        //    {
        //        abilities[abilityIndex].Use();
        //        energyComponent.UseEnergyPoints(abilities.energyCost);
        //        var powerAttackConfig = (PowerAttackConfig)abilities;
        //        float extraDamage = powerAttackConfig.extraDamage + damagerPerHit;
        //    }
        //}

        private void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["Default Attack"] = weaponInUse.GetAttackAnimation();
        }

        private bool IsTargetInRange(GameObject enemy)
        {
            float distanceToEnemy = (enemy.transform.position - transform.position).magnitude;
            return distanceToEnemy <= weaponInUse.GetAttackRange();
        }

        private void DoDamage(GameObject enemy)
        {
            if (Time.time - lastHitTime > weaponInUse.GetminTimeBetweenHits())
            {
                animator.SetTrigger("New Trigger");
                enemy.GetComponent<Enemy>().TakeDamage(damagerPerHit);
                lastHitTime = Time.time;
            }
        }
    }
}

