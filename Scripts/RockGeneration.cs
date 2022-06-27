using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGeneration : MonoBehaviour
{
    List<GameObject> RocksList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) {
            if (child.gameObject.tag == "Rock") {
                RocksList.Add(child.gameObject);
            }
        }

        int RockIndex = Random.Range(0, RocksList.Count-1);
        GameObject KeptRock = RocksList[RockIndex];
        foreach (GameObject rock in RocksList) {
            if (rock != KeptRock) {
                Destroy(rock);
            }
        }
        Color rc = KeptRock.GetComponent<SpriteRenderer>().color;
        KeptRock.GetComponent<SpriteRenderer>().color = new Color(rc.r, rc.g, rc.b, 1f);
    }

}
