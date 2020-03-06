using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dragon.Core;
using System;

namespace Dragon.Character
{
    public class SelfHealBehavior : MonoBehaviour, ISpecialAbility
    {
        SelfHealConfig config;
        Player player;

        public void SetConfig(SelfHealConfig configToSet)
        {
            player = FindObjectOfType<Player>();
            config = configToSet;
        }

        public void Use(AbilityUseParams useParams)
        {
            var playerAudioSource = player.GetComponent<AudioSource>();
            playerAudioSource.clip = config.GetAudioClip();
            playerAudioSource.Play();
            player.Heal(config.GetExtraHealth());
        }
    }
}
