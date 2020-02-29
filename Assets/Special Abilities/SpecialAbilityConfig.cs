using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dragon.Character

{    public abstract class SpecialAbilityConfig : ScriptableObject
    {
        [Header("Special Ability General")]
        public float energyCost = 10f;

        protected ISpecialAbility behavior;

        abstract public void AddComponent(GameObject gameObjectToAttachTo);

        public void Use()
        {
            behavior.Use();
        }
    }
}

