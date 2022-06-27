using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowflake : MonoBehaviour
{
    public Transform playerTransform;
    public Rigidbody2D rb;
    public GameObject SnowflakeObject;
    public float FrozenTime;
    public float FallSpeed;
    public float TargetingPercentage; //percentage of the distance from snowflake and player (amount it moves towards the player)
    public float SpawnRange;
    public float SpawnHeight;
    public int maxCooldown;
    public int minCooldown;

    private Color32 snowflakeColor = new Color32(255, 255, 255, 255);
    private Color32 snowflakeColorTransparent = new Color32(255, 255, 255, 0);
    private bool snowflakeActive = false;
    private Vector3 startPos = new Vector3(0,0,-1f);
    private float endHeight;
    [SerializeField] private WorldGeneration WGScript;
    // Start is called before the first frame update
    void Start()
    {
        SnowflakeObject.GetComponent<SpriteRenderer>().color = snowflakeColorTransparent;
        WaitForSnowflake();
    }

    void StartSnowflake() {
        if (WGScript.activeBiomes[1].biomeID == WGScript.biomes[0]) {
            float SpawnX = Random.Range(playerTransform.position.x - SpawnRange, playerTransform.position.x + SpawnRange);
            float SpawnY = playerTransform.position.y + SpawnHeight;
            startPos = new Vector3(SpawnX, SpawnY, startPos.z);
            SnowflakeObject.transform.position = startPos;
            endHeight = playerTransform.position.y - SpawnHeight;
            Debug.Log("StartsnowflakeEndHeight-" + endHeight.ToString());
            SnowflakeObject.GetComponent<SpriteRenderer>().color = snowflakeColor;
            snowflakeActive = true;
        }
        Debug.Log("StartSnow");
    }

    void EndSnowflake() {
        SnowflakeObject.GetComponent<SpriteRenderer>().color = snowflakeColorTransparent;
        snowflakeActive = false;
        Debug.Log("EndSnow");
        //trigger next wind cycle
        WaitForSnowflake();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            playerTransform.gameObject.GetComponent<MovementScript>().FreezeMovement = true;
            StartCoroutine(FrozenTimer());
            Debug.Log("FREEZE");
            Debug.Log(playerTransform.gameObject.GetComponent<MovementScript>().FreezeMovement);
        }
    }

    IEnumerator FrozenTimer() {
        yield return new WaitForSeconds(FrozenTime);
        playerTransform.gameObject.GetComponent<MovementScript>().FreezeMovement = false;
        Debug.Log("UNFREEZE");
        Debug.Log(playerTransform.gameObject.GetComponent<MovementScript>().FreezeMovement);
    }

    void WaitForSnowflake() {
        //picks a time until next wind gust and then waits, and lastly triggers the wind gust
        int waitTime = Random.Range(minCooldown, maxCooldown);
        StartCoroutine(SnowflakeTimer(waitTime));
    }

    IEnumerator SnowflakeTimer(int waitSeconds) {
        yield return new WaitForSeconds(waitSeconds);
        StartSnowflake();
    }

    // Update is called once per frame
    void Update()
    {
        if (snowflakeActive) {
            if (SnowflakeObject.transform.position.y > endHeight) {
                bool shield = false;
                if (shield == false) {
                    shield = true;
                    float xShift = (SnowflakeObject.transform.position.x - playerTransform.position.x) * TargetingPercentage;
                    SnowflakeObject.transform.position = new Vector3(SnowflakeObject.transform.position.x - xShift, SnowflakeObject.transform.position.y - (FallSpeed * Time.deltaTime), SnowflakeObject.transform.position.z);
                    shield = false;
                }
            }
            else {
                EndSnowflake();
            }
        }
    }
}
