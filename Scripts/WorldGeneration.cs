using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{ 
    private const float SpawnChunkDistance = 80f;
    private const float DeleteChunkDistance = 90f; //larger for when player falls into the void

    private const int amountOfChunkPositions = 3;

    [SerializeField] private Transform levelStart;
    [SerializeField] private Transform chunk;
    [SerializeField] private GameObject player;

    private Vector3 lastRightPosition;
    private Vector3 lastLeftPosition;
    private GameObject DeletedChunk; //Used in the update method when chunk is being deleted
    private int ChunkNumber; //total # of generated chunks

    List<GameObject> chunksList = new List<GameObject>();
    List<GameObject> GrassChunkPrefabs = new List<GameObject>();
    List<GameObject> WinterChunkPrefabs = new List<GameObject>();
    List<GameObject> FallChunkPrefabs = new List<GameObject>();
    List<GameObject> ForestChunkPrefabs = new List<GameObject>();
    List<int> GrassChunkFrequencies = new List<int>(); //indexes of these should match up with chunkPrefabs
    List<int> WinterChunkFrequencies = new List<int>();
    List<int> FallChunkFrequencies = new List<int>();
    List<int> ForestChunkFrequencies = new List<int>();
    Object[] subChunkPrefabs;
    Object[] subWinterChunkPrefabs;
    Object[] subFallChunkPrefabs;
    Object[] subForestChunkPrefabs;
    Object[] subAnyChunkPrefabs;

    //Biome Configuration
    //Biome IDs:
     //Grass(grass), Winter(winter)

    //biome variables
    private float minBiomeSize = 25f;
    private float maxBiomeSize = 50f;

    //biome visuals
    Color32 RightShadowColor = new Color32(0, 0, 0, 76);
    Color32 WinterTopColor = new Color32(241, 241, 241, 255);
    Color32 WinterFrontColor = new Color32(183, 183, 183, 255);
    Color32 FallTopColor = new Color32(221, 185, 18, 255); //before color method
    Color32 FallFrontColor = new Color32(178, 126, 19, 255); // before color method
    //DarkFallTopColor: (221, 105, 18, 255)
    //DarkFallFrontColor: (191, 75, 18, 255)
    Color32 ForestTopColor = new Color32(4, 156, 48, 255);
    Color32 ForestFrontColor = new Color32(12, 75, 25, 255);

    //non-changeable biome vars
    public string[] biomes = {"grass", "winter", "fall", "forest"};
    string defaultBiome; //usually get sets to biomes[0] in start
    int leftBiomeBound; //should be set by SetBiomeBoundary()
    int rightBiomeBound; //should be set by SetBiomeBoundary()

    public GameObject GrassBG;
    public GameObject WinterBG;
    public GameObject FallBG;
    public GameObject ForestBG;

    public struct Biostruct {
        public string biomeID;
        public float LBound;
        public float RBound;

        public Biostruct (string ID, float LB, float RB) {
            biomeID = ID;
            LBound = LB;
            RBound = RB;
        }
    }

    public Biostruct[] activeBiomes = new Biostruct[3];

    private void Awake() {
        //loading chunk prefabs
        subChunkPrefabs = Resources.LoadAll("ChunkPrefabs/GrassChunks", typeof(GameObject)); //Takes all prefabs in the chunk prefab folder and put them into the chunkprefab list
            foreach(GameObject subChunkPrefabs in subChunkPrefabs) {
                GameObject lo = (GameObject)subChunkPrefabs;
                /* //test specific chunks use this if statement (temp/delete after)
                if (lo.name == "Chunk5" || lo.name == "Chunk4") {
                    chunkPrefabs.Add(lo);
                }
                */
                GrassChunkPrefabs.Add(lo);
                Debug.Log(lo);
                //Name: "Chunk#-SpawnFrequency"
                //Example: "Chunk03-20"
                // 20
                string nameOfChunk = lo.name;
                string[] divider = nameOfChunk.Split("-");
                //divider[1] is the 2nd half of the name split at the -
                int Frequency;
                int.TryParse(divider[1], out Frequency);
                GrassChunkFrequencies.Add(Frequency);
            } 

        subWinterChunkPrefabs = Resources.LoadAll("ChunkPrefabs/WinterChunks", typeof(GameObject)); //Takes all prefabs in the chunk prefab folder and put them into the chunkprefab list
            foreach(GameObject WinterSCP in subWinterChunkPrefabs) {
                GameObject lo = (GameObject)WinterSCP;
                /* //test specific chunks use this if statement (temp/delete after)
                if (lo.name == "Chunk5" || lo.name == "Chunk4") {
                    chunkPrefabs.Add(lo);
                }
                */
                WinterChunkPrefabs.Add(lo);
                Debug.Log(lo);
                //Name: "Chunk#-SpawnFrequency"
                //Example: "Chunk03-20"
                // 20
                string nameOfChunk = lo.name;
                string[] divider = nameOfChunk.Split("-");
                //divider[1] is the 2nd half of the name split at the -
                int Frequency;
                int.TryParse(divider[1], out Frequency);
                WinterChunkFrequencies.Add(Frequency);
            }

        subFallChunkPrefabs = Resources.LoadAll("ChunkPrefabs/FallChunks", typeof(GameObject)); //Takes all prefabs in the chunk prefab folder and put them into the chunkprefab list
            foreach(GameObject FallSCP in subFallChunkPrefabs) {
                GameObject lo = (GameObject)FallSCP;
                /* //test specific chunks use this if statement (temp/delete after)
                if (lo.name == "Chunk5" || lo.name == "Chunk4") {
                    chunkPrefabs.Add(lo);
                }
                */
                FallChunkPrefabs.Add(lo);
                Debug.Log(lo);
                //Name: "Chunk#-SpawnFrequency"
                //Example: "Chunk03-20"
                // 20
                string nameOfChunk = lo.name;
                string[] divider = nameOfChunk.Split("-");
                //divider[1] is the 2nd half of the name split at the -
                int Frequency;
                int.TryParse(divider[1], out Frequency);
                FallChunkFrequencies.Add(Frequency);
            }

        subForestChunkPrefabs = Resources.LoadAll("ChunkPrefabs/ForestChunks", typeof(GameObject)); //Takes all prefabs in the chunk prefab folder and put them into the chunkprefab list
            foreach(GameObject ForestSCP in subForestChunkPrefabs) {
                GameObject lo = (GameObject)ForestSCP;
                /* //test specific chunks use this if statement (temp/delete after)
                if (lo.name == "Chunk5" || lo.name == "Chunk4") {
                    chunkPrefabs.Add(lo);
                }
                */
                ForestChunkPrefabs.Add(lo);
                Debug.Log(lo);
                //Name: "Chunk#-SpawnFrequency"
                //Example: "Chunk03-20"
                // 20
                string nameOfChunk = lo.name;
                string[] divider = nameOfChunk.Split("-");
                //divider[1] is the 2nd half of the name split at the -
                int Frequency;
                int.TryParse(divider[1], out Frequency);
                FallChunkFrequencies.Add(Frequency);
            }

        subAnyChunkPrefabs = Resources.LoadAll("ChunkPrefabs/AllBiomeChunks", typeof(GameObject)); //Takes all prefabs in the chunk prefab folder and put them into the chunkprefab list
            foreach(GameObject AnySCP in subAnyChunkPrefabs) {
                GameObject lo = (GameObject)AnySCP;
                //Add section for each biome
                //Grass
                GameObject GrassVersion = Instantiate(lo);
                GrassChunkPrefabs.Add(GrassCopy(GrassVersion));
                //Name: "Chunk#-SpawnFrequency"
                //Example: "Chunk03-20"
                // 20
                string nameOfChunk = lo.name;
                string[] divider = nameOfChunk.Split("-");
                //divider[1] is the 2nd half of the name split at the -
                int Frequency;
                int.TryParse(divider[1], out Frequency);
                GrassChunkFrequencies.Add(Frequency);
                //Winter
                GameObject WinterVersion = Instantiate(lo);
                WinterChunkPrefabs.Add(WinterCopy(WinterVersion));
                WinterChunkFrequencies.Add(Frequency);

                GameObject FallVersion = Instantiate(lo);
                FallChunkPrefabs.Add(FallCopy(FallVersion));
                FallChunkFrequencies.Add(Frequency);

                GameObject ForestVersion = Instantiate(lo);
                ForestChunkPrefabs.Add(ForestCopy(ForestVersion));
                ForestChunkFrequencies.Add(Frequency);

                //Disable temp game objects used to populate the biome prefabs lists
                GrassVersion.transform.position = new Vector3(0, 1000, 0);
                WinterVersion.transform.position = new Vector3(0, 1000, 0);
                FallVersion.transform.position = new Vector3(0, 1000, 0);
                ForestVersion.transform.position = new Vector3(0, 1000, 0);
            }

        lastRightPosition = levelStart.position;
        lastLeftPosition = levelStart.position;
        chunksList.Add(levelStart.gameObject);
        
        //biome init
        defaultBiome = biomes[1];
        SetActiveBiomes();
        DebugNearBiomes();

        //
        SpawnRightChunk();
        SpawnRightChunk();
        SpawnRightChunk();
        SpawnLeftChunk();
        SpawnLeftChunk();
        SpawnLeftChunk();
    }

    private void Update() {
    
       if (Vector3.Distance(player.transform.position, lastRightPosition) < SpawnChunkDistance) { //Spawn next chunk (right)
           SpawnRightChunk();
       }
       if (Vector3.Distance(player.transform.position, lastLeftPosition) < SpawnChunkDistance) { //Spawn next chunk (right)
           SpawnLeftChunk();
       }
       if (Vector3.Distance(player.transform.position, lastLeftPosition) > DeleteChunkDistance) { //Remove old chunks(removes left-most chunk)
            //index 0 is the left-most chunk
            lastLeftPosition = chunksList[1].transform.position;
            DeletedChunk = chunksList[0];
            chunksList.RemoveAt(0); //removes left most chunk from chunk list
            Destroy(DeletedChunk);
       }
       if (Vector3.Distance(player.transform.position, lastRightPosition) > DeleteChunkDistance) { //Remove old chunks(removes right-most chunk)
            //index 0 is the left-most chunk
            lastRightPosition = chunksList[chunksList.Count-2].transform.position; //-2 to get the second last element
            DeletedChunk = chunksList[chunksList.Count-1];
            Debug.Log(DeletedChunk);
            chunksList.RemoveAt(chunksList.Count-1); //removes right most chunk from chunk list
            Destroy(DeletedChunk);
       }

       //Biome Field Detection
       if (player.transform.position.x > activeBiomes[1].RBound) {
           AddRightBiome();
           DebugNearBiomes();
       }
       if (player.transform.position.x < activeBiomes[1].LBound) {
           AddLeftBiome();
           DebugNearBiomes();
       } 
    }

    //Biome Methods

    //Fall Color Methods
    //NotDoneYet

    //Platform Biome Methods
    GameObject WinterCopy (GameObject og) {
        //take the grass version and return the winter version
        foreach (Transform child in og.transform) {
            if (child.gameObject.tag == "Platform") {
                foreach (Transform platformChild in child) {
                    if (platformChild.gameObject.name == "TopColor") {
                        platformChild.gameObject.GetComponent<SpriteRenderer>().color = WinterTopColor;
                    } else if (platformChild.gameObject.name == "FrontColor") {
                        platformChild.gameObject.GetComponent<SpriteRenderer>().color = WinterFrontColor;
                    } else if (platformChild.gameObject.name == "RightSideShadow") {
                        platformChild.gameObject.GetComponent<SpriteRenderer>().color = RightShadowColor;
                    }
                }
            }
        }
        return og;
    }

    GameObject FallCopy (GameObject og) {
        //take the grass version and return the fall version
        foreach (Transform child in og.transform) {
            if (child.gameObject.tag == "Platform") {
                foreach (Transform platformChild in child) {
                    if (platformChild.gameObject.name == "TopColor") {
                        platformChild.gameObject.GetComponent<SpriteRenderer>().color = FallTopColor;
                    } else if (platformChild.gameObject.name == "FrontColor") {
                        platformChild.gameObject.GetComponent<SpriteRenderer>().color = FallFrontColor;
                    } else if (platformChild.gameObject.name == "RightSideShadow") {
                        platformChild.gameObject.GetComponent<SpriteRenderer>().color = RightShadowColor;
                    }
                }
            }
        }
        return og;
    }

    GameObject ForestCopy (GameObject og) {
        //take the grass version and return the fall version
        foreach (Transform child in og.transform) {
            if (child.gameObject.tag == "Platform") {
                foreach (Transform platformChild in child) {
                    if (platformChild.gameObject.name == "TopColor") {
                        platformChild.gameObject.GetComponent<SpriteRenderer>().color = ForestTopColor;
                    } else if (platformChild.gameObject.name == "FrontColor") {
                        platformChild.gameObject.GetComponent<SpriteRenderer>().color = ForestFrontColor;
                    } else if (platformChild.gameObject.name == "RightSideShadow") {
                        platformChild.gameObject.GetComponent<SpriteRenderer>().color = RightShadowColor;
                    }
                }
            }
        }
        return og;
    }

     GameObject GrassCopy (GameObject og) {
        //take the grass version and return the winter version
        foreach (Transform child in og.transform) {
            if (child.gameObject.tag == "Platform") {
                foreach (Transform platformChild in child) {
                    if (platformChild.gameObject.name == "TopColor") {
                        platformChild.gameObject.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
                    } else if (platformChild.gameObject.name == "FrontColor") {
                        platformChild.gameObject.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
                    } else if (platformChild.gameObject.name == "RightSideShadow") {
                        platformChild.gameObject.GetComponent<SpriteRenderer>().color = new Color(0,0,0,0);
                    }
                }
            }
        }
        return og;
    }

    void SetActiveBiomes() { //init from origin (use AddBiomeBoundary after startup)
        float leftBound = Random.Range(minBiomeSize, maxBiomeSize) * -1;
        float rightBound = Random.Range(minBiomeSize, maxBiomeSize);

        //origin biome
        Biostruct originBiome = new Biostruct("grass", leftBound, rightBound);
            //add to middle of active biomes
        activeBiomes[1] = originBiome;

        //right
        float RleftBound = Random.Range(minBiomeSize, maxBiomeSize);
        float RrightBound = Random.Range(minBiomeSize, maxBiomeSize);
        float RBiomeSize = RleftBound + RrightBound;
        Biostruct rightBiome = new Biostruct(pickBiomeID(), activeBiomes[1].RBound, activeBiomes[1].RBound + RBiomeSize);
        activeBiomes[2] = rightBiome;
        //left
        float LleftBound = Random.Range(minBiomeSize, maxBiomeSize);
        float LrightBound = Random.Range(minBiomeSize, maxBiomeSize);
        float LBiomeSize = LleftBound + LrightBound;
        Biostruct leftBiome = new Biostruct(pickBiomeID(), activeBiomes[1].LBound - LBiomeSize, activeBiomes[1].LBound);
        activeBiomes[0] = leftBiome;

        UpdateBackground();
    }

    void AddRightBiome () {
        //adjust activeBiomes
        activeBiomes[0] = activeBiomes[1];
        activeBiomes[1] = activeBiomes[2];

        float RleftBound = Random.Range(minBiomeSize, maxBiomeSize);
        float RrightBound = Random.Range(minBiomeSize, maxBiomeSize);
        float RBiomeSize = RleftBound + RrightBound;
        Biostruct rightBiome = new Biostruct(pickBiomeID(), activeBiomes[1].RBound, activeBiomes[1].RBound + RBiomeSize);
        activeBiomes[2] = rightBiome;

        UpdateBackground();
    }
    
    void AddLeftBiome () {
        //adjust activeBiomes
        activeBiomes[2] = activeBiomes[1];
        activeBiomes[1] = activeBiomes[0];

        float LleftBound = Random.Range(minBiomeSize, maxBiomeSize);
        float LrightBound = Random.Range(minBiomeSize, maxBiomeSize);
        float LBiomeSize = LleftBound + LrightBound;
        Biostruct leftBiome = new Biostruct(pickBiomeID(), activeBiomes[1].LBound - LBiomeSize, activeBiomes[1].LBound);
        activeBiomes[0] = leftBiome;

        UpdateBackground();
    }

    string pickBiomeID () {
        return biomes[Random.Range(0, biomes.Length)];
    }

    string whatBiomeIsX (float x) {
        //first check is we know the biome
        if (activeBiomes[0].LBound < x && activeBiomes[2].RBound > x) {
            //we are in known biome region
            //check if we are in activeBiome[0]
            if (activeBiomes[0].LBound < x && activeBiomes[0].RBound > x) {
                //in activeBiome[0]
                return activeBiomes[0].biomeID;
            }
            else if (activeBiomes[1].LBound < x && activeBiomes[1].RBound > x) {
                //in activeBiome[1]
                return activeBiomes[1].biomeID;
            }
            else if (activeBiomes[2].LBound < x && activeBiomes[2].RBound > x) {
                //in activeBiome[2]
                return activeBiomes[2].biomeID;
            }
            else {
                Debug.Log("Lost in biome region");
                return "Lost in biome region";
            }
        }
        else {
            //x is out of known biomes
            Debug.Log(x.ToString() + "-none");
            return "none";
        }
    }

    void DebugNearBiomes() {
        Debug.Log(activeBiomes[0].biomeID + "-0");
        Debug.Log(activeBiomes[1].biomeID + "-1");
        Debug.Log(activeBiomes[2].biomeID + "-2");
    }

    //Biome Background Updater
    void UpdateBackground() {
        if (activeBiomes[1].biomeID == biomes[0]) {
            //should be grass
            //EnableBG(GrassBG);
            //DisableBG(WinterBG);
            GameObject.Find("BackgroundParent").GetComponent<BackgroundScript>().SetGrassBackgrounds();
        }
        if (activeBiomes[1].biomeID == biomes[1]) {
            //should be winter
            //EnableBG(WinterBG);
            //DisableBG(GrassBG);
            GameObject.Find("BackgroundParent").GetComponent<BackgroundScript>().SetWinterBackgrounds();
        }
        if (activeBiomes[1].biomeID == biomes[2]) {
            //should be fall
            GameObject.Find("BackgroundParent").GetComponent<BackgroundScript>().SetFallBackgrounds();
        }
        if (activeBiomes[1].biomeID == biomes[3]) {
            //should be forest
            GameObject.Find("BackgroundParent").GetComponent<BackgroundScript>().SetForestBackgrounds();
        }
    }

    private void DisableBG (GameObject bg) {
        //invisible main
        Color temp = bg.GetComponent<SpriteRenderer>().color;
        temp.a = 0f;
        bg.GetComponent<SpriteRenderer>().color = temp;

        Component[] ChildRenderers = bg.GetComponentsInChildren(typeof(SpriteRenderer));
        if (ChildRenderers != null) {
            foreach (SpriteRenderer sr in ChildRenderers) {
                Color tmp = sr.color;
                tmp.a = 0f;
                sr.color = tmp;
            }
        }
    }

    private void EnableBG (GameObject bg) {
        //invisible main
        Color temp = bg.GetComponent<SpriteRenderer>().color;
        temp.a = 255f;
        bg.GetComponent<SpriteRenderer>().color = temp;

        Component[] ChildRenderers = bg.GetComponentsInChildren(typeof(SpriteRenderer));
        if (ChildRenderers != null) {
            foreach (SpriteRenderer sr in ChildRenderers) {
                Color tmp = sr.color;
                tmp.a = 255f;
                sr.color = tmp;
            }
        }
    }

    private void newChunk(float x) {
        //sum frequencies and assign regions then choose out of the sum of frequencies to find the next chunk considering spawn frequencies
        //Grass
        if (whatBiomeIsX(x) == biomes[0]) {
                //Obtain Frequency Sum
            int frequencySum = 0;
            foreach(int num in GrassChunkFrequencies) {
                frequencySum = frequencySum + num;
            }
            int choice = Random.Range(0, frequencySum);
            int totalSeen = 0;
            bool chunkFound = false;
            for (int i=0; i < GrassChunkFrequencies.Count; i++) {
                totalSeen = totalSeen + GrassChunkFrequencies[i];
                if (totalSeen >= choice && chunkFound == false) {
                    //chunk found
                    chunkFound = true; //shield
                    chunk = GrassChunkPrefabs[i].transform; //chosen chunk
                }
            }
        }
        if (whatBiomeIsX(x) == biomes[1]) {
                //Obtain Frequency Sum
            int frequencySum = 0;
            foreach(int num in WinterChunkFrequencies) {
                frequencySum = frequencySum + num;
            }
            int choice = Random.Range(0, frequencySum);
            int totalSeen = 0;
            bool chunkFound = false;
            for (int i=0; i < WinterChunkFrequencies.Count; i++) {
                totalSeen = totalSeen + WinterChunkFrequencies[i];
                if (totalSeen >= choice && chunkFound == false) {
                    //chunk found
                    chunkFound = true; //shield
                    chunk = WinterChunkPrefabs[i].transform; //chosen chunk
                }
            }
        }
        if (whatBiomeIsX(x) == biomes[2]) {
                //Obtain Frequency Sum
            int frequencySum = 0;
            foreach(int num in FallChunkFrequencies) {
                frequencySum = frequencySum + num;
            }
            int choice = Random.Range(0, frequencySum);
            int totalSeen = 0;
            bool chunkFound = false;
            for (int i=0; i < FallChunkFrequencies.Count; i++) {
                totalSeen = totalSeen + FallChunkFrequencies[i];
                if (totalSeen >= choice && chunkFound == false) {
                    //chunk found
                    chunkFound = true; //shield
                    chunk = FallChunkPrefabs[i].transform; //chosen chunk
                }
            }
        }
        if (whatBiomeIsX(x) == biomes[3]) {
                //Obtain Frequency Sum
            int frequencySum = 0;
            foreach(int num in ForestChunkFrequencies) {
                frequencySum = frequencySum + num;
            }
            int choice = Random.Range(0, frequencySum);
            int totalSeen = 0;
            bool chunkFound = false;
            for (int i=0; i < ForestChunkFrequencies.Count; i++) {
                totalSeen = totalSeen + FallChunkFrequencies[i];
                if (totalSeen >= choice && chunkFound == false) {
                    //chunk found
                    chunkFound = true; //shield
                    chunk = ForestChunkPrefabs[i].transform; //chosen chunk
                }
            }
        }
    }

    /*
    In the spawn chunk procedure, 
    the void is used to choose chunk attachment point and
    the Transform function is for choosing where to attach to
    */

    private void SpawnRightChunk() {
        newChunk(lastRightPosition.x);
        Transform lastChunkTransform = SpawnRightChunk(lastRightPosition);

        //Coins
        List<GameObject> coinsList = new List<GameObject>(); //current coins on chunk being generated
        //Find all coins in current chunk
        foreach(Transform t in lastChunkTransform) {
           if (t.gameObject.tag == "Coins") {
               coinsList.Add(t.gameObject);
           }
        }
        //Pick a coin
        int randomCoinIndex = Random.Range(0, coinsList.Count);
        for (int i=0; i < coinsList.Count; i++) {
            if (i != randomCoinIndex) {
                Destroy(coinsList[i]);
            }
        }

        int positionNum = Random.Range(1, amountOfChunkPositions);
        if (lastChunkTransform.Find("RightPosition" + positionNum.ToString()) != null) {
            lastRightPosition = lastChunkTransform.Find("RightPosition" + positionNum.ToString()).position;
        }
        else {
            //no other end positions (default to Position1)
            lastRightPosition = lastChunkTransform.Find("RightPosition1").position;
        }
    }

    private Transform SpawnRightChunk(Vector3 spawnPosition) {
        Transform chunkTransform = Instantiate(chunk, spawnPosition, Quaternion.identity);
        chunkTransform.position = new Vector3(chunkTransform.position.x, chunkTransform.position.y, 0); //set z-value so platform is infront of the background
        chunksList.Add(chunkTransform.gameObject); //adds chunk to end of list
        ChunkNumber++;
        chunkTransform.gameObject.name = "Chunk+" + ChunkNumber.ToString();
        return chunkTransform;
    }

    private void SpawnLeftChunk() {
        newChunk(lastLeftPosition.x);
        Transform lastChunkTransform = SpawnLeftChunk(lastLeftPosition);

        //Coins
        //Coins
        List<GameObject> coinsList = new List<GameObject>(); //current coins on chunk being generated
        //Find all coins in current chunk
        foreach(Transform t in lastChunkTransform) {
           if (t.gameObject.tag == "Coins") {
               coinsList.Add(t.gameObject);
           }
        }
        //Pick a coin
        int randomCoinIndex = Random.Range(0, coinsList.Count);
        for (int i=0; i < coinsList.Count; i++) {
            if (i != randomCoinIndex) {
                Destroy(coinsList[i]);
            }
        }

        int positionNum = Random.Range(1, amountOfChunkPositions);
        if (lastChunkTransform.Find("LeftPosition" + positionNum.ToString()) != null) {
            lastLeftPosition = lastChunkTransform.Find("LeftPosition" + positionNum.ToString()).position;
        }
        else {
            //no other end positions (default to Position1)
            lastLeftPosition = lastChunkTransform.Find("LeftPosition1").position;
        }
    }

    private Transform SpawnLeftChunk(Vector3 spawnPosition) {
        //Find Left Position by taking the difference of Vector3s and adding the difference to the spawn point(allowing an offset so the chunk spawns in the left direction)
        Vector3 leftPos = chunk.Find("LeftPosition1").position;
        //pick RightPosition that attaches to previous chunk
        Vector3 rightPos;
        int positionNum = Random.Range(1, amountOfChunkPositions);
        if (chunk.Find("RightPosition" + positionNum.ToString()) != null) {
            rightPos = chunk.Find("RightPosition" + positionNum.ToString()).position;
        }
        else {
            //default to RightPosition1 if nothing is found
            rightPos = chunk.Find("RightPosition1").position;
        }
        Vector3 differenceInPos = leftPos - rightPos;

        Transform chunkTransform = Instantiate(chunk, spawnPosition+differenceInPos, Quaternion.identity);
        chunkTransform.position = new Vector3(chunkTransform.position.x, chunkTransform.position.y, 0); //set z-value so platform is infront of the background
        chunksList.Insert(0, chunkTransform.gameObject); //Adds chunk to left of list
        ChunkNumber++;
        chunkTransform.gameObject.name = "Chunk-" + ChunkNumber.ToString();
        /*Debug.Log(spawnPosition);
        Debug.Log(differenceInPos);
        Debug.Log(chunkTransform.position);
        */
        return chunkTransform;
    }
}
