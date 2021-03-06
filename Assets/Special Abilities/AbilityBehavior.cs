﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dragon.Character
{
    public abstract class AbilityBehavior : MonoBehaviour
    {
        const float PARTICLE_CLEAN_UP_DELAY = 20f;
        protected AbilityConfig config;    

        public abstract void Use (GameObject target = null);

        public void SetConfig(AbilityConfig configToSet)
        {
            config = configToSet;
        }

        protected void PlayParticleEffect()
        {
            GameObject particleObject = Instantiate(config.GetParticlesPrefab(), transform.position, Quaternion.identity);
            particleObject.transform.parent = transform;
            particleObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(DestroyParticleWhenFinished(particleObject));
        }
        IEnumerator DestroyParticleWhenFinished(GameObject particlePrefab)
        {
            while (particlePrefab.GetComponent<ParticleSystem>().isPlaying)
            {
                yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
            }
            Destroy(particlePrefab);
            yield return new WaitForEndOfFrame();
        }

        protected void PlayAbilityAnimation()
        {
            AnimatorOverrideController animatorOverrideController = GetComponent<Character>().GetAnimatorController();
            var abilityAnimation = config.GetAbilityAnimation();
            var animator = GetComponent<Animator>();

            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["Default Attack"] = abilityAnimation;
            animator.SetTrigger("New Trigger");
        }

        protected void PlayAbilitySound()
        {
            var abilitySound = config.GetRandomAbilitySound();
            var audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(abilitySound);
        }
    }
}
