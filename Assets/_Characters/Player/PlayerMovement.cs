using UnityEngine;
using UnityEngine.AI;
using Dragon.CameraUI;


namespace Dragon.Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    public class PlayerMovement : MonoBehaviour
    {
        CameraRaycaster cameraRaycaster = null;
        AICharacterControl aiCharacterControl = null;
        GameObject walkTarget = null;
        void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            aiCharacterControl = GetComponent<AICharacterControl>();
            walkTarget = new GameObject("walkTarget");
            cameraRaycaster.onMouseOverWalkable += Walk;
        }

        private void Walk(Vector3 destination)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                walkTarget.transform.position = destination;
                aiCharacterControl.SetTarget(walkTarget.transform);
            }              
        }
    }
}
