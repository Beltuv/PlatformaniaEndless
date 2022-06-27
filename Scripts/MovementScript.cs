using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementScript : MonoBehaviour
{
    [SerializeField] private LayerMask platformsLayerMask;
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider2D;
    public GameObject respawnObject;
    public float moveForce; ///
    public float maxSpeed;
    public float jumpPower = 5f;
    public float GroundDetectionHeight;
    public bool FreezeMovement = false;
    public float SnapFactor;
       // Start is called before the first frame update
    void Start()
    {
        FreezeMovement = false;
      rb = GetComponent<Rigidbody2D>();
      boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f); ///
        //transform.position += movement * Time.deltaTime * moveSpeed; ///

        if (Input.GetButtonDown("Jump")) { //Checks if already jumping
            if (FreezeMovement == false) {
                Jump();
            }
        }
    }

    void FixedUpdate() {
        float movementXInput = Input.GetAxis("Horizontal");
        if (rb.velocity.magnitude < maxSpeed && FreezeMovement == false) {
            Vector2 movement = new Vector2(RoundMovementAxis(movementXInput), 0);
            rb.AddForce(moveForce * movement);
            //Debug.Log(RoundMovementAxis(movementXInput));
        }
    }

    float RoundMovementAxis(float input) {
        float returnVal = Mathf.Ceil(input * SnapFactor * 10f) * 0.1f;
        if (returnVal > 1f) {
            returnVal = 1f;
        }
        if (returnVal < -1f) {
            returnVal = -1f;
        }
        return returnVal;
    }

    void Jump() {
        if (IsGrounded()) {
          gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
          respawnObject.transform.position = rb.gameObject.transform.position; 
          gameObject.GetComponent<GameEffect>().IncreaseSavedJumps();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Teleporter") {
            other.gameObject.GetComponent<Teleportation>().TeleportPlayer();
        }
    }

    private bool IsGrounded() {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, GroundDetectionHeight, platformsLayerMask);
        return raycastHit2D.collider != null;
    }

    //Called from a raincollision script
    public void RainImpulseDown() {
        float RainForce = GameObject.Find("RainParent").GetComponent<RainManager>().rainForce;
        Vector3 forceDirection = new Vector3(0, -RainForce, 0);
        rb.AddForce(forceDirection, ForceMode2D.Impulse);
    }
}
