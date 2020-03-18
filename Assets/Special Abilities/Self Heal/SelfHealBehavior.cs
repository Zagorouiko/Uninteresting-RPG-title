﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dragon.Core;
using System;

namespace Dragon.Character
{
    public class SelfHealBehavior : AbilityBehavior
    {
        PlayerMovement player;

        private void Start()
        {
            player = GetComponent<PlayerMovement>();
        }

        public override void Use(GameObject target)
        {
            PlayAbilitySound();

            var playerHealth = player.GetComponent<HealthSystem>();          
            playerHealth.Heal((config as SelfHealConfig).GetExtraHealth());
            PlayParticleEffect();
        }
    }
}
