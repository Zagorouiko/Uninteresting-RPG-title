using UnityEngine;
using Dragon.CameraUI;
using Dragon.Weapons;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Dragon.Character
{
    public class SpecialAbilities : MonoBehaviour
    {               
        [SerializeField] AbilityConfig[] abilities;
        [SerializeField] Image energyOrb;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 5f;

        public float currentEnergyPoints;
        float energyAsPercentage { get { return currentEnergyPoints / maxEnergyPoints; } }                    

        void Start()
        {
            SetCurrentMaxEnergy();
            AttachInitialAbilities();
            UpdateEnergyBar();        
        }

        void Update()
        {
            if (currentEnergyPoints < maxEnergyPoints)
            {
                AddEnergyPoints();
                UpdateEnergyBar();
            }                   
        }

        void AttachInitialAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }
        }

        public int GetNumberOfAbilities()
        {
            return abilities.Length;
        }

        public void AttemptSpecialAbility(int abilityIndex)
        {
            var energyCost = abilities[abilityIndex].GetEnergyCost();
            if (energyCost <= currentEnergyPoints)
            {
                UseEnergyPoints(energyCost);
                //var abilityParams = new AbilityUseParams(enemy, baseDamage);
                //abilities[abilityIndex].Use(abilityParams);
            } else
            {
                // out of energy sound
            }
        }

        private void AddEnergyPoints()
        {
            var pointsToAdd = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);
        }

        private void SetCurrentMaxEnergy()
        {
            currentEnergyPoints = maxEnergyPoints;
        }

        private void UpdateEnergyBar()
        {
            energyOrb.fillAmount = energyAsPercentage;
        }

        public void UseEnergyPoints(float pointsPerHit)
        {
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - pointsPerHit, 0f, maxEnergyPoints);
            UpdateEnergyBar();
        }
    }
}

