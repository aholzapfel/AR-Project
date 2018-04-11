using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ShipMovingBehavior : MonoBehaviour, ITrackableEventHandler
{
	
	#region PRIVATE_MEMBER_VARIABLES

	private TrackableBehaviour mTrackableBehaviour;

	#endregion // PRIVATE_MEMBER_VARIABLES

	public GameObject outsidePosition;
	public GameObject ship1;
	public GameObject ship2;

	private Vector3 ship1OriginalPosition;
	private Vector3 ship2OriginalPosition;

	private float speed = 0.1f;

	private bool targetFound = false;

	#region UNTIY_MONOBEHAVIOUR_METHODS



	void Start()
	{
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}

		ship1OriginalPosition = ship1.transform.position;
		ship2OriginalPosition = ship2.transform.position;

		ship1.transform.position = outsidePosition.transform.position;
		ship2.transform.position = outsidePosition.transform.position;
	}


	// Update is called once per frame
	void Update () {
		if (targetFound) {
			ship1.transform.position = Vector3.MoveTowards (ship1.transform.position, ship1OriginalPosition, speed * Time.deltaTime);
			ship2.transform.position = Vector3.MoveTowards (ship2.transform.position, ship2OriginalPosition, speed * Time.deltaTime);
		} else {
			ship1.transform.position = outsidePosition.transform.position;
			ship2.transform.position = outsidePosition.transform.position;
		}
	}

	#endregion // UNTIY_MONOBEHAVIOUR_METHODS



	#region PUBLIC_METHODS

	/// <summary>
	/// Implementation of the ITrackableEventHandler function called when the
	/// tracking state changes.
	/// </summary>
	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
			newStatus == TrackableBehaviour.Status.TRACKED ||
			newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			OnTrackingFound();
		}
		else
		{
			OnTrackingLost();
		}
	}

	#endregion // PUBLIC_METHODS



	#region PRIVATE_METHODS


	private void OnTrackingFound()
	{
		Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
		Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

		// Enable rendering:
		foreach (Renderer component in rendererComponents)
		{
			component.enabled = true;
		}

		// Enable colliders:
		foreach (Collider component in colliderComponents)
		{
			component.enabled = true;
		}

		targetFound = true;

		Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
	}


	private void OnTrackingLost()
	{
		Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
		Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

		// Disable rendering:
		foreach (Renderer component in rendererComponents)
		{
			component.enabled = false;
		}

		// Disable colliders:
		foreach (Collider component in colliderComponents)
		{
			component.enabled = false;
		}

		targetFound = false;

		Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");

	}

	#endregion // PRIVATE_METHODS
} 
