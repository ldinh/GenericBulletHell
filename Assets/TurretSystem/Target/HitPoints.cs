using UnityEngine;
using System.Collections;

//This script takes care of hitpoints and dying

public class HitPoints : MonoBehaviour 
{
	
	public float points = 1.0f;
	
	public void Hit (float damage) 
	{
		MeshRenderer mr = transform.GetComponent<MeshRenderer>();
		mr.material.color = Color.green;
		
		
		points -= damage;
		if(points <= 0.0f)
		{
			Destroy(gameObject);
		}
	
	}
}
