using UnityEngine;
using System.Collections;

public class FindRadius : MonoBehaviour {

	public int numObjects = 12;
	public int radiusToSpawn = 75;
	public GameObject prefab;
	void OnEnable()
	{
		Vector3 center = transform.position;
		for (int i = 0; i < numObjects; i++)
		{
			int a = i * radiusToSpawn;
			Vector3 pos = RandomCircle(center, radiusToSpawn ,a);
			Instantiate(prefab, pos, Quaternion.identity);
		}
	}
	Vector3 RandomCircle(Vector3 center, float radius,int a)
	{
		float ang = a;
		Vector3 pos;
		pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
		pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		pos.z = center.z;
		return pos;
	}
}