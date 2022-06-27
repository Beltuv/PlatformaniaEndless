using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float LifeTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitToDestroy());
    }

    IEnumerator WaitToDestroy() {
        yield return new WaitForSeconds(LifeTime);
        Destroy(this.gameObject);
    }
}
