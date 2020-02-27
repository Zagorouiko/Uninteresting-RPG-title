using UnityEngine;
using Dragon.Core;
using Dragon.Weapons;

namespace Dragon.Character
{
    public class Player : MonoBehaviour, IDamageable
    {
        float currentHealthPoints = 100f;
        float maxHealthPoints = 100f;

        [SerializeField] Weapon weaponInUse;     
        [SerializeField] GameObject weaponSocket;

        private void Start()
        {
            SetCurrentMaxHealth();
            PutWeaponInHand();            
        }

        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        private void PutWeaponInHand()
        {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            var weapon = Instantiate(weaponPrefab, weaponSocket.transform);
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
        }

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            // if (currentHealthPoints <= 0) { Destroy(gameObject); }
        }
    }
}

