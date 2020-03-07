using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dragon.Character
{
    [CreateAssetMenu(menuName = ("RPG/SpecialAbility/Self Heal"))]
    public class SelfHealConfig : AbilityConfig
    {
        [Header("Self Heal specific")]
        [SerializeField] float extraHealth = 25f;

        public override AbilityBehavior GetBehaviorComponent(GameObject gameObjectToAttachTo)
        {
            return gameObjectToAttachTo.AddComponent<SelfHealBehavior>();
        }

        public float GetExtraHealth()
        {
            return extraHealth;
        }
    }
}
