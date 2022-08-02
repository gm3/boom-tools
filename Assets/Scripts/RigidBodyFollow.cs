using UnityEngine;
using System.Collections;

public class RigidBodyFollow : MonoBehaviour {
	public Transform target;
	public float speed = 1F;
	public Vector3 targetMoveTowards;
	public Rigidbody rb;
	public float thrust = 1F;
	public float revthrust = -1F;
	public Transform LookAttarget;
	public Transform UpVector; 
	public float moveSpeed = 20F; 
	public float rotationSpeed = 5F; 
	public Transform OziTransform; 
	public float MaxDist = 10F;
	public float MinDist = 5F; 
	public float MinY = 1F; 
	public float MaxY = 5; 
	public float CurrentY; 
	public float StartY = 5F; 
	public float yOffset;
	public float amplitude;
	public float frequency;


	void Awake() {
		rb = GetComponent<Rigidbody>();
		LookAttarget = GameObject.FindWithTag("LookAtTarget").transform; 
		UpVector = GameObject.FindWithTag("UpVector").transform; 
		//OziTransform.transform.position = new Vector3(0,StartY,0);
	}


	void FixedUpdate() {
		


		if(Vector3.Distance(OziTransform.position,LookAttarget.position) >= MinDist)
		{
			OziTransform.rotation = Quaternion.Slerp(OziTransform.rotation,
				Quaternion.LookRotation(LookAttarget.position - OziTransform.position), rotationSpeed*Time.deltaTime); 
			//OziTransform.rotation.z += OziTransform.rotation.z+90;
			//OziTransform.position += OziTransform.forward * moveSpeed * Time.deltaTime;
			rb.AddForce(transform.forward * thrust);
		}


	}
}