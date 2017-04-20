using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This script spawns a continious stream of targets

public class TargetManager : MonoBehaviour 
{
	public GameObject targetPrefab;
		
	public static int enemycount = 25;
	
	float spawntime = 0.0f;
	
	void Update()
	{
		
		if(Time.time > spawntime && Target.getAllTargetList().Count < enemycount)
		{
			Instantiate(targetPrefab, new Vector3((Random.value - 0.5f) * 100.0f, Random.value * 40.0f, 120.0f), Quaternion.identity);
			spawntime = Time.time + 0.1f;
		}
	}
	
}
