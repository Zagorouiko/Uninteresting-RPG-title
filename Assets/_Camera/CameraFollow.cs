using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dragon.CameraUI
{
    public class CameraFollow : MonoBehaviour
    {
        GameObject player;
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        void LateUpdate()
        {
            transform.position = player.transform.position;
        }
    }
}

