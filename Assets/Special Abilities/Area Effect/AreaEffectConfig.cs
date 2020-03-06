using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dragon.Character
{
    [CreateAssetMenu(menuName = ("RPG/SpecialAbility/Area Effect"))]
    public class AreaEffectConfig : AbilityConfig
    {
        [Header("Area Effect specific")]
        [SerializeField] float Radius = 5f;
        [SerializeField] float DamageToEachTarget = 15f;

        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviorComponent = gameObjectToAttachTo.AddComponent<AreaEffectBehavior>();
            behaviorComponent.SetConfig(this);
            behavior = behaviorComponent;
        }

        public float GetRadius()
        {
            return Radius;
        }

        public float GetDamage()
        {
            return DamageToEachTarget;
        }
    }
}

