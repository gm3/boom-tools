using System;
using System.Collections;
using UnityEngine;

public class LerpExample : MonoBehaviour {
	float lerpTime = 1f;
	float currentLerpTime;

	float moveDistance = 10f;

	Vector3 startPos;
	Vector3 endPos;

	protected void Start() {
		startPos = transform.position;
		endPos = transform.position + transform.up * moveDistance;
	}

	protected void Update() {
		//reset when we press spacebar
		if (Input.GetKeyDown(KeyCode.Space)) {
			currentLerpTime = 0f;
		}

		//increment timer once per frame
		currentLerpTime += Time.deltaTime;
		if (currentLerpTime > lerpTime) {
			currentLerpTime = lerpTime;
		}

		//lerp!
		float perc = currentLerpTime / lerpTime;
		transform.position = Vector3.Lerp(startPos, endPos, perc);
	}
}