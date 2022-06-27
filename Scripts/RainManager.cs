using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainManager : MonoBehaviour
{
    public float rainForce;
    public float rainRange; //range in one direction (radius)
    public float rainSpawnHeight;
    public int rainTime; //Duration of rain period
    public int rainDropLimit;
    public float timeBetweenInitalSpawns;
    public int minCooldown;
    public int maxCooldown;
    public Transform playerTransform;
    public Rigidbody2D rb;
    public GameObject rainObject;
    public Transform rainParent;

    //private vars
    private Color32 colorTransparent = new Color32(0,0,0,0);
    private bool rainActive = false;
    public int activeRainDrops = 0; //amount of active rain drops at once (editable from raincollision script)
    private bool blockDropSpawn = false;
    // Start is called before the first frame update
    void Start()
    {
        rainObject.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 255, 255);
        rainActive = true;
        WaitForRain();
    }

    void Update() {
        if (rainActive) {
            spawnRainDrop();
        }
    }

    Vector3 pickDropSpawn () {
        float playerX = playerTransform.position.x;
        float playerY = playerTransform.position.y;

        float spawnX = Random.Range(playerX - rainRange, playerX + rainRange);
        float spawnY = playerY + rainSpawnHeight;
        float spawnZ = rainObject.transform.position.z;

        Vector3 spawnPos = new Vector3(spawnX, spawnY, spawnZ);

        return spawnPos;
    }

    void spawnRainDrop() {
        if (activeRainDrops < rainDropLimit && blockDropSpawn == false) {
            activeRainDrops++;
            StartCoroutine(dropWaitTimer());
            GameObject newRainDrop = Instantiate(rainObject, rainParent);
            newRainDrop.transform.position = pickDropSpawn();
        }
    }

    void WaitForRain() {
        //picks a time until next rain period and then waits, and lastly triggers the rain period
        int waitTime = Random.Range(minCooldown, maxCooldown);
        StartCoroutine(RainTimer(waitTime));
    }

    IEnumerator RainTimer(int waitSeconds) {
        yield return new WaitForSeconds(waitSeconds);
        //startRain() ??
    }

    IEnumerator dropWaitTimer () {
        blockDropSpawn = true;
        yield return new WaitForSeconds(timeBetweenInitalSpawns);
        blockDropSpawn = false;
    }
}
