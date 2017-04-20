using UnityEngine;

using System.Collections.Generic;
using System.Collections;

public class Turret_Randomizer : MonoBehaviour {

  GameObject[] startTurrets;
  List<GameObject> randomizer;
  Dictionary<GameObject, bool> allTurrets;
  int activeTurrets = 0;
  int sel;
  private float elapsedTime;

  // Use this for initialization
  void Start ()
  {
    randomizer = new List<GameObject>();
    startTurrets = GameObject.FindGameObjectsWithTag("Turret");
    allTurrets = new Dictionary<GameObject, bool>();
    foreach(GameObject turret in startTurrets)
    {
      allTurrets.Add(turret, false);
    }
    InvokeRepeating ("AddActiveTurret", 3, 3);
  }

  //Activate turrets if there are less than 5 active turrets.
  void AddActiveTurret()
  {
    if(ActiveTurretCounter () < 5)
    {
      foreach(KeyValuePair<GameObject, bool> turret in allTurrets)
      {
        if(turret.Value == false) //finds inactive turrets, places keys in array
        {
          randomizer.Add(turret.Key);
        }
      }
      sel = Random.Range (0, randomizer.Count - 1); //Randomly selected turret to turn on
      GameObject rando_to_turn_on = randomizer [sel];
      allTurrets [rando_to_turn_on] = true;
      randomizer = new List<GameObject>();
    }
    else
    {
      foreach(KeyValuePair<GameObject, bool> turret in allTurrets)
      {
        if(turret.Value == false) //finds inactive turrets, places keys in array
        {
          randomizer.Add(turret.Key);
        }
      }
      sel = Random.Range (0, randomizer.Count - 1); //Randomly selected turret to turn off
      GameObject rando_to_turn_on = randomizer [sel];
      randomizer = new List<GameObject>();

      foreach(KeyValuePair<GameObject, bool> turret in allTurrets)
      {
        if(turret.Value == true) //finds inactive turrents, places keys in array
        {
          randomizer.Add(turret.Key);
        }
      }
      sel = Random.Range (0, randomizer.Count - 1);
      GameObject rando_to_turn_off = randomizer [sel];
      allTurrets [rando_to_turn_on] = true;
      allTurrets [rando_to_turn_off] = false;
    }
  }

  //Counts the number of active turrets.
  int ActiveTurretCounter ()
  {
    activeTurrets = 0;
    foreach(KeyValuePair<GameObject, bool> turret in allTurrets)
    {
      if(turret.Value == true)
      {
        activeTurrets++;
      }
    }
    return activeTurrets;
  }

  // Update is called once per frame
  void Update ()
  {
    foreach (KeyValuePair<GameObject, bool> turret in allTurrets)
    {
      if(turret.Value == true)
      {
        turret.Key.transform.Find("Stand_default").gameObject.GetComponent<Renderer>().material.color = Color.green;
        turret.Key.transform.Find("Stand_default").gameObject.transform.Find("Bed_default").gameObject.GetComponent<Renderer>().material.color = Color.green;
        turret.Key.transform.Find("Stand_default").gameObject.transform.Find("Bed_default").gameObject.transform.Find("Turret_default").gameObject.GetComponent<Renderer>().material.color = Color.green;
        turret.Key.gameObject.GetComponent<Turret>().enabled = true;
      }
      else
      {
        turret.Key.transform.Find("Stand_default").gameObject.GetComponent<Renderer>().material.color = Color.red;
        turret.Key.transform.Find("Stand_default").gameObject.transform.Find("Bed_default").gameObject.GetComponent<Renderer>().material.color = Color.red;
        turret.Key.transform.Find("Stand_default").gameObject.transform.Find("Bed_default").gameObject.transform.Find("Turret_default").gameObject.GetComponent<Renderer>().material.color = Color.red;
        turret.Key.gameObject.GetComponent<Turret>().enabled = false;

      }
    }
  }
}
