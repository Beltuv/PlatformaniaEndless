using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Platforms")) {
            GameObject.Find("RainParent").GetComponent<RainManager>().activeRainDrops -= 1;
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Player") {
            GameObject.Find("Player").GetComponent<MovementScript>().RainImpulseDown();
            GameObject.Find("RainParent").GetComponent<RainManager>().activeRainDrops -= 1;
            Destroy(this.gameObject);
        }
    }

    //in case the rain dodges through any platform TODO
}
