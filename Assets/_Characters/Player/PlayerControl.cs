 using UnityEngine;
using Dragon.Core;
using Dragon.Weapons;
using System;
using System.Collections;
using Dragon.CameraUI;


namespace Dragon.Character
{
    public class PlayerControl : MonoBehaviour
    {
        SpecialAbilities abilities;
        EnemyAI enemy;
        Character character;
        WeaponSystem weaponSystem;

        private void Start()
        {
            weaponSystem = GetComponent<WeaponSystem>();
            character = GetComponent<Character>();
            RegisterForMouseEvents();         
            abilities = GetComponent<SpecialAbilities>();
            
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

        private void RegisterForMouseEvents()
        {
            var cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverWalkable += OnMouseOverWalkable;
        }

        public void OnMouseOverWalkable(Vector3 destination)
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

            } else if (Input.GetMouseButton(0) && !IsTargetInRange(enemy)) {

                MoveAndAttack(enemy);
                StartCoroutine(MoveAndAttack(enemy));
            }
            else if (Input.GetMouseButtonDown(1) && IsTargetInRange(enemy))
            {
                abilities.AttemptSpecialAbility(0, enemy.gameObject);
            }

            else if (Input.GetMouseButton(0) && !IsTargetInRange(enemy))
            {
                StartCoroutine(MoveAndPowerAttack(enemy));
            }
        }

        IEnumerator MoveAndPowerAttack(EnemyAI enemy)
        {
            character.SetDestination(enemy.transform.position);
            abilities.AttemptSpecialAbility(0, enemy.gameObject);
            yield return new WaitForSeconds(1f);
        }

        IEnumerator MoveAndAttack(EnemyAI enemy)
        {
            character.SetDestination(enemy.transform.position);
            while (!IsTargetInRange)
            {
                yield return new WaitForEndFrame();
            }
          
        }

        private bool IsTargetInRange(EnemyAI enemy)
        {
            float distanceToEnemy = (enemy.transform.position - transform.position).magnitude;
            return distanceToEnemy <= weaponSystem.GetCurrentWeaponConfig().GetAttackRange();
        }
    }
}

