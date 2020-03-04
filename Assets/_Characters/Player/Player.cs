 using UnityEngine;
using Dragon.Core;
using Dragon.Weapons;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Dragon.Character
{
    public class Player : MonoBehaviour, IDamageable
    {
        float currentHealthPoints = 100f;
        float maxHealthPoints = 100f;

        [SerializeField] Weapon weaponInUse;     
        [SerializeField] GameObject weaponSocket;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;

        AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
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
            bool playerDies = (currentHealthPoints - damage <= 0);
            if (playerDies) {
                ReduceHealth(damage);
                PlayDeathSound();
                StartCoroutine(KillPlayer());
            } else
            {
                PlayDamageSound();
                ReduceHealth(damage);
            }                    
        }

        public void PlayDamageSound()
        {
            var randomNumber = Random.Range(0, damageSounds.Length);
            audioSource.clip = damageSounds[randomNumber];
            audioSource.Play();
        }

        public void PlayDeathSound()
        {
            var randomNumber = Random.Range(0, deathSounds.Length);
            audioSource.clip = deathSounds[randomNumber];
            audioSource.Play();
        }

        IEnumerator KillPlayer()
        {
            yield return new WaitForSecondsRealtime(2f);
            SceneManager.LoadScene(0);
        }

        private void ReduceHealth(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }
    }
}

