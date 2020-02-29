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
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Use()
        {

        }
    }
}

