using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {
//	Approximate dimensions of the vehicle.
	public float length = 10;
	public float width = 5;
	public float height = 2;
	
//	Height above planet surface to hover.
	public float hoverHeight = 4;

//	Strength and persistence of the lifting force on the hover.
	public float springiness = 3;
	public float damping = .01f;
	
//	Forward thrust parameter. Increase for a faster vehicle.
	public float thrust = 10;
	
//	Alter y coordinate of Centre of Mass to increase hover's roll on
//	turns.
	public Vector3 centreOfMass;

//	The planet object...
	public Transform planet;
	
//	...and its gravity.
	public float gravity = 9.8f;
	
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody>().centerOfMass = centreOfMass;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	//	Gravity toward the "planet".
		Vector3 gravForce = (planet.position - transform.position).normalized * gravity;
		GetComponent<Rigidbody>().AddForce(gravForce);
		
	//	Get the four corners of the vehicle in world space.
		Vector3 frontLeft = transform.TransformPoint(-width / 2, -height / 2, length / 2);
		Vector3 frontRight = transform.TransformPoint(width / 2, -height / 2, length / 2);
		Vector3 backLeft = transform.TransformPoint(-width / 2, -height / 2, -length / 2);
		Vector3 backRight = transform.TransformPoint(width / 2, -height / 2, -length / 2);
		
	//	Vehicle's relative "up" direction.
		Vector3 relUp = transform.TransformDirection(Vector3.up);
		RaycastHit frontLeftHit;
		RaycastHit frontRightHit;
		RaycastHit backLeftHit;
		RaycastHit backRightHit;
		
	//	Measure the distance to the ground with rays.
		Physics.Raycast(frontLeft, -relUp, out frontLeftHit);
		Physics.Raycast(frontRight, -relUp, out frontRightHit);
		Physics.Raycast(backLeft, -relUp, out backLeftHit);
		Physics.Raycast(backRight, -relUp, out backRightHit);
	
	//	Get the current velocity of the corner points in the
	//	hover's up/down direction to act as damping for the
	//	springy hovering force.
		Vector3 dampVec = new Vector3(0, damping, 0);
		
		Vector3 frontLeftDamp = transform.TransformDirection(
				Vector3.Scale(transform.InverseTransformDirection(GetComponent<Rigidbody>().GetPointVelocity(frontLeft)), dampVec));
		Vector3 frontRightDamp = transform.TransformDirection(Vector3.Scale(transform.InverseTransformDirection(GetComponent<Rigidbody>().GetPointVelocity(frontRight)), dampVec));
		Vector3 backLeftDamp = transform.TransformDirection(Vector3.Scale(transform.InverseTransformDirection(GetComponent<Rigidbody>().GetPointVelocity(backLeft)), dampVec));
		Vector3 backRightDamp = transform.TransformDirection(Vector3.Scale(transform.InverseTransformDirection(GetComponent<Rigidbody>().GetPointVelocity(backLeft)), dampVec));

	//	Calculate the lift given by each corner.
		Vector3 frontLeftLift = (-relUp * gravity / 4) + (relUp * (hoverHeight - frontLeftHit.distance) * springiness) - frontLeftDamp;
		Vector3 frontRightLift = (-relUp * gravity / 4) + (relUp * (hoverHeight - frontRightHit.distance) * springiness) - frontRightDamp;
		Vector3 backLeftLift = (-relUp * gravity / 4) + (relUp * (hoverHeight - backLeftHit.distance) * springiness) - backLeftDamp;
		Vector3 backRightLift = (-relUp * gravity / 4) + (relUp * (hoverHeight - backRightHit.distance) * springiness) - backRightDamp;
		
	//	Calculate a simple forward thrust from the arrow keys.
		float lThrust, rThrust;
		
		lThrust = rThrust = thrust * Input.GetAxis("Vertical");
		
		float horizAxis = Input.GetAxis("Horizontal");
		
		Vector3 lThrustForce = transform.TransformDirection(Vector3.forward) * (lThrust + horizAxis);
		Vector3 rThrustForce = transform.TransformDirection(Vector3.forward) * (rThrust - horizAxis);

	//	Add the forces to the hover at the appropriate places. Note that
	//	the back corners have forward thrust as well as lift.
		GetComponent<Rigidbody>().AddForceAtPosition(frontLeftLift, frontLeft);
		GetComponent<Rigidbody>().AddForceAtPosition(frontRightLift, frontRight);
		GetComponent<Rigidbody>().AddForceAtPosition(backLeftLift + lThrustForce, backLeft);
		GetComponent<Rigidbody>().AddForceAtPosition(backRightLift + rThrustForce, backRight);
		
	}
}
