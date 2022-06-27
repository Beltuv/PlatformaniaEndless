using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafUI : MonoBehaviour
{
    public float startSize;
    public float sizeLoopCount;
    public float timeBetweenExpansions;


    //private vars
    private bool shield = false;
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(wait5());
    }

    public void grow() {
        for (int i = 0; i < sizeLoopCount; i++) {
            if (shield == false) {
                shield = true;
                float newWidth = this.gameObject.GetComponent<RectTransform>().sizeDelta.x + 5;
                float newHeight = this.gameObject.GetComponent<RectTransform>().sizeDelta.y + 5;
                this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, newHeight);
                StartCoroutine(waitForShieldReset());
            }
        }  
    }

    void Update () {
        Debug.Log(shield);
    }

    IEnumerator waitForShieldReset () {
        yield return new WaitForSeconds(timeBetweenExpansions);
        shield = false;
    }

    IEnumerator wait5 () {
        yield return new WaitForSeconds(5);
        grow();
    }
}
