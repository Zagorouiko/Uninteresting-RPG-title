using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dragon.Character
{
    public class PowerAttackBehavior : MonoBehaviour, ISpecialAbility
    {
        PowerAttackConfig config;
        public void SetConfig(PowerAttackConfig configToSet)
        {
            config = configToSet;
        } 

        public void Use(AbilityUseParams useParams)
        {
            print("Power attack used by: " + gameObject.name);
            float damageToDeal = useParams.baseDamage + config.GetExtraDamage();
            useParams.target.TakeDamage(damageToDeal);
        }
    }
}

