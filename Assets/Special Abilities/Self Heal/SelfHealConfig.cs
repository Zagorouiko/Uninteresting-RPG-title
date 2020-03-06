using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dragon.Character
{
    [CreateAssetMenu(menuName = ("RPG/SpecialAbility/Self Heal"))]
    public class SelfHealConfig : SpecialAbility
    {
        [Header("Self Heal specific")]
        [SerializeField] float extraHealth = 25f;

        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviorComponent = gameObjectToAttachTo.AddComponent<SelfHealBehavior>();
            behaviorComponent.SetConfig(this);
            behavior = behaviorComponent;
        }

        public float GetExtraHealth()
        {
            return extraHealth;
        }
    }
}
