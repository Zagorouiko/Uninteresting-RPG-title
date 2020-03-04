using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dragon.Character
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] Image energyOrb;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 5f;

        public float currentEnergyPoints;
        float energyAsPercentage { get { return currentEnergyPoints / maxEnergyPoints; } }

        void Start()
        {
            SetCurrentMaxEnergy();
        }

        private void Update()
        {
           if (currentEnergyPoints < maxEnergyPoints)
            {
                AddEnergyPoints();
                UpdateEnergyBar();
            }
        }

        private void AddEnergyPoints()
        {
            var pointsToAdd = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0 ,maxEnergyPoints);
        }

        private void SetCurrentMaxEnergy()
        {
            currentEnergyPoints = maxEnergyPoints;
        }

        public bool IsEnergyAvailable(float energyAmount)
        {
            return energyAmount <= currentEnergyPoints;
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

