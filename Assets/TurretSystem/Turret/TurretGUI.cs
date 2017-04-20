using UnityEngine;
using System.Collections;

public class TurretGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	bool showgui = false;
	
	void OnGUI()
	{
		
		
		GUILayout.BeginArea(new Rect(10,10,250,Screen.height-20));
		
		GUILayout.BeginHorizontal();
		showgui = GUILayout.Toggle(showgui, "Show config.");
		
		if(showgui)
		{
			Turret.usePrediction = GUILayout.Toggle(Turret.usePrediction, "Use prediction");
			GUILayout.EndHorizontal();
			
			GUILayout.Label("Target count: "+TargetManager.enemycount.ToString() + " : " + Target.getAllTargetList().Count.ToString());
			TargetManager.enemycount = (int)GUILayout.HorizontalSlider(TargetManager.enemycount, 0, 50);
			
			GUILayout.Label("Fire delay: "+Turret.firingdelay.ToString());
			Turret.firingdelay = GUILayout.HorizontalSlider(Turret.firingdelay, 0.0f, 1.0f);
			
			GUILayout.Label("Range: "+Turret.range.ToString());
			Turret.range = GUILayout.HorizontalSlider(Turret.range, 10.0f, 100.0f);
			
			GUILayout.Label("Projectile speed: "+Turret.projectileSpeed.ToString());
			Turret.projectileSpeed = GUILayout.HorizontalSlider(Turret.projectileSpeed, 5.0f, 100.0f);
			
			GUILayout.Label("Aim speed: "+Turret.aim_speed.ToString());
			Turret.aim_speed = GUILayout.HorizontalSlider(Turret.aim_speed, 1.0f, 400.0f);
			
			GUILayout.Label("Target selection weights");
			GUILayout.Label("Distance weight: "+Turret.weight_distance.ToString());
			Turret.weight_distance = GUILayout.HorizontalSlider(Turret.weight_distance, -1.0f, 1.0f);
			
			GUILayout.Label("Acceleration weight: "+Turret.weight_acceleration.ToString());
			Turret.weight_acceleration = GUILayout.HorizontalSlider(Turret.weight_acceleration, 0.0f, 1.0f);
			
			GUILayout.Label("Favourite weight: "+Turret.weight_favourite.ToString());
			Turret.weight_favourite = GUILayout.HorizontalSlider(Turret.weight_favourite, 0.0f, 1.0f);
			
			GUILayout.Label("Other turrets favourite weight: "+Turret.weight_othersfavourite.ToString());
			Turret.weight_othersfavourite = GUILayout.HorizontalSlider(Turret.weight_othersfavourite, 0.0f, 1.0f);
			
			GUILayout.Label("Aim distance weight: "+Turret.weight_aimdistance.ToString());
			Turret.weight_aimdistance = GUILayout.HorizontalSlider(Turret.weight_aimdistance, 0.0f, 1.0f);
			
			GUILayout.Label("Health weight: "+Turret.weight_health.ToString());
			Turret.weight_health = GUILayout.HorizontalSlider(Turret.weight_health, -1.0f, 1.0f);
			
			GUILayout.Label("Precision: "+Turret.precision.ToString());
			Turret.precision = GUILayout.HorizontalSlider(Turret.precision, 0.1f, 3.0f);
		}
		else
		{
			GUILayout.EndHorizontal();
		}
		
		GUILayout.EndArea();
		
		/*
		if(GUI.Button(new Rect(100,100,100,100), "spawn"))
		{
			for(int i = 0; i < 1; i++)
			{
				Instantiate(targetPrefab, new Vector3((Random.value - 0.5f) * 100, 60.0f, (Random.value - 0.5f) * 100), Quaternion.identity);
			}
		}
		GUI.Label(new Rect(100,200,100,100), Target.allTargets.Count.ToString());
		GUI.Label(new Rect(100,300,100,100), killtime.ToString());
	*/
	}
}
