using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCoins : MonoBehaviour
{
  //vars that can be changed
  public int GhostCoinValue = 5;
  public float GhostCoinTransparency;

  //vars that shouldn't be changed
  public GameObject player;
  public GameObject camera;
  public Rigidbody2D rb;
  private int NumberOfCoins = 0;
  private float playerRotation;
  private int firstBound = 120; //firstBound is the first bound in CCW direction (degrees) --Set value for default setting
  private int secondBound = 240; //  -- set for default value
  private int minBoundSize = 30; //degrees
  private int maxBoundSize = 120; //degrees

  void Start() {
    UpdateCoins();
    HideGhostCoins(false);
  }

  void Update() {
    playerRotation = camera.transform.eulerAngles.z;
    Debug.Log(playerRotation);
    if (playerRotation > firstBound && playerRotation < secondBound) {
      HideGhostCoins(false);
    } else {
      HideGhostCoins(true);
    }
  }

  private void HideGhostCoins(bool hide) {
    GameObject[] AllGhostCoins = GameObject.FindGameObjectsWithTag("GhostCoins");

    if (hide) {
      foreach (GameObject gc in AllGhostCoins) {
        gc.GetComponent<Collider2D>().enabled = false;
        Color32 spriteColor = gc.GetComponent<SpriteRenderer>().color;
        gc.GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 0f); //transparent
      }
    } else {
      foreach (GameObject gc in AllGhostCoins) {
        gc.GetComponent<Collider2D>().enabled = true;
        Color32 spriteColor = gc.GetComponent<SpriteRenderer>().color;
        gc.GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, GhostCoinTransparency); //default opacity
      }
    }
  }

  void UpdateCoins()
  {
    NumberOfCoins = player.GetComponent<GameEffect>().getCoins();
  }
   
  private void GhostCoin() {
    float rot = transform.eulerAngles.z; //player rotation in degrees
    player.GetComponent<GameEffect>().ChangeCoinValue(GhostCoinValue);
    player.GetComponent<GameEffect>().IncreaseSavedGhostCoins();
  }

  private void changeGhostCoinBound() {
    int newFirstBound = Random.Range(0, 360);
    int newBoundSize = Random.Range(minBoundSize, maxBoundSize);
    firstBound = newFirstBound;
    secondBound = firstBound + newBoundSize;
  }

  private void OnCollisionEnter2D(Collision2D other) {
    if (other.gameObject.tag == "GhostCoins") {
      GhostCoin();
      Destroy(other.gameObject);
    }
  }
}
