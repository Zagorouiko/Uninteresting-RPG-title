using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dragon.Core;
using System;

namespace Dragon.Character
{
    public class SelfHealBehavior : AbilityBehavior
    {
        Player player;

        private void Start()
        {
            player = FindObjectOfType<Player>();
        }

        public override void Use(AbilityUseParams useParams)
        {
            var playerAudioSource = player.GetComponent<AudioSource>();
            playerAudioSource.clip = config.GetAudioClip();
            playerAudioSource.Play();
            PlayParticleEffect();
            player.Heal((config as SelfHealConfig).GetExtraHealth());
        }
    }
}
