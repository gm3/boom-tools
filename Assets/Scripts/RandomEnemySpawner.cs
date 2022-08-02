using UnityEngine;
using System.Collections;

public class RandomEnemySpawner : MonoBehaviour {

	public GameObject[] enemies;
	public int amount;
	public int howManyToSpawn;
	private Vector3 spawnPoint;



	// Update is called once per frame
	void Update () {
		enemies = GameObject.FindGameObjectsWithTag ("ENEMY_TARGET");
		amount = enemies.Length;


		if (amount != howManyToSpawn) {
			InvokeRepeating ("spawnEnemy", 1, 5f);
		}
	


	}

	void spawnEnemy (){

		spawnPoint.x = Random.Range (-20,20);
		spawnPoint.y = 2;
		spawnPoint.z = Random.Range (-20,20);

		Instantiate (enemies [UnityEngine.Random.Range(0, enemies.Length -1)], spawnPoint, Quaternion.identity);
		CancelInvoke ();


	}
}
