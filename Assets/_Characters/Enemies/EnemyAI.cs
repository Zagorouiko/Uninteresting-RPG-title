using UnityEngine;
using Dragon.Core;
using Dragon.Weapons;
using System.Collections;

namespace Dragon.Character
{
    [RequireComponent(typeof(WeaponSystem))]
    [RequireComponent(typeof(Character))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 6f;

        bool isAttacking = false;
        float currentWeaponRange;    
        PlayerMovement player;
        Character character;

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
            player = FindObjectOfType<PlayerMovement>();           
        }

        private void Update()
        {
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeaponConfig().GetAttackRange();
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer > chaseRadius && state != State.patrolling)
            {
                StopAllCoroutines();
                state = State.patrolling;
            }
            if (distanceToPlayer <= chaseRadius && state != State.chasing)
            {
                StopAllCoroutines();
                StartCoroutine(ChasePlayer());
            }
            if (distanceToPlayer <= currentWeaponRange && state != State.attacking)
            {
                StopAllCoroutines();
                state = State.attacking;
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

