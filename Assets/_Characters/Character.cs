using System;
using UnityEngine;
using UnityEngine.AI;
using Dragon.CameraUI;

namespace Dragon.Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class Character : MonoBehaviour
    {
        [Header("Capsule Collider Settings")]
        [SerializeField] Vector3 capsuleColliderCenter = new Vector3(0, 1.03f, 0);
        [SerializeField] float capsuleColliderRadius = 02f;
        [SerializeField] float capsuleColliderHeight = 2.03f;

        [Header("Setup Settings")]
        [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;
        

        [Header("Movement Properties")]
        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] float moveSpeedMultiplier = 2f;
        [SerializeField] float animationSpeedMultiplier = 2f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float moveThreshold = 1f;


        float turnAmount;
        float forwardAmount;
        Vector3 groundNormal;

        CapsuleCollider capsuleCollider;
        Animator animator;
        Rigidbody rigidBody;
        NavMeshAgent agent;


        void Awake()
        {
            AddRequiredComponents();
        }

        private void AddRequiredComponents()
        {
            animator = gameObject.AddComponent<Animator>();
            capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            animator.avatar = characterAvatar;
            
            capsuleCollider.center = capsuleColliderCenter;
            capsuleCollider.radius = capsuleColliderRadius;
            capsuleCollider.height = capsuleColliderHeight;
        }

        void Start()
        {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            
            rigidBody = GetComponent<Rigidbody>();

            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            animator.applyRootMotion = true;

            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updatePosition = true;
            agent.stoppingDistance = stoppingDistance;

            cameraRaycaster.onMouseOverWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void Update()
        {
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                Move(agent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }

        public void Kill()
        {

        }

        public void Move(Vector3 movement)
        {
            SetForwardAndTurn(movement);
            ApplyExtraTurnRotation();
            UpdateAnimator();
        }

        private void SetForwardAndTurn(Vector3 movement)
        {
            if (movement.magnitude > moveThreshold)
            {
                movement.Normalize();
            }

            var localMove = transform.InverseTransformDirection(movement);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            forwardAmount = localMove.z;
        }

        void UpdateAnimator()
        {
            animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = animationSpeedMultiplier;
        }

        void ApplyExtraTurnRotation()
        {
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                agent.SetDestination(destination);
            }
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                agent.SetDestination(enemy.transform.position);
            }
        }

        void OnAnimatorMove()
        {
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = (animator.deltaPosition * moveSpeedMultiplier / Time.deltaTime);

                velocity.y = rigidBody.velocity.y;
                rigidBody.velocity = velocity;
            }    
        }
    }
}