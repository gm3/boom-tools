using UnityEngine;
using System.Collections;



public class DroneBehaviour : MonoBehaviour
{

	public GameObject player;
	public GameObject targetHeightTransform;
	public float minY;
	public float maxY;
	public float targetHeight;
	public float targetMinHeight;
	public float targetMaxHeight;
	private bool isGrounded;
	public float upThrust = 5F;
	//public Transform targetToLookAt;

	private Rigidbody rb;


	[SerializeField]
	private float moveSpeed = 250;
	[SerializeField]
	private float minDistance = 50.0f;
	[SerializeField]
	private float maxDistance = 100.0f;
	[SerializeField]
	private float rotationDrag = 0.75f;
	[SerializeField]
	private bool canShoot = true;
	[SerializeField]
	private float brakeForce = 5f;

	private bool isShooting = false;
	private Vector3 direction;
	private float distance = 0.0f;


	public enum CurrentState { Idle, Following, Attacking };
	public CurrentState currentState;
	public bool debugGizmo = true;

	public float DistanceToPlayer { get { return distance; } }
	public bool CanShoot { get { return canShoot; } set { canShoot = value; } }



	void Start()
	{  
		targetHeightTransform = GameObject.FindGameObjectWithTag("TargetHeightTransform");
		player = GameObject.FindGameObjectWithTag("Player");
		currentState = CurrentState.Idle;
		isShooting = false;
		rb = GetComponent<Rigidbody> ();
	}

	void Update() 
	{
		//Find the distance to the player
		distance = Vector3.Distance(player.transform.position, this.transform.position);

		//Face the drone to the player
		direction = (player.transform.position - this.transform.position);
		direction.Normalize();
	}

	private void FixedUpdate()
	{  
		this.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(direction, Vector3.up);
		this.GetComponent<Rigidbody>().angularDrag = rotationDrag;
		targetHeight = targetHeightTransform.transform.position.y;

		//If the player is in range move towards
		if(distance > minDistance && distance < maxDistance )
		{
			currentState = CurrentState.Following;
			DroneMovesToPlayer();
		}


		//If the player is close enough shoot
		else if(distance < minDistance)
		{

			DroneStopsMoving();

			if(canShoot)
			{
				currentState = CurrentState.Attacking;
				ShootPlayer();
			}
		}

		//If the player is out of range stop moving
		else
		{
			currentState = CurrentState.Idle;
			DroneStopsMoving();
		}

		//if (rb.transform.position.y < minY) {
		 
			//rb.AddForce(transform.up * upThrust);
		//}

		//if (rb.transform.position.y > maxY) {

			//rb.AddForce(transform.up * -upThrust);

		//}


		if (rb.transform.position.y > targetHeight) {

			rb.AddForce(transform.up * -upThrust);

		}

		if (rb.transform.position.y == targetHeight) {

			rb.AddForce(transform.up * 0);

		}

		if (rb.transform.position.y < targetHeight) {

			rb.AddForce(transform.up * upThrust);

		}

		//Vector3 relativePos = targetToLookAt.position - transform.position;
		//transform.rotation = Quaternion.LookRotation (relativePos);

	}

	private void DroneStopsMoving()
	{ 
		isShooting = false;
		this.GetComponent<Rigidbody>().drag = (brakeForce);
	}

	private void DroneMovesToPlayer()
	{
		isShooting = false;
		this.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * moveSpeed);
	}

	private void ShootPlayer()
	{
		isShooting = true;
		//Shoot player ...
	}

	private void OnDrawGizmosSelected()
	{
		if (debugGizmo)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(this.transform.position, maxDistance);

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.transform.position, minDistance);
		}
	}

	void OnCollisionStay(Collision collisionInfo) {
		//isGrounded = true;
		}

}