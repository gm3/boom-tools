using UnityEngine;
using System.Collections;

public class LookAtScript : MonoBehaviour {


	private Transform targetToLookAt;
	// Use this for initialization

	void Awake() {
		//rb = GetComponent<Rigidbody>();
		targetToLookAt = GameObject.FindWithTag("LookAtTarget").transform; 
		//UpVector = GameObject.FindWithTag("UpVector").transform; 
		//OziTransform.position.y = StartY;
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	  
		Vector3 relativePos = targetToLookAt.position - transform.position;
		transform.rotation = Quaternion.LookRotation (relativePos);

		 
	}
}