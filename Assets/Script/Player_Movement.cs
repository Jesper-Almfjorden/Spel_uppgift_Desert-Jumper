using UnityEngine;
using System.Collections;

public class Player_Movement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpPower = 10f;
    public bool grounded = false;

    void Update()
    {
        if (!grounded && GetComponent<Rigidbody2D>().velocity.y == 0)
        {
            grounded = true;
        }
        if (Input.GetKeyDown(KeyCode.W) && grounded == true)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpPower);
            grounded = false;
        }
        //Next two if statements are for moving left and right
        if (Input.GetKey(KeyCode.D))
            transform.Translate(new Vector2(1, 0) * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
            transform.Translate(new Vector2(-1, 0) * moveSpeed * Time.deltaTime);
    }

}
