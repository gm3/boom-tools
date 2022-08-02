using UnityEngine;
using System.Collections;

public class MaintainYHeight : MonoBehaviour {

		//what is the player

		public GameObject theTarget;

		//where is the player

		private Vector3 lastTargetPosition;

		//camera needs to move

		private float distanceToMoveX;

		// Smooth towards the height of the target

		private float cameraTargetHeight;
		public float smoothTime;
		public float triggerHeight;
		private float yVelocity;

		private float newHeight;

		void Start () {

			//what is the target
		//theTarget = FindObjectByTag("") ();

			//Initialize
			lastTargetPosition = theTarget.transform.position;

		}

		void Update () {

			//equation for camera move
			//new position minus old position

			distanceToMoveX = theTarget.transform.position.x - lastTargetPosition.x;

			transform.position = new Vector3(transform.position.x + distanceToMoveX, transform.position.y, transform.position.z);

			//move camera Y postion

			yVelocity = Camera.main.velocity.y;

			if (theTarget.transform.position.y >= (transform.position.y)+triggerHeight) {

				newHeight = Mathf.SmoothDamp(transform.position.y, transform.position.y + triggerHeight, ref yVelocity,smoothTime);
				transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);    

			}

			else if (theTarget.transform.position.y <= (transform.position.y)-triggerHeight) {

				newHeight = Mathf.SmoothDamp(transform.position.y, transform.position.y - triggerHeight, ref yVelocity,smoothTime);
				transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);    

			}

			//find target position

			lastTargetPosition = theTarget.transform.position;

		}
	}