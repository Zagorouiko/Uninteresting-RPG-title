using UnityEngine;
using UnityEngine.UI;
using Dragon.CameraUI;

namespace Dragon.Character
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] RawImage energyBar;
        [SerializeField] float maxEnergyPoints = 100f;

        float pointsPerHit = 10f;
        float currentEnergyPoints;
        float energyAsPercentage { get { return currentEnergyPoints / maxEnergyPoints; } }

        CameraRaycaster cameraRaycaster;

        void Start()
        {
            SetCurrentMaxEnergy();
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        private void OnMouseOverEnemy(Enemy enemy)
        {
            if(Input.GetMouseButtonDown(1)) {
                UseEnergyPoints();
                UpdateEnergyBar();
            }       
        }
        private void UpdateEnergyBar()
        {
            float xValue = -(energyAsPercentage / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        private void SetCurrentMaxEnergy()
        {
            currentEnergyPoints = maxEnergyPoints;
        }

        private void UseEnergyPoints()
        {           
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - pointsPerHit, 0f, maxEnergyPoints);
        }
    }
}

