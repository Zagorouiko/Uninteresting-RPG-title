using UnityEngine;
using Dragon.Core;
using Dragon.Weapons;

namespace Dragon.Character
{
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {
        bool isAttacking = false;
        float currentWeaponRange;
        
        PlayerMovement player;


        private void Start()
        {
            player = FindObjectOfType<PlayerMovement>();           
        }

        private void Update()
        {
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrentWeaponConfig().GetAttackRange();
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(255f, 0f, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);
        }

        public void TakeDamage(float damage)
        {

        }
    }
}

