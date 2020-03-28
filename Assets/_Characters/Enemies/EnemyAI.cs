using UnityEngine;
using Dragon.Core;
using Dragon.Weapons;
using System.Collections;
using System;

namespace Dragon.Character
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(Character))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] WaypointContainer patrolPath;

        [SerializeField] int nextWaypointIndex;
        float waypointTolerance = 2f;

        bool isAttacking = false;
        float currentWeaponRange;    
        PlayerControl player;
        Character character;
        WeaponSystem weaponSystem;

        float distanceToPlayer;

        enum State
        {
            idle,
            patrolling,
            attacking, 
            chasing
        }

        State state = State.idle;


        private void Start()
        {
            character = GetComponent<Character>();
            player = FindObjectOfType<PlayerControl>();
            weaponSystem = GetComponent<WeaponSystem>();
        }

        private void Update()
        {
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeaponConfig().GetAttackRange();
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer > chaseRadius && state != State.patrolling)
            {
                StopAllCoroutines();
                weaponSystem.StopAttacking();
                StartCoroutine(Patrol());
            }
            if (distanceToPlayer <= chaseRadius && state != State.chasing)
            {
                StopAllCoroutines();
                weaponSystem.StopAttacking();
                StartCoroutine(ChasePlayer());
            }
            if (distanceToPlayer <= currentWeaponRange && state != State.attacking)
            {
                StopAllCoroutines();
                StartCoroutine(AttackPlayer());                
            }
        }

        IEnumerator AttackPlayer()
        {
            state = State.attacking;
            var weaponConfig = weaponSystem.GetCurrentWeaponConfig();
            weaponSystem.AttackTarget(player.gameObject);
            yield return new WaitForSeconds(weaponConfig.GetminTimeBetweenHits());
        }

        IEnumerator Patrol()
        {
            state = State.patrolling;
            while (patrolPath != null)
            {
                Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWaypointPos);
                CycleWaypointWhenClose(nextWaypointPos);
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void CycleWaypointWhenClose(Vector3 nextWaypointPos)
        {
            if (Vector3.Distance(transform.position, nextWaypointPos) <= waypointTolerance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
            }                                             
        }

        IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while (distanceToPlayer >= currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(255f, 0f, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            Gizmos.color = new Color(255f, 0f, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }

        public void TakeDamage(float damage)
        {

        }
    }
}

