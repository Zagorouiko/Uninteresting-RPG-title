using UnityEngine;
using Dragon.Core;
using Dragon.Weapons;

namespace Dragon.Character
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        float currentHealthPoints = 100f;
        float maxHealthPoints = 100f;

        bool isAttacking = false;
        float damagePerShot = 10f;

        Vector3 aimOffset = new Vector3(0, 1f, 0);
        [SerializeField] float secondBetweenShots = 0.5f;
        [SerializeField] float variation = 0.1f;
        [SerializeField] float moveRadius = 7f;
        [SerializeField] float attackRadius = 5f;
        
        Player player;
        AICharacterControl aiCharacterControl;
        [SerializeField] GameObject projectile;
        [SerializeField] GameObject projectileSpawn;

        GameObject spawnedProjectile;
        Projectile projectileComponent;

        private void Start()
        {
            player = FindObjectOfType<Player>();           
            aiCharacterControl = GetComponent<AICharacterControl>();
        }

        private void Update()
        {
            if (player.healthAsPercentage <= Mathf.Epsilon)
            {
                StopAllCoroutines();
                aiCharacterControl.enabled = false;
                Destroy(this);
            }

            MoveTowardPlayer();
        }

        private void MoveTowardPlayer()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                float randomisedDelay = Random.Range(secondBetweenShots - variation, secondBetweenShots + variation);
                InvokeRepeating("FireProjectile", 0f, randomisedDelay);

            }

            if (distanceToPlayer > attackRadius && isAttacking)
            {
                CancelInvoke();
                isAttacking = false;
            }

            if (distanceToPlayer <= moveRadius)
            {
                aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                aiCharacterControl.SetTarget(transform);
            }
        }

        private void FireProjectile()
        {
            spawnedProjectile = Instantiate(projectile, projectileSpawn.transform.position, Quaternion.identity);
            projectileComponent = spawnedProjectile.GetComponent<Projectile>();
            SpawnProjectile();

            Vector3 projectileToPlayer = (player.transform.position + aimOffset - projectileSpawn.transform.position).normalized;
            Vector3 multipliedProjectileVector = projectileToPlayer * projectileComponent.projectileSpeed;
            spawnedProjectile.GetComponent<Rigidbody>().velocity = multipliedProjectileVector * projectileComponent.projectileSpeed;
        }

        private void SpawnProjectile()
        {

            projectileComponent.SetShooter(gameObject);
            projectileComponent.SetdamagedCaused(damagePerShot);
        }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if (currentHealthPoints <= 0) { Destroy(gameObject); }
        }

        public float healthAsPercentage
        {
            get { return currentHealthPoints / maxHealthPoints; }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(255f, 0f, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            Gizmos.color = new Color(0f, 0f, 255f, 1f);
            Gizmos.DrawWireSphere(transform.position, moveRadius);
        }
    }
}

