using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBot : MonoBehaviour
{
	public Transform m_Transform;
	public GameObject m_Player;
	public float Range = 100.0f;
	public bool AutoFire = true;
	public static bool isFiring;

	void Start()
	{
		m_Transform = transform;
	}

	protected virtual void OnEnable()
	{
		
	}

	protected virtual void OnDisable()
	{
		
	}

	void FixedUpdate()
	{
		if (AutoFire)
		{
			Ray ray = new Ray(m_Transform.position, m_Transform.forward);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				Rigidbody body = hit.collider.attachedRigidbody;
				if (body != null && !body.isKinematic)
				{
					//m_Player.Attack.TryStart();
					isFiring = true;
				}
				else
				{
					//m_Player.Attack.TryStop();
					isFiring = false;
				}
			}
		}
	}
}
