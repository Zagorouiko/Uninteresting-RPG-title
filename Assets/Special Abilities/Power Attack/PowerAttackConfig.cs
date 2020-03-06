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
        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviorComponent = gameObjectToAttachTo.AddComponent<PowerAttackBehavior>();
            behaviorComponent.SetConfig(this);
            behavior = behaviorComponent;
        }

        public float GetExtraDamage()
        {
            return extraDamage;
        }
    }
}
