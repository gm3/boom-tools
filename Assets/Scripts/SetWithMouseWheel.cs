using UnityEngine;
using System.Collections;

public class SetWithMouseWheel : MonoBehaviour
{
	private int c=0;
	public int maxc = 8;
	//private vp_PlayerEventHandler m_player = null;
	private GameObject go_Player;

	//void Start ()
	//{
	//go_Player = GameObject.FindGameObjectWithTag("Player");
	//m_player = go_Player.GetComponent<vp_PlayerEventHandler>();
	//}

	void Awake () 
	{
		//m_player = transform.root.GetComponentInChildren<vp_PlayerEventHandler>();
	}

	void Update ()
	{
		if (Input.GetAxis("Mouse ScrollWheel") > 0) // next
		{
			if (c < maxc)
			{
				c++;
			}
			if (c == maxc)
			{
				c = maxc;
			}
			// Weapon Change
			//m_player.SetNextWeapon.Try();
		}

		if (Input.GetAxis("Mouse ScrollWheel") < 0) // previous
		{
			if (c <= maxc)
			{
				c--;
			}
			if (c <= 0)
			{
				c = 0;
			}
			// Weapon Change
			//m_player.SetPrevWeapon.Try();
		}
	}
}