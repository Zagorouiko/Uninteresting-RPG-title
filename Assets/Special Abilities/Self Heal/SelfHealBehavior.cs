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
            PlayAbilitySound();
            PlayParticleEffect();
            player.Heal((config as SelfHealConfig).GetExtraHealth());
        }
    }
}
