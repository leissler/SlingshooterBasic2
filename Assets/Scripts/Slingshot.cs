using UnityEngine;
using System.Collections;

public class Slingshot : MonoBehaviour {

	// Public inspector fields

	public float velocityMultiplier = 4.0f;

	private GameObject launchPoint;

	public GameObject prefabProjectile;

	// Internal fields

	private bool aimingMode;
	private Vector3 launchPos;

	private GameObject projectile;

	void Awake() {
		Transform launchPointTrans = transform.Find("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive(false);

		launchPos = launchPointTrans.position;

	}

	void OnMouseEnter() {
		//print ("Yay, the mouse has entered!");
		launchPoint.SetActive(true);
	}

	void OnMouseExit() {
		//print ("Oh no, the mouse has exited!");
		launchPoint.SetActive(false);
	}

	void OnMouseDown() {
		// Set the aiming mode variable to true
		aimingMode = true;

		// Instatiate a projectile and store it in our "projectile" variable
		projectile = Instantiate( prefabProjectile ) as GameObject;

		// Set its position to the launch point
		projectile.transform.position = launchPos;

		// Make our projectile kinematic (for now)

		projectile.GetComponent<Rigidbody>().isKinematic = true;

	}

	void Update() {
		// if we are not in aiming mode, then do nothing
		if (aimingMode == false) {
			return;
		}

		// Get the current mouse position in screen coordinates
		Vector3 mousePos2D = Input.mousePosition;

		// Convert mouse position to 3D world space
		mousePos2D.z = - Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

		// Find the delta between the launch position and my mouse position
		Vector3 mouseDelta = mousePos3D - launchPos;

		// Move the projectile to this new position

		float radius = GetComponent<SphereCollider>().radius; 
		mouseDelta = Vector3.ClampMagnitude(mouseDelta, radius);

		projectile.transform.position = launchPos + mouseDelta;


		// if the mouse button has been released
		if(Input.GetMouseButtonUp(0) ) {

			// exit aiming mode
			aimingMode = false;

			// stop the projectle from being kinematic
			projectile.GetComponent<Rigidbody>().isKinematic = false;

			// set the projectiles velocity to the negative mouse delta vector
			projectile.GetComponent<Rigidbody>().velocity = -mouseDelta * velocityMultiplier;


			// Set the cameras poi to this newly created projectile
			FollowCam.S.poi = projectile;
		}
	}










	
}
