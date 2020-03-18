using System;
using UnityEngine;
using UnityEngine.AI;
using Dragon.CameraUI;

namespace Dragon.Character
{
    public class Character : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] float audioSourceSpatialBlend = 0.5f;

        [Header("Capsule Collider")]
        [SerializeField] Vector3 capsuleColliderCenter = new Vector3(0, 1.03f, 0);
        [SerializeField] float capsuleColliderRadius = 02f;
        [SerializeField] float capsuleColliderHeight = 2.03f;

        [Header("Animator")]
        [SerializeField] RuntimeAnimatorController animatorController;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] Avatar characterAvatar;
        

        [Header("Movement")]
        [SerializeField] float moveSpeedMultiplier = 2f;
        [SerializeField] float animationSpeedMultiplier = 2f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float moveThreshold = 1f;

        [Header("Nav Mesh")]
        [SerializeField] float navMeshAgentSteeringSpeed = 1.0f;
        [SerializeField] float NavMeshAgentStoppingDistance = 1.3f;

        float turnAmount;
        float forwardAmount;
        Vector3 groundNormal;

        CapsuleCollider capsuleCollider;
        Animator animator;
        Rigidbody rigidBody;
        NavMeshAgent navMeshAgent;
        bool isAlive = true;


        void Awake()
        {
            AddRequiredComponents();
        }

        private void AddRequiredComponents()
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = audioSourceSpatialBlend;

            rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

            animator = gameObject.AddComponent<Animator>();
            capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            animator.avatar = characterAvatar;
            animator.runtimeAnimatorController = animatorController;
            
            capsuleCollider.center = capsuleColliderCenter;
            capsuleCollider.radius = capsuleColliderRadius;
            capsuleCollider.height = capsuleColliderHeight;

            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            navMeshAgent.speed = navMeshAgentSteeringSpeed;
            navMeshAgent.autoBraking = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.updatePosition = true;
            navMeshAgent.stoppingDistance = NavMeshAgentStoppingDistance;
        }

        void Update()
        {
            if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance && isAlive)
            {
                Move(navMeshAgent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }

        public void Kill()
        {
            isAlive = false;
        }

        public void SetDestination(Vector3 worldPos)
        {
            navMeshAgent.destination = worldPos;
        }

        void Move(Vector3 movement)
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