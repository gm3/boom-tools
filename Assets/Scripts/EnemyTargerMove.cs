using UnityEngine;
using System.Collections;

public class EnemyTargetMove : MonoBehaviour {

	// Use this for initialization
	//public Transform track;
	public float moveSpeed = 3;
	public GameObject thingToTarget;
	public GameObject targetHeightTransform;
	public float targetHeight;
	private bool isGrounded = default;
	public float upThrust = 5F;
	private Rigidbody rb;

	//[SerializeField]
	//private float moveSpeed2 = 250;
	[SerializeField]
	private float minDistance2 = 50.0f;
	[SerializeField]
	private float maxDistance2 = 100.0f;
	[SerializeField]
	private float rotationDrag2 = 0.75f;
	[SerializeField]
	private bool canShoot2 = true;
	[SerializeField]
	private float brakeForce2 = 5f;

	private bool isShooting = false;
	private Vector3 direction;
	private float distance = 0.0f;


	public enum CurrentState { Idle, Following, Attacking };
	public CurrentState currentState;
	public bool debugGizmo = true;

	public float DistanceToPlayer { get { return distance; } }
	public bool CanShoot { get { return canShoot2; } set { canShoot2 = value; } }

	
	// Update is called once per frame
	void Start () {

		targetHeightTransform = GameObject.FindGameObjectWithTag("TargetHeightTransform");
		thingToTarget = GameObject.FindGameObjectWithTag("Player");
		currentState = CurrentState.Idle;
		isShooting = false;
		rb = GetComponent<Rigidbody> ();

		//float move = moveSpeed * Time.deltaTime;
		//transform.position = Vector3.MoveTowards (transform.position, track.position, move);
	
	}

	void Update() 
	{
		//Find the distance to the player
		distance = Vector3.Distance(thingToTarget.transform.position, this.transform.position);

		//Face the drone to the player
		direction = (thingToTarget.transform.position - this.transform.position);
		direction.Normalize();
	}

	private void FixedUpdate()
	{  
		this.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(direction, Vector3.up);
		this.GetComponent<Rigidbody>().angularDrag = rotationDrag2;
		targetHeight = targetHeightTransform.transform.position.y;

		//If the player is in range move towards
		if(distance > minDistance2 && distance < maxDistance2 )
		{
			currentState = CurrentState.Following;
			DroneMovesToPlayer();
		}


		//If the player is close enough shoot
		else if(distance < minDistance2)
		{

			DroneStopsMoving();

			if(canShoot2)
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
		this.GetComponent<Rigidbody>().drag = (brakeForce2);
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
			Gizmos.DrawWireSphere(this.transform.position, maxDistance2);

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.transform.position, minDistance2);
		}
	}

	void OnCollisionStay(Collision collisionInfo) {
		isGrounded = true;
	}
}
