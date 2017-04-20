using UnityEngine;
using System.Collections;

//This script moves the targets across the battlefield

public class BuzzAround : MonoBehaviour {

	float waypointtime = 0.0f;
	
	Vector3 waypoint;
	
	Vector3 getWaypoint()
	{
		waypointtime = Time.time + 3.0f;
		return new Vector3((Random.value - 0.5f) * 100.0f, 3.0f + Random.value * 20.0f, transform.position.z - 50.0f);
	}
	
	
	public void Hit()
	{
		GetComponent<Rigidbody>().useGravity = true;
		this.enabled = false;
	}
	
	
	void FixedUpdate () 
	{
		if((waypoint-transform.position).magnitude < 3.0f || Time.time > waypointtime)
		{
			waypoint = getWaypoint();
		}
		
		Vector3 toWaypoint = (waypoint-transform.position).normalized * 20.0f;
		
		GetComponent<Rigidbody>().AddForce((toWaypoint - GetComponent<Rigidbody>().velocity) * 10.0f * Time.deltaTime);
		
	}
}
