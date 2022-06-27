using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
    //Objects/Parents we are going to activate and disable when they are in biome
    public GameObject WindParent;
    public GameObject SnowParent;
    public GameObject RainParent;
    public GameObject LeafParent;

    public float HazardUpdateFrequency;

    // Start is called before the first frame update
    void Start()
    {
        WindParent.SetActive(false);
        SnowParent.SetActive(false);
        RainParent.SetActive(false);
        LeafParent.SetActive(false);
        UpdateActiveHazard();
        StartCoroutine(HazardTimer());
    }

    void UpdateActiveHazard() {
        if (GameObject.Find("WorldGeneration").GetComponent<WorldGeneration>().activeBiomes[1].biomeID == "grass") {
            WindParent.SetActive(true);
            SnowParent.SetActive(false);
            RainParent.SetActive(false);
            LeafParent.SetActive(false);
        } 
        if (GameObject.Find("WorldGeneration").GetComponent<WorldGeneration>().activeBiomes[1].biomeID == "winter") {
            WindParent.SetActive(false);
            SnowParent.SetActive(true);
            RainParent.SetActive(false);
            LeafParent.SetActive(false);
        }
        if (GameObject.Find("WorldGeneration").GetComponent<WorldGeneration>().activeBiomes[1].biomeID == "forest") {
            WindParent.SetActive(false);
            SnowParent.SetActive(false);
            RainParent.SetActive(true);
            LeafParent.SetActive(false);
        } 
        if (GameObject.Find("WorldGeneration").GetComponent<WorldGeneration>().activeBiomes[1].biomeID == "fall") {
            WindParent.SetActive(false);
            SnowParent.SetActive(false);
            RainParent.SetActive(false);
            LeafParent.SetActive(true);
        }
    }

    IEnumerator HazardTimer() {
        yield return new WaitForSeconds(HazardUpdateFrequency);
        UpdateActiveHazard();
        RestartHazardTimer();
    }

    void RestartHazardTimer() {
        StartCoroutine(HazardTimer());
    }
}
