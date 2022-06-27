using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveNamespace 
{
    //Save Data Class
    public class SaveInfo {
        public int trophy;
        public string activeColor = "default";
        public Color32 defaultColor = new Color32(255, 161, 121, 255); //id: default
        public Color32 redColor = new Color32(255, 81, 76, 255); //id: red
        public Color32 blueColor = new Color32(35, 159, 255, 255); //id: blue
        public Color32 greenColor = new Color32(0, 140, 60, 255); //id: green

        //All-Time Player Stats
        public int totalCoins;
        public int totalGhostCoins;
        public int totalDist;
        public int totalJumps;
        public int totalTime;
            //Hidden Player stats
        public int totalDeaths;
        public int totalGames;

        public Color32 getActiveColor() {
            if (activeColor == "red") {
                return redColor;
            }
            else if (activeColor == "blue") {
                return blueColor;
            }
            else if (activeColor == "green") {
                return greenColor;
            }
            else {
                //rely on default if no active color
                return defaultColor;
            }
        }
    }
}

public class SaveScript : MonoBehaviour
{
    void Start() {
        DontDestroyOnLoad(gameObject);
    }
   
}
