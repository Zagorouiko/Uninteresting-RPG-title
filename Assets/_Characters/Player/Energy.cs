using UnityEngine;
using UnityEngine.UI;

namespace Dragon.Character
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] RawImage energyBar;
        [SerializeField] float maxEnergyPoints = 100f;

        float currentEnergyPoints;
        float energyAsPercentage { get { return currentEnergyPoints / maxEnergyPoints; } }

        void Start()
        {
            SetCurrentMaxEnergy();
        }

        private void SetCurrentMaxEnergy()
        {
            currentEnergyPoints = maxEnergyPoints;
        }

        public bool isEnergyAvailable(float energyAmount)
        {
            return energyAmount <= currentEnergyPoints;
        }

        private void UpdateEnergyBar()
        {
            float xValue = -(energyAsPercentage / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        public void UseEnergyPoints(float pointsPerHit)
        {
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - pointsPerHit, 0f, maxEnergyPoints);
            UpdateEnergyBar();
        }          
    }
}

