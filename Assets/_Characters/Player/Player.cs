 using UnityEngine;
using Dragon.Core;
using Dragon.Weapons;
using System;
using System.Collections;


namespace Dragon.Character
{
    public class Player : MonoBehaviour
    {                
        [SerializeField] Weapon currentWeaponConfig;     
        [SerializeField] GameObject weaponSocket;
        
        GameObject weaponObject;          

        private void Start()
        {
            PutWeaponInHand(currentWeaponConfig);
        }

        public void PutWeaponInHand(Weapon weaponToUse)
        {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            Destroy(weaponObject);
            weaponObject = Instantiate(weaponPrefab, weaponSocket.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
        }

    }
}

