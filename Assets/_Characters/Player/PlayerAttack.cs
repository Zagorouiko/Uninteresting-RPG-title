using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dragon.CameraUI;
using Dragon.Weapons;

namespace Dragon.Character
{
    public class PlayerAttack : MonoBehaviour
    {

        CameraRaycaster cameraRaycaster = null;
        int enemyLayer = 9;       
        float damagerPerHit = 10f;     
        float lastHitTime = 0f;

        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Weapon weaponInUse;
        Animator animator;

        void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.notifyMouseClickObservers += ProcessAttackClick;
            SetupRuntimeAnimator();
        }

        private void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["Default Attack"] = weaponInUse.GetAttackAnimation();
        }

        private void ProcessAttackClick(RaycastHit raycastHit, int layerHit)
        {
            if (layerHit == enemyLayer)
            {
                GameObject enemy = raycastHit.collider.gameObject;
                if (IsTargetInRange(enemy, layerHit))
                {
                    DoDamage(enemy);
                }
            }           
        }

        private bool IsTargetInRange(GameObject enemy, int layerHit)
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

