 using UnityEngine;
using Dragon.Core;
using Dragon.Weapons;
using System;
using System.Collections;
using Dragon.CameraUI;


namespace Dragon.Character
{
    public class PlayerMovement : MonoBehaviour
    {
        SpecialAbilities abilities;
        CameraRaycaster cameraRaycaster;
        EnemyAI enemy = null;
        Character character;
        WeaponSystem weaponSystem;

        private void Start()
        {
            weaponSystem = GetComponent<WeaponSystem>();
            character = GetComponent<Character>();
            RegisterForMouseEvents();         
            abilities = GetComponent<SpecialAbilities>();
            
        }

        private void RegisterForMouseEvents()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverWalkable += OnMouseOverWalkable;
        }

        private void OnMouseOverWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                character.SetDestination(destination);
            }
        }

        private void OnMouseOverEnemy(EnemyAI enemyToSet)
        {
            enemy = enemyToSet;
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy))
            {
                weaponSystem.AttackTarget(enemy.gameObject);
            }

            if (Input.GetMouseButtonDown(1) && IsTargetInRange(enemy))
            {
                abilities.AttemptSpecialAbility(0, enemy.gameObject);
            }
        }

        private void Update()
        {
             ScanForAbilityDown();
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

        private bool IsTargetInRange(EnemyAI enemy)
        {
            float distanceToEnemy = (enemy.transform.position - transform.position).magnitude;
            return distanceToEnemy <= weaponSystem.GetCurrentWeaponConfig().GetAttackRange();
        }
    }
}

