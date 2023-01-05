using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class RandomAudioClipPlayer : MonoBehaviour {

	public AudioClip[] clips;

	void Start () {
		GetComponent<AudioSource>().clip = clips[Random.Range (0, clips.Length)];
		GetComponent<AudioSource>().Play();
	}

	void Update () {





	}



}