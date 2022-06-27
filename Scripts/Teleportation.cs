using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    //Nearly everything is hard coded in TeleportPlayer().
    public Transform playerTransform;
    public GameObject ExitObject;
    public float ExitHeightOffset;
    public Vector3 Location1;
    public Vector3 Location2;
    public Vector3 Location3;
    public Vector3 Location4;
    public Vector3 Location5;
    Vector3 blankVector3 = new Vector3(0f, 0f, 0f);

    public void TeleportPlayer() {

        Rigidbody2D rb = playerTransform.gameObject.GetComponent<Rigidbody2D>();

        int LocationNumber = Random.Range(1, 6); //second param is # of locations + 1

        if (ExitObject == null) {
            if (LocationNumber == 1) {
                if (Location1 != blankVector3) {
                    playerTransform.position = Location1;
                    KillVelocity(rb);
                }
                else {
                    TeleportPlayer();
                }
            }
            if (LocationNumber == 2) {
                if (Location2 != blankVector3) {
                    playerTransform.position = Location2;
                    KillVelocity(rb);
                }
                else {
                    TeleportPlayer();
                }
            }
            if (LocationNumber == 3) {
                if (Location3 != blankVector3) {
                    playerTransform.position = Location3;
                    KillVelocity(rb);
                }
                else {
                    TeleportPlayer();
                }
            }
            if (LocationNumber == 4) {
                if (Location4 != blankVector3) {
                    playerTransform.position = Location4;
                    KillVelocity(rb);
                }
                else {
                    TeleportPlayer();
                }
            }
            if (LocationNumber == 5) {
                if (Location5 != blankVector3) {
                    playerTransform.position = Location5;
                    KillVelocity(rb);
                }
                else {
                    TeleportPlayer();
                }
            }
        } else {

            Transform ExitTransform = ExitObject.GetComponent<Transform>();

            playerTransform.position = new Vector3 (ExitTransform.position.x, 
            ExitTransform.position.y + ExitHeightOffset, ExitTransform.position.z);
        }
    }

    void KillVelocity(Rigidbody2D rb) {
        rb.velocity = Vector3.zero;
    }
}
