using UnityEngine;

namespace Dragon.Character
{
	public class ThirdPersonCharacter : MonoBehaviour
	{
		[SerializeField] float movingTurnSpeed = 360;
		[SerializeField] float stationaryTurnSpeed = 180;
		[SerializeField] float moveThreshold = 1f;

		Rigidbody rigidBody;
		Animator animator;
		float turnAmount;
		float forwardAmount;
		Vector3 groundNormal;

		void Start()
		{
			animator = GetComponent<Animator>();
			rigidBody = GetComponent<Rigidbody>();
			rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			animator.applyRootMotion = true;
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
		}

		void ApplyExtraTurnRotation()
		{
			float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
			transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
		}
	}
}