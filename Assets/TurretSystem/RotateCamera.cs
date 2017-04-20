using UnityEngine;
using System.Collections;

//This script rotates the origin of our camera so we can see the battlefield

public class RotateCamera : MonoBehaviour 
{

	void Update () 
	{
		transform.Rotate(0f, Time.deltaTime, 0f);
	}
}
