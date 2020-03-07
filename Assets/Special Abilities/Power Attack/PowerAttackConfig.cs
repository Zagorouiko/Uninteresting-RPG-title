using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dragon.Character
{
    [CreateAssetMenu(menuName = ("RPG/SpecialAbility/Power Attack"))]
    public class PowerAttackConfig : AbilityConfig
    {
        [Header("Power Attack specific")]
        [SerializeField] float extraDamage = 1f;
        public override AbilityBehavior GetBehaviorComponent(GameObject gameObjectToAttachTo)
        {
            return gameObjectToAttachTo.AddComponent<PowerAttackBehavior>();
        }

        public float GetExtraDamage()
        {
            return extraDamage;
        }
    }
}
