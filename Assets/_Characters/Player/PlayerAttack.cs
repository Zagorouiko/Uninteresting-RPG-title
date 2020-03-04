using UnityEngine;
using Dragon.CameraUI;
using Dragon.Weapons;
using System;

namespace Dragon.Character
{
    public class PlayerAttack : MonoBehaviour
    {       
        float baseDamage = 25f;     
        float lastHitTime = 0f;

        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        [SerializeField] Weapon weaponInUse = null;
        [SerializeField] SpecialAbility[] abilities;

        Animator animator;
        Energy energyComponent;
        AICharacterControl aiCharacterControl;
        CameraRaycaster cameraRaycaster;

        void Start()
        {
            abilities[0].AttachComponentTo(gameObject);
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

            if (Input.GetMouseButtonDown(1) && IsTargetInRange(enemyGameObject))
            {               
                AttemptSpecialAbility(0, enemy);
            }
        }   

        private void AttemptSpecialAbility(int abilityIndex, Enemy enemy)
        {
            var energyComponent = GetComponent<Energy>();
            var energyCost = abilities[abilityIndex].GetEnergyCost();
            print(energyComponent.IsEnergyAvailable(energyCost));
            if (energyComponent.IsEnergyAvailable(energyCost))
            {
                energyComponent.UseEnergyPoints(energyCost);
                var abilityParams = new AbilityUseParams(enemy, baseDamage);
                abilities[abilityIndex].Use(abilityParams);
            }
        }

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
                enemy.GetComponent<Enemy>().TakeDamage(baseDamage);
                lastHitTime = Time.time;
            }
        }
    }
}

