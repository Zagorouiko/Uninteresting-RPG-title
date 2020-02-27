using System;
using UnityEngine;
using Dragon.Character;
using UnityEngine.AI;
using Dragon.CameraUI;


namespace Dragon.Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {
        ThirdPersonCharacter thirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
        CameraRaycaster cameraRaycaster = null;
        AICharacterControl aiCharacterControl = null;
        GameObject walkTarget = null;

        // TODO solve fight between serialize and const
        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] const int enemyLayerNumber = 9;

        void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
            aiCharacterControl = GetComponent<AICharacterControl>();
            walkTarget = new GameObject("walkTarget");

            //cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick;
            cameraRaycaster.onMouseOverWalkable += Walk;
        }

        private void Walk(Vector3 destination)
        {
            walkTarget.transform.position = destination;
            aiCharacterControl.SetTarget(walkTarget.transform);
        }

        //void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
        //{
        //    switch (layerHit)
        //    {
        //        case enemyLayerNumber:
        //            // navigate to the enemy
        //            GameObject enemy = raycastHit.collider.gameObject;
        //            aiCharacterControl.SetTarget(enemy.transform);
        //            break;
        //        case walkableLayerNumber:
        //            // navigate to point on the ground              
        //            walkTarget.transform.position = raycastHit.point;
        //            aiCharacterControl.SetTarget(walkTarget.transform);
        //            break;
        //        default:
        //            Debug.LogWarning("Don't know how to handle mouse click for player movement");
        //            return;
        //    }
        //}

        //TODO make this get called again
        void ProcessDirectMovement()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // calculate camera relative direction to move:
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

            thirdPersonCharacter.Move(movement, false, false);
        }
    }
}
