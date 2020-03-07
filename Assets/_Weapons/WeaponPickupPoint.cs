using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dragon.Character;

namespace Dragon.Weapons
{
    [ExecuteInEditMode]
    public class WeaponPickupPoint : MonoBehaviour
    {
        [SerializeField] Weapon weaponConfig;
        [SerializeField] AudioClip pickUpSFX;

        private void Update()
        {
            if (!Application.isPlaying)
            {
                DestroyChildren();
                InstantiateWeapon();
            }          
        }

        private void DestroyChildren()
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        private void InstantiateWeapon()
        {
            var weapon = weaponConfig.GetWeaponPrefab();
            Instantiate(weapon, gameObject.transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = FindObjectOfType<Player>();
            player.GetComponent<AudioSource>().PlayOneShot(pickUpSFX);
            player.PutWeaponInHand(weaponConfig);
        }
    }
}

