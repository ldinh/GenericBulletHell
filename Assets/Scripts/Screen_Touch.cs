using UnityEngine;
using System.Collections;

public class Screen_Touch : MonoBehaviour {

  public bool isDead;
  private Touch touch;
  public GameObject player = null;
  private float elapsedTime;
  public AudioSource explosion;
  public AudioClip explosionClip;
  public AudioSource touchSource;

  public AudioClip touchClip;
  public AudioClip touchClip2;
  public AudioClip touchClip3;
  public AudioClip touchClip4;

  int canPlaytouch = 1;

  public bool isPaused = true;
  int canPlayExplosion = 1;

  // Use this for initialization
  void Start () {
    if(!PlayerPrefs.HasKey("Rounds_played"))
    {
      PlayerPrefs.SetInt("Rounds_played", 1);
    }
    else
    {
      PlayerPrefs.SetInt("Rounds_played", PlayerPrefs.GetInt("Rounds_played")+1);
    }
  }

  // Update is called once per frame
  void Update()
  {
    // if no "Player" object, player is dead.
    if (GameObject.Find ("Player") != null) {
      isDead = false;
    } else {
      isDead = true;
    }

    if(!isDead){
      elapsedTime = Time.timeSinceLevelLoad;
      if ( Input.touchCount != 0 )
      {
        Touch touch = Input.touches[0];
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if ( Physics.Raycast(ray, out hit, 100f ) )
        {
          if(hit.transform.gameObject.name == "Player") //If the user is touching the player character, game unpauses.
          {
            Time.timeScale = 1;
            isPaused = false;
            if(canPlaytouch == 1)
            {
              float rand = Random.Range(0, 1.0f);
              Debug.Log ("rand: " + rand);
              if(rand > 0 && rand <= 0.25f) //Play a random sound when user touches player character
              {
                touchSource.PlayOneShot(touchClip, 1.0f);
                canPlaytouch = 0;
              }
              if(rand > 0.25f && rand <= 0.50f)
              {
                touchSource.PlayOneShot(touchClip2, 1.0f);
                canPlaytouch = 0;
              }
              if(rand > 0.50f && rand <= 0.75f)
              {
                touchSource.PlayOneShot(touchClip3, 1.0f);
                canPlaytouch = 0;
              }
              if(rand > 0.75f)
              {
                touchSource.PlayOneShot(touchClip4, 1.0f);
                canPlaytouch = 0;
              }
            }
          }
          else //user not touching player char
          {
            Time.timeScale = 0;
            isPaused = true;
          }
        }
      }
      else //User not touching screen
      {
        Time.timeScale = 0;
        isPaused = true;
        canPlaytouch = 1;
      }
    }
    else
    {
      Time.timeScale = 1;
      isPaused = false;
      if(canPlayExplosion == 1)
      {
        canPlayExplosion = 0;
        explosion.PlayOneShot(explosionClip, 1.0f);
      }
    }
  }

  void OnGUI()
  {
    //Game over menu
    if(isDead){
      // Debug.Log("Rounds_played: " + PlayerPrefs.GetInt("Rounds_played"));
      GUI.Box(new Rect(Screen.width * (0.10f),Screen.height * (0.10f),Screen.width * (0.8f), Screen.height * (0.8f)), "Game Over");

      GUI.Label (new Rect(Screen.width * (1f/2f) - ((Screen.width * (3f/5f))/2),Screen.height * (0.2f),Screen.width * (3f/5f),Screen.height * (1f/9f)), "Score: " + elapsedTime.ToString("F2") + " sec");

      if(GUI.Button(new Rect(Screen.width * (0.2f),Screen.height * (0.5f), Screen.width * (0.6f), Screen.height * (0.125f)), "Retry")) {
        if(PlayerPrefs.GetInt("Rounds_played") % 3 == 0){
          //Mobile Ad code: Display a video add if the "round" that the player is on is a multiple of 3.
        }
        Application.LoadLevel (Application.loadedLevelName); //reload stage
      }
    }
  }

}
