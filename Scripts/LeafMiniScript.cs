using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafMiniScript : MonoBehaviour
{
    

    public float LifeTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitToDestroy());
    }
    
    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            GameObject.Find("LeafParent").GetComponent<LeafManager>().TriggerLeafUI();
        }
    }

    IEnumerator WaitToDestroy() {
        yield return new WaitForSeconds(LifeTime);
        GameObject.Find("LeafParent").GetComponent<LeafManager>().activeLeaves = GameObject.Find("LeafParent").GetComponent<LeafManager>().activeLeaves - 1;
        Destroy(this.gameObject);
    }
}
