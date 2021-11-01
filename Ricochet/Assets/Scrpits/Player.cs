using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject playerObj;
    // Player's speed
    [Range(0, 50)]
    public float speed = 10;

    // Player's sensitivity
    [Range(0, 10)]
    public float sens = 5;
    float sensMod = 25;

    public float jumpForce = 10;

    int invert = -1;
    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Forward
        if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") > 0)
        {
            playerObj.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        // Backward
        if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") < 0)
        {
            playerObj.transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        // Right
        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
        {
            playerObj.transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        // Left
        if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
        {
            playerObj.transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            playerObj.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Horizontal mouse movement
        playerObj.transform.Rotate(new Vector3(0, Input.GetAxisRaw("Mouse X"), 0) * sens * sensMod * Time.deltaTime);

        // Vertical mouse movement
        playerObj.GetComponentInChildren<Camera>().transform.Rotate(new Vector3(Input.GetAxisRaw("Mouse Y"), 0, 0) * sens * sensMod * Time.deltaTime * invert);
    }
}
