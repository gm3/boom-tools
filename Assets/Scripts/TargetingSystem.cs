using UnityEngine;
using System.Collections;

public class TargetingSystem : MonoBehaviour {

	public GameObject target;
	public GameObject ozi;
	// Use this for initialization
	void Start () {

		ozi = GameObject.FindGameObjectWithTag("OZI_SWIVAL");
		target = GameObject.FindGameObjectWithTag("OZI_PLAYER_TARGET");

	}

	// Update is called once per frame
	void Update () {

		Vector3 relativePos = target.transform.position - ozi.transform.position;
		ozi.transform.rotation = Quaternion.LookRotation (relativePos);


	}
}
