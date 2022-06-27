using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SaveNamespace;

public class GameEffect : MonoBehaviour
{
    public GameObject cameraObject;
    public int Coins;
    public Rigidbody2D rb;
    public Text ClockText;
    public int LifeTime;
    public int DealthPenalty;
    public float WorldRotationSpeed;
    public int MaxCrazyModeSpeed;
    public int MaxCrazyModeTimeInterval;
    public int VoidVelocity;
    public Vector3 SpawnPoint;
    public GameObject respawnObject;
    public GameObject coinCountObject;
    public Text TrophyCountObject;
    public float FlyPadForce;
    public float FlyPadTime;
    public float CoinRespawnTime;
    public int AutoSaveTime;
    public GameObject GameOverImage;
    public Animator GameOverAnimator;
    public static int TrophyCount;
    public CircleCollider2D playerCircleCollider;
    public Transform rsr;
    public Transform lsr;


    float DefaultGravity;
    int DefaultTime;
    float GameOverScreenTime = 3f;
    public static bool CrazyModeIsActive = false;
    public static bool ToggleCrazyMode = false;

    Color32 EasyModeColor = new Color32(0, 210, 255, 255);
    Color32 NormalModeColor = new Color32(204, 174, 255, 255);
    Color32 HardModeColor = new Color32(255, 119, 0, 255);

    SaveInfo saveInfo = new SaveInfo(); //main SaveInfo object
    
    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       DefaultGravity = rb.gravityScale;
       DefaultTime = int.Parse(ClockText.text);

       loadData();

       LoadCoin();
       SetTrophyCount(Coins); //LoadsAmount
       StartCoroutine(ReduceTime(LifeTime)); //Begins a permanent loop of the game.

       if (CrazyModeIsActive == true) {
           CrazyMode();
       }

       rb.gameObject.GetComponent<SpriteRenderer>().color = saveInfo.getActiveColor();
    }

    // Update is called once per frame
    void Update()
    {
        //Return to Main Menu
        if (Input.GetKey(KeyCode.Escape)) {
            if (SceneManager.GetActiveScene().buildIndex != 0) {
                saveData(saveInfo);
                SceneManager.LoadScene(0);
            }
        }

        //respawn detection
        

        if (rb.velocity.y <= VoidVelocity) { // <= because its negative
            Respawn();
        }

        //Main Rotation Effect
        cameraObject.transform.Rotate(0f, 0f, 1f * Time.deltaTime * WorldRotationSpeed);

        //Settings Detection
        if (ToggleCrazyMode == true) {
           CrazyMode();
        }

        //Player Info Section
        if (GameObject.Find("GameManager")) {
            if(GameObject.Find("GameManager").GetComponent<PlayerInfo>().WaitingForGameCountUpdate == true) {
                GameObject.Find("GameManager").GetComponent<PlayerInfo>().WaitingForGameCountUpdate = false;
                IncreaseGameCount();
            }
        }
        updateDist();
    }

    //Player Stats Recorder Methods
    int tempDist = 0;
    void updateDist() {
        if (tempDist != 0) {
            int addedDist = Mathf.RoundToInt(Mathf.Abs(rb.gameObject.transform.position.x - tempDist));
            if (addedDist > 0) {
                saveInfo.totalDist = saveInfo.totalDist + addedDist;
                tempDist = 0;
            }
        } else {
            tempDist = Mathf.RoundToInt(rb.gameObject.transform.position.x);
        }
    }
    public void IncreaseSavedJumps() {
        saveInfo.totalJumps = saveInfo.totalJumps + 1;
    }
    public void IncreaseSavedGhostCoins() {
        saveInfo.totalGhostCoins = saveInfo.totalGhostCoins + 1;
    }
    public void IncreaseSavedTime() {
        saveInfo.totalTime = saveInfo.totalTime + 1;
    }
    public void IncreaseGameCount() {
        Debug.Log("Before:" + saveInfo.totalGames.ToString());
        saveInfo.totalGames = saveInfo.totalGames + 1;
        Debug.Log("After:" + saveInfo.totalGames.ToString());
        saveData(saveInfo);
    }
    public void IncreaseDeathCount() {
        saveInfo.totalDeaths = saveInfo.totalDeaths + 1;
    }

    //Save Method
    public void saveData(SaveNamespace.SaveInfo saveClass) {
        Debug.Log("Saving..." + saveClass.trophy);
        string json = JsonUtility.ToJson(saveClass);
        Debug.Log(json);
        File.WriteAllText(Application.dataPath + "/saveFile.json", json);
    }

    public void loadData() {
        string Readjson = File.ReadAllText(Application.dataPath + "/saveFile.json");
        saveInfo = JsonUtility.FromJson<SaveNamespace.SaveInfo>(Readjson);
    }

    void Respawn() {
        rb.velocity = new Vector2(0, 0);
        
        //Check if respawn point is on the edge of the platform
        //there could be a future problem with the *4 in the ray origins
        float playerSizeOffset = playerCircleCollider.radius*6; //How much the respawn will be fixed by (distance)
        float rayDist = 200f;
        Vector3 leftRayOrigin = new Vector3(respawnObject.transform.position.x - playerSizeOffset, respawnObject.transform.position.y, respawnObject.transform.position.z);
        Vector3 rightRayOrigin = new Vector3(respawnObject.transform.position.x + playerSizeOffset, respawnObject.transform.position.y, respawnObject.transform.position.z);
        RaycastHit2D leftRay = Physics2D.Raycast(leftRayOrigin, Vector2.down, rayDist);
        RaycastHit2D rightRay = Physics2D.Raycast(rightRayOrigin, Vector2.down, rayDist);
        if (leftRay.collider == null) {
            //respawn needs to be moved to the right
            transform.position = new Vector3(respawnObject.transform.position.x + playerSizeOffset, respawnObject.transform.position.y, respawnObject.transform.position.z);
            Debug.Log("Needs to be moved right");
        } 
        if (rightRay.collider == null) {
            //respawn needs to be moved to the left
            transform.position = new Vector3(respawnObject.transform.position.x - playerSizeOffset, respawnObject.transform.position.y, respawnObject.transform.position.z);
            Debug.Log("Needs to be moved left");
        }

        if (rightRay.collider && leftRay.collider) {
            //respawn point is already good
            transform.position = respawnObject.transform.position;
            Debug.Log("Respawn is already good!");
        }
        
        IncreaseDeathCount();
        rb.gameObject.GetComponent<MovementScript>().FreezeMovement = false;

        Coins = Coins - DealthPenalty;
        if (Coins < 0) {
            //game over
            saveData(saveInfo);
            StartCoroutine(GameOver());
        }
        LoadCoin();
    }

    private void OnCollisionEnter2D(Collision2D other) {

        if (other.gameObject.tag == "Coins") {
            CoinCollision(other);
        }

        if (other.gameObject.tag == "FlyPad") {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector3.up * FlyPadForce);
            Invoke("ResetGravity", FlyPadTime);
        }
    }

    void LoadCoin() {
        coinCountObject.GetComponent<UnityEngine.UI.Text>().text = Coins.ToString();
    }

    void ResetGravity() {
        rb.gravityScale = DefaultGravity;
    }

    IEnumerator CoinHit (GameObject coin) {
        coin.SetActive(false);

        yield return new WaitForSeconds(CoinRespawnTime);

        if (coin != null) {
            coin.SetActive(true);
        }
    }

    void CoinCollision(Collision2D other) {
        Coins += 1;
        saveInfo.totalCoins = saveInfo.totalCoins + 1;
        coinCountObject.GetComponent<UnityEngine.UI.Text>().text = Coins.ToString();
        ClockText.text = DefaultTime.ToString();
        StartCoroutine(CoinHit(other.gameObject)); 

        if (Coins > TrophyCount) {
            SetTrophyCount(Coins);
        }
    }

    public void ChangeCoinValue(int increment) {
        Coins += increment;
        coinCountObject.GetComponent<UnityEngine.UI.Text>().text = Coins.ToString();
        ClockText.text = DefaultTime.ToString();

        if (Coins > TrophyCount) {
            SetTrophyCount(Coins);
        }
    }

    IEnumerator ReduceTime(int time) { //Runs a loop
        int CurrentTime = int.Parse(ClockText.text);
        int NewTime = CurrentTime - 1;

        if (NewTime > 0) {
            ClockText.text = NewTime.ToString();
            IncreaseSavedTime();
        } else {
            StartCoroutine(GameOver());
        }

        yield return new WaitForSeconds(time);
        
        StartCoroutine(ReduceTime(time));
    }

    IEnumerator GameOver() {
        GameOverAnimator.SetTrigger("GameOverTrigger");

        yield return new WaitForSeconds(GameOverScreenTime);

        SceneManager.LoadScene(0); //Main Menu
    }

    void CrazyMode() {
        if (CrazyModeIsActive == false) {
            return;
        }

        int NewSpinSpeed = Random.Range(-MaxCrazyModeSpeed, MaxCrazyModeSpeed + 1);
        WorldRotationSpeed = NewSpinSpeed;
        int NewSpeedRandom = Random.Range(1, MaxCrazyModeTimeInterval + 1);
        float NewSpeedDuration = NewSpeedRandom / 10;
        StartCoroutine(WaitLoopCrazyMode(NewSpeedDuration));
    }

    IEnumerator WaitLoopCrazyMode(float time) {
        yield return new WaitForSeconds(time);
        CrazyMode();
    }

    void RestartCrazyMode() {
        CrazyModeIsActive = true;
        CrazyMode();
    }

    void SetTrophyCount(int amount) {
        if (saveInfo.trophy > amount) {
            TrophyCount = saveInfo.trophy;
        } else {
            TrophyCount = amount;
        }
        TrophyCountObject.text = TrophyCount.ToString();

        //Add to save Data
        saveInfo.trophy = TrophyCount;
        saveData(saveInfo);
    }

    public int getCoins()
    {
        return Coins;
    }

}