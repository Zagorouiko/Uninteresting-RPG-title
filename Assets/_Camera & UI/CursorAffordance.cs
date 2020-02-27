using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dragon.CameraUI
{
    public class CursorAffordance : MonoBehaviour
    {
        [SerializeField] Texture2D attackCursor = null;
        
        [SerializeField] Texture2D errorCursor = null;

        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);
        CameraRaycaster cameraRaycaster;

        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] const int enemyLayerNumber = 9;


        void Start()
        {
            cameraRaycaster = GetComponent<CameraRaycaster>();
            cameraRaycaster.notifyLayerChangeObservers += OnLayerChange;
        }

        void OnLayerChange(int layerhit)
        {
            switch (layerhit)
            {
                case enemyLayerNumber:
                    Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.Auto);
                    break;
                case 10:
                    Cursor.SetCursor(errorCursor, cursorHotspot, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(errorCursor, cursorHotspot, CursorMode.Auto);
                    return;
            }
        }
    }
}

