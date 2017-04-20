using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public float damage = 1.0f;
	public float lifetime = 3.0f;
	
	public Transform sender;
	bool hashit = false;
	
	float lightintensity;
	
	
	float spawntime;
	// Use this for initialization
	void Start () {
		spawntime = Time.time;
		lightintensity = GetComponent<Light>().intensity;
	}
	
	void OnCollisionEnter(Collision other)
	{
		GetComponent<Rigidbody>().useGravity = true;
		other.gameObject.SendMessage("Hit", damage, SendMessageOptions.DontRequireReceiver);
		hashit = true;
		GetComponent<Collider>().enabled = false;
		
		HitPoints hp = other.transform.GetComponent<HitPoints>();
		if(hp)
		{
			GetComponent<Light>().intensity = 1.0f;
		}
	}
	
	void Update ()
	{
		if(Time.time - spawntime > lifetime)
		{
			Destroy(gameObject);
		}
		if(!GetComponent<Collider>().enabled && sender && !hashit)
		{
			if((transform.position - sender.position).magnitude > 3.0f)
			{
				GetComponent<Collider>().enabled = true;
			}
		}
		
		GetComponent<Light>().intensity += (lightintensity - GetComponent<Light>().intensity)*0.1f;
		
	}
}
