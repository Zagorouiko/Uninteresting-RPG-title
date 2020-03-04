using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dragon.Character
{
    [RequireComponent(typeof(Image))]
    public class PlayerHealthBar : MonoBehaviour
    {

        Image healthImage;
        Player player;

        void Start()
        {
            player = FindObjectOfType<Player>();
            healthImage = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            healthImage.fillAmount = player.healthAsPercentage;
        }
    }
}

