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

        private void OnMouseOverEnemy(EnemyAI enemy)
        {
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy))
            {
                weaponSystem.AttackTarget(enemy.gameObject);

            } else if (Input.GetMouseButton(0) && !IsTargetInRange(enemy)) {

                StartCoroutine(MoveAndAttack(enemy));
            }
            else if (Input.GetMouseButtonDown(1) && IsTargetInRange(enemy))
            {
                abilities.AttemptSpecialAbility(0, enemy.gameObject);
            }

            else if (Input.GetMouseButton(1) && !IsTargetInRange(enemy))
            {
                StartCoroutine(MoveAndPowerAttack(enemy));
            }
        }

        IEnumerator MoveToTarget(EnemyAI enemy)
        {
            character.SetDestination(enemy.transform.position);
            while (!IsTargetInRange(enemy))
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }

        IEnumerator MoveAndPowerAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy));
            abilities.AttemptSpecialAbility(0, enemy.gameObject);
        }

        IEnumerator MoveAndAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy));
            weaponSystem.AttackTarget(enemy.gameObject);
        }

        private bool IsTargetInRange(EnemyAI enemy)
        {
            float distanceToEnemy = (enemy.transform.position - transform.position).magnitude;
            return distanceToEnemy <= weaponSystem.GetCurrentWeaponConfig().GetAttackRange();
        }
    }
}

