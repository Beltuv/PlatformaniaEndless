using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafManager : MonoBehaviour
{
    public float LeafDuration; //how long the UI covers the screen
    public float LeafRange; //range in one direction (radius)
    public float LeafSpawnHeight;
    public int LeafLimit;
    public float timeBetweenInitalSpawns;
    public int minCooldown;
    public int maxCooldown;
    public Transform playerTransform;
    public Rigidbody2D rb;
    public GameObject LeafObject;
    public Transform LeafParent;
    public GameObject UIObject;

    //private vars
    private Color32 colorTransparent = new Color32(0,0,0,0);
    public bool LeafActive = false;
    public int activeLeaves = 0; //amount of active leaves at once (editable from leaf mini script)
    private bool blockDropSpawn = false;
    public bool UIEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        UIObject.SetActive(false);
        LeafObject.GetComponent<SpriteRenderer>().color = new Color32(255, 144, 0, 255);
        LeafActive = true; //temp until we specify only active in fall biome
    }

    void Update() {
        if (LeafActive) {
            spawnLeaf();
        }
    }

    Vector3 pickDropSpawn () {
        float playerX = playerTransform.position.x;
        float playerY = playerTransform.position.y;

        float spawnX = Random.Range(playerX - LeafRange, playerX + LeafRange);
        float spawnY = playerY + LeafSpawnHeight;
        float spawnZ = LeafObject.transform.position.z;

        Vector3 spawnPos = new Vector3(spawnX, spawnY, spawnZ);

        return spawnPos;
    }

    void spawnLeaf() {
        if (activeLeaves < LeafLimit && blockDropSpawn == false) {
            activeLeaves++;
            StartCoroutine(dropWaitTimer());
            GameObject newLeaf = Instantiate(LeafObject, LeafParent);
            newLeaf.transform.position = pickDropSpawn();
        }
    }

    IEnumerator dropWaitTimer () {
        blockDropSpawn = true;
        yield return new WaitForSeconds(timeBetweenInitalSpawns);
        blockDropSpawn = false;
    }

    //temp sol to leaf not expanding in UI (instant enable/disable)
    public void TriggerLeafUI () { //call coroutine
        if (UIEnabled == false) {
            UIEnabled = true;
            StartCoroutine(LeafRoutine());
        }
    }
    IEnumerator LeafRoutine() {
        UIObject.SetActive(true);
        yield return new WaitForSeconds(LeafDuration);
        UIObject.SetActive(false);
        UIEnabled = false;
    }
}
