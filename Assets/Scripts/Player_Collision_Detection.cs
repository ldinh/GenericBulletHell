using UnityEngine;
using System.Collections;

public class Player_Collision_Detection : MonoBehaviour {

  // Use this for initialization
  void Start () {

  }

  // Update is called once per frame
  void Update () {

  }

  //Place on player object. Destroys player if it comes in contact with other objects
  void OnTriggerEnter(Collider col){
    Destroy(this.gameObject);
  }
}
