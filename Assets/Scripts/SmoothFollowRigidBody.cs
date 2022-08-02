using UnityEngine;
using System.Collections;

public class SmoothFollowRigidBody : MonoBehaviour {

		public Transform target;    // The target we are following
		public float distance;      // The distance from the target along its Z axis
		public float height;        // the height we want the camera to be above the target
		public float positionDamping;   // how quickly we should get to the target position
		public float rotationDamping;   // how quickly we should get to the target rotation
		Rigidbody rBody;

		void Awake() {
			rBody = GetComponent<Rigidbody>();
		}

		// Use this for public variable initialization
		public void Reset() {
			distance = 3;
			height = 1;
			positionDamping = 6;
			rotationDamping = 60;
		}

		// LateUpdate is called once per frame
		public void FixedUpdate () {
			ensureReferencesAreIntact();
			#region Get Transform Manipulation
			// The desired position
			Vector3 targetPosition = target.position + target.up * height - target.forward * distance;
			// The desired rotation
			Quaternion targetRotation = Quaternion.LookRotation(target.position-transform.position, target.up);
			#endregion

			#region Manipulate Transform
			rBody.position = Vector3.MoveTowards(rBody.position, targetPosition, positionDamping * Time.deltaTime);
			rBody.rotation = Quaternion.RotateTowards(rBody.rotation, targetRotation, rotationDamping * Time.deltaTime);
			#endregion
		}

		// Checks to make sure all required references still exist and disables the script if not
		private void ensureReferencesAreIntact() {
			if (target == null) {
				Debug.LogError("No target is set in the SmoothFollow Script attached to " + name);
				this.enabled = false;
			}
		}
	}