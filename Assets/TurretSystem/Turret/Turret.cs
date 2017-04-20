using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret: MonoBehaviour 
{
	
	//Static list of all turrets
	public static List<Turret> allTurrets;
	
	public static List<Turret> getAllTurretList()
	{
		if(allTurrets == null)
		{
			allTurrets = new List<Turret>();
		}
		return allTurrets;
	}
	
	//Projectiles are armed when they leave the turrets collider. This is the radius used.
	float DistanceBeforeArmProjectile = 3.0f;
	
	//References to turret parts
	public Transform turretBed;
	public Transform turret;
	
	//Current euler orientation
	Vector3 currenteuler;
	
	//Speed of turrent moement
	public static float aim_speed = 300.0f;
	
	//Prefab of projectile
	public GameObject projectile;
	
	//Speed of projectile when fired
	public static float projectileSpeed = 4.0f;
	
	//How long between fire
	public static float firingdelay =2.0f;
	
	//Used to for firedelay and reloaddelay
	float nextpossiblefire = 0.0f;
	
	public static float range = 33.0f;
	
	//AI weights
	public static float weight_distance = 0.5f;
	public static float weight_acceleration = 0.3f;
	public static float weight_favourite = 1.0f;
	public static float weight_othersfavourite = 0.2f;
	public static float weight_aimdistance = 0.6f;
	public static float weight_health = 1.0f;
	
	//Turret precision
	public static float precision = 0.3f; //How many units of error do we accept
	
	
	//Turrets local list of targets
	List<Target> targets;
	
	//This turrets favourite target
	public Target favourite;
	
	// Use this for initialization
	IEnumerator Start ()
	{
		getAllTurretList().Add(this);
		targets = new List<Target>();
		foreach(Target t in Target.getAllTargetList())	
		{
			targets.Add(t);
		}
		yield return null;
	}
	
	public void AddTarget(Target t)
	{
		targets.Add(t);
	}
	
	public void RemoveTarget(Target t)
	{
		targets.Remove(t);
	}
	
	
	void OnDestroy()
	{
		getAllTurretList().Remove(this);
	}
	
	
	bool CalculateAimPoint(Vector3 targetpos, Vector3 targetvel, Vector3 ourpos, float bulletspeed, ref Vector3 aimpoint)
	{
		if(!usePrediction)
		{
			aimpoint = targetpos;
			return true;
		}
		
		Vector3 dP = targetpos - ourpos;
		float a = (targetvel.x * targetvel.x + targetvel.y * targetvel.y + targetvel.z * targetvel.z) - (bulletspeed * bulletspeed);
		float b = 2.0f * (dP.x * targetvel.x +dP.y * targetvel.y +dP.z * targetvel.z);
		float c = dP.x * dP.x + dP.y * dP.y + dP.z * dP.z;
		float d = b*b - 4 * a * c;
		if (d < 0.0f)
		{
			return false;
		}
		d = (float) Mathf.Sqrt(d);
		float t0 = (-b - d) / (2.0f * a);
		float t1 = (-b + d) / (2.0f * a);
		float t;
		if (t0 > t1)
		{
			float cx = t0;
			t0 = t1;
			t1 = cx;
		}
		if (t1 < 0.0f)
		{
			return false;
		}
		if (t0 >= 0.0f)
		{
			t = t0;
		}
		else
		{
			t = t1;
		}
	
		aimpoint = targetpos + targetvel * t;
		return true;
	}
	
	float getTargetScore(Target t)
	{
		//Big score is a good target
		
		float score = 0.0f;
		
		Vector3 toIntercept = Vector3.zero;
			
		bool canintercept = CalculateAimPoint(transform.InverseTransformPoint(t.transform.position),  transform.InverseTransformDirection(t.velocity - velocity), Vector3.zero, projectileSpeed, ref toIntercept);
		
		//Discard if interception point invalid
		if(!canintercept) 
		{
			//Debug.Log ("Cant intercept");
			return float.MinValue;
		}
		//Discard if too far away
		float dist = toIntercept.magnitude;
		if(dist > range || dist <= DistanceBeforeArmProjectile )
		{
			//Debug.Log ("Not in range");
			return float.MinValue;
		}
		
		//Discard if inside dead angle under turret
		if(Vector3.Angle(toIntercept, -transform.up) < 45.0f) 
		{
			//Debug.Log ("Below threshold");
			return float.MinValue;
		}
		
		//is there line of sight to interceptionpoint (close to target)
		RaycastHit hit = new RaycastHit();
		
		//Is there line of sight to interceptpoint, if not do I hit my target? 
		Vector3 toin = transform.TransformPoint( toIntercept );
		//Debug.DrawLine(transform.position, toin, Color.red );
		Vector3 dir = (toin - transform.position).normalized;
		if(Physics.Raycast(transform.position + dir*DistanceBeforeArmProjectile, dir, out hit, dist - DistanceBeforeArmProjectile))
		{
			Target tgt = hit.collider.transform.GetComponent<Target>();
			if(tgt == null || tgt != t)
			{
				return float.MinValue;
			}
		}
		
		HitPoints hp = t.GetComponent<HitPoints>();
		if(hp)
		{
			score += hp.points * weight_health;
		}
		
		
		//Take distance into account
		score += ((range - dist)/range) * weight_distance;
		
		//Acceleration is bad for prediction.
		score -= t.acceleration.magnitude * weight_acceleration;
		
		//Like favourite and dislike other turrets favourite
		foreach(Turret tur in allTurrets)
		{
			if(tur.favourite == t)
			{
				if(tur == this)
				{
					score += weight_favourite;	
				}
				else
				{
					score -= weight_othersfavourite;
				}
			}
			
		}
		
		
		//How many degrees must we turn gun
		score += ((180.0f - Vector3.Angle(transform.TransformDirection(turret.forward), toIntercept.normalized))/180.0f) * weight_aimdistance;
		
		//If i pull trigger now how much will I be off.
		float errorangle = Vector3.Angle(toIntercept.normalized, transform.InverseTransformDirection(turret.forward));
		float miss = toIntercept.magnitude * Mathf.Tan(errorangle * Mathf.Deg2Rad);
	
		//Should I fire?
		if(errorangle < 90 && miss < precision)
		{
			firewillhit = true;	
		}
		
		return score;
	}
	
	
	Vector3 lastposition;
	float lasttime = 0.0f;
	Vector3 velocity = Vector3.zero;
	
	public static bool usePrediction = false;
	
	public bool debug = false;
	
	bool firewillhit = false;
	
	
	
	
	// Update is called once per frame
	void Update ()
	{
		velocity = (transform.position - lastposition)/(Time.time - lasttime);
		lasttime = Time.deltaTime;
		lastposition = transform.position;
		
		
		//future proposal - each turret has own list of targets.
		
		//Find best target
		//targets.Sort((Target a, Target b)=>getTargetScore(b).CompareTo(getTargetScore(a)));
		
		firewillhit = false;
		
		//Sort list iteratively
		for(int j = 0; j < 20; j++)
		{
			bool done = false;
			for(int i=0;i<targets.Count-1;i++)
			{
				if(getTargetScore(targets[i]) < getTargetScore(targets[i+1]))
				{
					Target tmp = targets[i];
					targets[i] = targets[i+1];
					targets[i+1] = tmp;
					break;
				}
				done = true;
			}
			if(done)
			{
				break;
			}
		}
		
		if(debug)
		{
			//Draw debugline to threat
			float f = 1.0f;
			foreach(Target t in targets)
			{
				Debug.DrawLine(transform.position, t.transform.position, new Color(f,0.0f,0.0f));	
				f -= 1.0f/targets.Count; 
			}	
			
			//Draw range around turret
			int steps = 32;
			for(int i=0; i<32;i++)
			{
				float aa = Mathf.PI * 2.0f * i /(steps-1);
				float bb = Mathf.PI * 2.0f * (i+1) /(steps-1);
				Vector3 a = new Vector3(range*Mathf.Cos(aa), 0.0f, range*Mathf.Sin(aa));
				Vector3 b = new Vector3(range*Mathf.Cos(bb), 0.0f, range*Mathf.Sin(bb));
				Debug.DrawLine(transform.position + a, transform.position + b, Color.red  );	
			}
		}
		
		Vector3 aim = Vector3.forward;
		favourite = null;
		if(targets.Count > 0 && getTargetScore(targets[0]) > float.MinValue)
		{
			favourite = targets[0];
			Debug.DrawLine(transform.position, favourite.transform.position, Color.cyan);
			Vector3 interceptionpoint = Vector3.zero;
			if(CalculateAimPoint(transform.InverseTransformPoint(favourite.transform.position), transform.InverseTransformDirection(favourite.velocity - velocity), Vector3.zero, projectileSpeed, ref interceptionpoint))
			{
				aim = interceptionpoint.normalized;
			}
			if(firewillhit) //Vector3.Dot (interceptionpoint, transform.InverseTransformDirection(turret.forward)) > 0.5f &&  miss < 0.15f)
			{
				//Debug.DrawLine(turret.position, turret.position + turret.forward * 100.0f, Color.blue);
				if(Time.time > nextpossiblefire)
				{
					GameObject go = (GameObject)Instantiate(projectile, turret.position, turret.rotation);
					Projectile p = go.GetComponent<Projectile>();
					p.GetComponent<Rigidbody>().velocity = turret.forward * projectileSpeed + velocity;
					p.lifetime = range/projectileSpeed + 1.0f;
					p.sender = transform;
					nextpossiblefire = Time.time + firingdelay;
					
				}
			}
		}
		
		Vector3 target_euler = Quaternion.LookRotation(aim, transform.up).eulerAngles;

		while(target_euler.x - currenteuler.x > 180.0f)		currenteuler.x += 360.0f;
		while(target_euler.x - currenteuler.x < -180.0f)	currenteuler.x -= 360.0f;
		while(target_euler.y - currenteuler.y > 180.0f)		currenteuler.y += 360.0f;
		while(target_euler.y - currenteuler.y < -180.0f)	currenteuler.y -= 360.0f;
		
		currenteuler.x += Mathf.Clamp(target_euler.x - currenteuler.x, -aim_speed * Time.deltaTime, aim_speed * Time.deltaTime);
		currenteuler.y += Mathf.Clamp(target_euler.y - currenteuler.y, -aim_speed * Time.deltaTime, aim_speed * Time.deltaTime);
	
		//Set the rotation
		turretBed.localRotation = Quaternion.Euler(new Vector3 (0.0f, currenteuler.y, 0.0f ));
		turret.localRotation = Quaternion.Euler(new Vector3 (currenteuler.x, 0.0f, 0.0f));
	
	}
	
	
}
