using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This script indicates to the turrets that this is a target.

//It also pickups speed and accelecation to be used for aiming

public class Target : MonoBehaviour 
{
	//Static list of all targets
	public static List<Target> allTargets;
	
	Vector3 lastvelocity;
	Vector3 lastposition;
	public Vector3 velocity = Vector3.zero;
	public Vector3 acceleration = Vector3.zero;
	
	//float spawntime;
	
	//Turrets operate on this list of targets to figure out what to aim at
	public static List<Target> getAllTargetList()
	{
		if(allTargets == null)
		{
			allTargets = new List<Target>();
		}
		return allTargets;
	}
	
	void Start () 
	{
		getAllTargetList().Add(this);
		foreach(Turret t in Turret.getAllTurretList())
		{
			t.AddTarget(this);
		}
		lastposition = transform.position;
		//spawntime = Time.time;
	}
	
	
	void FixedUpdate()
	{
		acceleration = (velocity - lastvelocity)/Time.deltaTime;
		lastvelocity = velocity;
		
		velocity = (transform.position - lastposition)/Time.deltaTime;
		lastposition = transform.position;
	
		//if(Time.time - spawntime > 60.0f)
		//{
		//	Destroy(gameObject);
		//}
		
		//Debug.DrawLine(transform.position, transform.position + velocity, Color.blue);
		//Debug.DrawLine(transform.position, transform.position + acceleration*10.0f, Color.red);
		
	}
	
	void OnDestroy()
	{
		allTargets.Remove(this);
		foreach(Turret t in Turret.allTurrets)
		{
			t.RemoveTarget(this);
		}
	}
}