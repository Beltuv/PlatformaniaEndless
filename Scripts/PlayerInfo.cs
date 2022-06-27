using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SaveNamespace;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    public GameObject CoinsInfoObject;
    public GameObject GhostCoinsInfoObject;
    public GameObject DistInfoObject;
    public GameObject JumpsInfoObject;
    public GameObject TimeInfoObject;
    public GameObject PlayerSkillObject;

    public bool WaitingForGameCountUpdate;

    SaveInfo saveInfo = new SaveInfo();

    void Start() {
        WaitingForGameCountUpdate = false;
    }

    public void CallWaitForGCU() {
        WaitingForGameCountUpdate = true;
    }

    public void loadData() {
        string Readjson = File.ReadAllText(Application.dataPath + "/saveFile.json");
        saveInfo = JsonUtility.FromJson<SaveNamespace.SaveInfo>(Readjson);
    }

    public void LoadUI() { //Loads from save file only
        if (CoinsInfoObject) {
            CoinsInfoObject.transform.Find("InfoBox").GetComponent<TMP_Text>().text = saveInfo.totalCoins.ToString();
        }
        if (GhostCoinsInfoObject) {
            GhostCoinsInfoObject.transform.Find("InfoBox").GetComponent<TMP_Text>().text = saveInfo.totalGhostCoins.ToString();
        }
        if (DistInfoObject) {
            DistInfoObject.transform.Find("InfoBox").GetComponent<TMP_Text>().text = saveInfo.totalDist.ToString();
        }
        if (JumpsInfoObject) {
            JumpsInfoObject.transform.Find("InfoBox").GetComponent<TMP_Text>().text = saveInfo.totalJumps.ToString();
        }
        if (TimeInfoObject) {
            TimeInfoObject.transform.Find("InfoBox").GetComponent<TMP_Text>().text = saveInfo.totalTime.ToString();
        }
        if (PlayerSkillObject) {
            PlayerSkillObject.transform.Find("InfoBox").GetComponent<TMP_Text>().text = statFormulaCalculator().ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        loadData();
        LoadUI();
    }

    public int statFormulaCalculator() {
        //Changeable Constants
        float badTraitFactor = 0.01f;

        float coins = saveInfo.totalCoins;
        float GCoins = saveInfo.totalGhostCoins;
        float dist = saveInfo.totalDist;
        float jumps = saveInfo.totalJumps;
        float time = saveInfo.totalTime;
        float deaths = saveInfo.totalDeaths;
        float games = saveInfo.totalGames;

        /*Debug.Log(coins);
        Debug.Log(GCoins);
        Debug.Log(dist);
        Debug.Log(jumps);
        Debug.Log(time);
        Debug.Log(deaths);
        Debug.Log(games);*/

        float tempSkill = 0f;
        //calculation
        float CpD, GCpD, JpD;
        float CpG, GCpG, DpG, JpG, TpG, DEApG;
        float DpT;
        if (dist != 0) {
            CpD = coins / dist;
            GCpD = GCoins / dist;
            JpD = jumps / dist;
        } else {
            CpD = 0;
            GCpD = 0;
            JpD = 0;
        }
        if (games != 0) {
            CpG = coins / games;
            GCpG = GCoins /games;
            DpG = dist / games;
            JpG = jumps / games;
            TpG = time / games;
            DEApG = deaths / games;
        } else {
            CpG = 0;
            GCpG = 0;
            DpG = 0;
            JpG = 0;
            TpG = 0;
            DEApG = 0;
        }
        if (time != 0) {
            DpT = dist / time;
        } else {
            DpT = 0;
        }
        float goodTrait = CpD * GCpD * CpG * GCpG * DpG * JpG * TpG * DpT; //bigger = better
        float badTrait = JpD * DEApG; //smaller = better
        if (badTrait != 0) {
            tempSkill = Mathf.Sqrt(goodTrait) / (badTrait * badTraitFactor);
        } else  {
            tempSkill = 0f;
        }
        int intSkill = Mathf.RoundToInt(tempSkill);
        return intSkill;
    }
}
