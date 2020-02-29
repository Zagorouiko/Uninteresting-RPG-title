using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dragon.Character
{
    [CreateAssetMenu(menuName = ("RPG/SpecialAbility/Power Attack"))]
    public class PowerAttackConfig : SpecialAbilityConfig
    {
        [Header("Power Attack specific")]
        [SerializeField] float extraDamage = 10f;
        public override ISpecialAbility AddComponent(GameObject gameObjectToAttachTo)
        {
            var behaviorComponent = gameObjectToAttachTo.AddComponent<PowerAttackBehavior>();
            behaviorComponent.SetConfig(this);
            return behaviorComponent;
        }

    }
}
