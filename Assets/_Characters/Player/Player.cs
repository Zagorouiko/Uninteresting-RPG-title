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
        public float currentHealthPoints = 100f;
        float maxHealthPoints = 100f;
        float regenPointsPerSecond = 5f;
        bool isPlayerDead;

        [SerializeField] Weapon currentWeaponConfig;     
        [SerializeField] GameObject weaponSocket;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;

        GameObject weaponObject;
        const string DEATH_TRIGGER = "New Trigger 0";
        AudioSource audioSource;
        Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            SetCurrentMaxHealth();
            PutWeaponInHand(currentWeaponConfig);
        }
        private void Update()
        {
            if (!isPlayerDead)
            {
                RegenHealthPoints();
            }               
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

        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        public void TakeDamage(float damage)
        {
            isPlayerDead = (currentHealthPoints - damage <= 0);
            if (isPlayerDead) {
                ReduceHealth(damage);              
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
            PlayDeathSound();
            animator.SetTrigger(DEATH_TRIGGER);
            yield return new WaitForSecondsRealtime(4f);

            SceneManager.LoadScene(0);
        }

        public void Heal(float healAmount)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + healAmount, 0, maxHealthPoints);
        }

        private void ReduceHealth(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }

        private void RegenHealthPoints()
        {
            var pointsToAdd = regenPointsPerSecond * Time.deltaTime;
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + pointsToAdd, 0, maxHealthPoints);
        }
    }
}

