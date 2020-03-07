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

        public override AbilityBehavior GetBehaviorComponent(GameObject gameObjectToAttachTo)
        {            
            return gameObjectToAttachTo.AddComponent<AreaEffectBehavior>();
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

