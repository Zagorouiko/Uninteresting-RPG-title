using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dragon.Core;
using System;

namespace Dragon.Character
{
    public class SelfHealBehavior : AbilityBehavior
    {
        PlayerControl player;

        private void Start()
        {
            player = GetComponent<PlayerControl>();
        }

        public override void Use(GameObject target)
        {
            PlayAbilitySound();

            var playerHealth = player.GetComponent<HealthSystem>();          
            playerHealth.Heal((config as SelfHealConfig).GetExtraHealth());
            PlayParticleEffect();
            PlayAbilityAnimation();
        }
    }
}
