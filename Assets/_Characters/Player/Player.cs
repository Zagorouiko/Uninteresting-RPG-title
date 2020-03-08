 using UnityEngine;
using Dragon.Core;
using Dragon.Weapons;
using System;
using System.Collections;
using Dragon.CameraUI;


namespace Dragon.Character
{
    public class Player : MonoBehaviour
    {
        float baseDamage = 25f;
        float lastHitTime = 0f;

        [SerializeField] Weapon weaponInUse;
        [SerializeField] Weapon currentWeaponConfig;     
        [SerializeField] GameObject weaponSocket;
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        [Range(.1f, 1f)]
        [SerializeField] float criticalHitChance = .1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;

        SpecialAbilities abilities;
        GameObject weaponObject;
        CameraRaycaster cameraRaycaster;
        Enemy enemy = null;
        Animator animator;

        private void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;

            //SetAttackAnimation();
            abilities = GetComponent<SpecialAbilities>();
            PutWeaponInHand(currentWeaponConfig);
        }

        private void Update()
        {
            var healthPercentage = GetComponent<HealthSystem>().healthAsPercentage;
            if (healthPercentage > Mathf.Epsilon)
            {
                ScanForAbilityDown();
            }
        }

        private void ScanForAbilityDown()
        {
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    abilities.AttemptSpecialAbility(keyIndex);
                }
            }
        }

        private void OnMouseOverEnemy(Enemy enemyToSet)
        {
            enemy = enemyToSet;
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy))
            {
                AttackTarget(enemy);
            }

            if (Input.GetMouseButtonDown(1) && IsTargetInRange(enemy))
            {
                abilities.AttemptSpecialAbility(0, enemy.gameObject);
            }
        }

        private bool IsTargetInRange(Enemy enemy)
        {
            float distanceToEnemy = (enemy.transform.position - transform.position).magnitude;
            return distanceToEnemy <= weaponInUse.GetAttackRange();
        }

        private void AttackTarget(Enemy enemy)
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
            }
            else
            {
                return (baseDamage + weaponInUse.GetAdditionalDamage());
            }
        }

        private void SetAttackAnimation()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["Default Attack"] = weaponInUse.GetAttackAnimation();
        }

        public void PutWeaponInHand(Weapon weaponToUse)
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

