using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public Transform playerTransform;
    public Rigidbody2D rb;
    public GameObject windObject;
    public int windOffset = 25;
    public int maxCooldown = 30;
    public int minCooldown = 15;
    public float windForce;
    public float maxVelocityFromWind;

    //private vars
    private float windSpeed = 10f;
    private bool windActive = false;
    private bool nearWind = false;
    private int WindPosZ = -2;
    private Color32 windColor = new Color32(184, 255, 253, 255);
    private Color32 windColorTransparent = new Color32(184, 255, 253, 0);
    private Vector3 startPos = new Vector3(0,0,0);
    private Vector3 endPos = new Vector3(0,0,0);
    [SerializeField] private WorldGeneration WGScript;

    void Start() {
        //init
        windObject.GetComponent<SpriteRenderer>().color = windColorTransparent;
        windActive = false; 
        WaitForWind();
    }

    void windGust() {
        if (WGScript.activeBiomes[1].biomeID == WGScript.biomes[0]) {
            //wind start
            startPos = new Vector3(playerTransform.position.x - windOffset, playerTransform.position.y, WindPosZ);
            endPos = new Vector3(playerTransform.position.x + windOffset, playerTransform.position.y, WindPosZ);
            windObject.transform.position = startPos;
            windObject.GetComponent<SpriteRenderer>().color = windColor;
            windActive = true;
        }
    }

    void endWind() {
        windObject.GetComponent<SpriteRenderer>().color = windColorTransparent;
        windActive = false;

        //trigger next wind cycle
        WaitForWind();
    }

    void WaitForWind() {
        //picks a time until next wind gust and then waits, and lastly triggers the wind gust
        int waitTime = Random.Range(minCooldown, maxCooldown);
        StartCoroutine(WindTimer(waitTime));
    }

    IEnumerator WindTimer(int waitSeconds) {
        yield return new WaitForSeconds(waitSeconds);
        windGust();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            nearWind = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            nearWind = false;
        }
    }

    void Update() {
        if (windActive) {
            //going right until beyond endPos
            if (windObject.transform.position.x < endPos.x) {
                windObject.transform.position = new Vector3(windObject.transform.position.x + (windSpeed * Time.deltaTime), windObject.transform.position.y, windObject.transform.position.z);
            }
            else {
                //wind is done
                endWind();
            }

            //also need to apply force while the wind is active
            //we also need to make sure the player doesnt get force away to severely
            float currentSpeed = Vector3.Magnitude(rb.velocity);

            if (nearWind == true) {
                //only apply force when player is near the wind object
                rb.AddForce(new Vector2(windForce, 0));
            }
            
        }
    }
    
}
