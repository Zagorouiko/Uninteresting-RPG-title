using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Dragon.Character
{
    public class HealthSystem : MonoBehaviour
    {       
        [SerializeField] Image healthBar;
        [SerializeField] AudioClip[] damageSounds;
        [SerializeField] AudioClip[] deathSounds;
        [SerializeField] float currentHealthPoints = 100f;
        [SerializeField] float maxHealthPoints = 100f;

        float regenPointsPerSecond = 5f;
        bool isCharacterDead = false;

        AudioSource audioSource;
        Animator animator;
        Character characterMovement;
        const string DEATH_TRIGGER = "New Trigger 0";

        public float healthAsPercentage { get { return currentHealthPoints / maxHealthPoints; } }

        void Start()
        {
            characterMovement = GetComponent<Character>();
            audioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
            SetCurrentMaxHealth();
        }

        void Update()
        {
            if (!isCharacterDead)
            {
                RegenHealthPoints();
            }
            UpdateHealthBar();
        }
        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        void UpdateHealthBar()
        {
            if (healthBar)
            {
                healthBar.fillAmount = healthAsPercentage;
            }
        }

        public void TakeDamage(float damage)
        {
            isCharacterDead = (currentHealthPoints - damage <= 0);
            if (isCharacterDead)
            {
                StartCoroutine(KillCharacter());
            }
            else
            {
                PlayDamageSound();
                ReduceHealth(damage);
            }
        }

        IEnumerator KillCharacter()
        {
            StopAllCoroutines();
            characterMovement.Kill();           
            animator.SetTrigger(DEATH_TRIGGER);

            var playerComponent = GetComponent<PlayerControl>();
            if (playerComponent && playerComponent.isActiveAndEnabled)
            {
                PlayDeathSound();
                yield return new WaitForSecondsRealtime(4f);
                SceneManager.LoadScene(0);
            }
            Destroy(gameObject, 3f);
        }
        private void ReduceHealth(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }

        public void Heal(float healAmount)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + healAmount, 0, maxHealthPoints);
        }

        private void RegenHealthPoints()
        {
            var pointsToAdd = regenPointsPerSecond * Time.deltaTime;
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + pointsToAdd, 0, maxHealthPoints);
        }

        public void PlayDeathSound()
        {
            var randomNumber = Random.Range(0, deathSounds.Length);
            audioSource.clip = deathSounds[randomNumber];
            audioSource.Play();
        }

        public void PlayDamageSound()
        {
            var randomNumber = Random.Range(0, damageSounds.Length);
            var clip = damageSounds[randomNumber];
            audioSource.PlayOneShot(clip);
        }
    }
}
