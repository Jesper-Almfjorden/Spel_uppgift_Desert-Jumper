using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Parallax scrolling script that should be assigned to a layer
/// </summary>

public class ScrollingScript : MonoBehaviour
{
    /// <summary>
    /// Scrolling speed
    /// </summary>
    public Vector2 speed = new Vector2(10, 10);

    /// <summary>
    /// Moving direction
    /// </summary>
    public Vector2 direction = new Vector2(-1, 0);

    /// <summary>
    /// Movement should be applied to camera
    /// </summary>
    public bool isLinkenToCamera = false;

    /// <summary>
    /// 1 - Background is infinite
    /// </summary>
    public bool isLooping = false;

    /// <summary>
    /// 2 - List of children with a renderer
    /// </summary>
    private List<SpriteRenderer> backgroundPart;

    // 3 - Get all the children

    // Use this for initialization
    void Start()
    {
        // For infinite backgrund only
        if (isLooping)
        {
            // Get all the children of the layer with a renderer
            backgroundPart = new List<SpriteRenderer>();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                SpriteRenderer r = child.GetComponent<SpriteRenderer>();

                // Add only the visible childern
                if (r != null)
                {
                    backgroundPart.Add(r);
                }
            }

            // Sort by position.
            // Note: Get the children from left to right.
            // We would need to add a few conditions to handle
            // all the possible scrolling directions.
            backgroundPart = backgroundPart.OrderBy(
                t => t.transform.position.x
            ).ToList();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        Vector3 movement = new Vector3(
            speed.x * direction.x,
            speed.y * direction.y,
            0);

        movement *= Time.deltaTime;
        transform.Translate(movement);

        // Move the camera
        if (isLinkenToCamera)
        {
            Camera.main.transform.Translate(movement);
        }

        // 4 - Loop
        if (isLooping)
        {
            // Get the first object.
            // The list is ordered from left (x position) to right:
            SpriteRenderer firstChild = backgroundPart.FirstOrDefault();

            if (firstChild != null)
            {
                // Check if the child is already (partly) before the camera.
                // We test the position first because the IsVisibleForm
                // method is a bit heavier to execute.
                if (firstChild.transform.position.y <
                    Camera.main.transform.position.y)
                {
                    // If the child is already on the left of the camera,
                    // we test if it´s completely outside and needs to be
                    // recycled.
                    print("Stage 2");

                    if (firstChild.IsVisibleFrom(Camera.main) == false)
                    {
                        // Get the last child position.
                        SpriteRenderer lastChild =
                            backgroundPart.LastOrDefault();

                        Vector3 lastPosition = lastChild.transform.position;
                        Vector3 lastSize = (lastChild.bounds.max - lastChild.bounds.min);

                        // Set the position of the recyled one to be AFTER
                        // the last child.
                        // Note: Only work for horizontal scrolling currently.
                        firstChild.transform.position = new
                        Vector3(lastPosition.x, firstChild.transform.position.y + lastSize.y * 2,
                                firstChild.transform.position.z);

                        // Set the recycled child to the last position
                        // of the backgroundPart list.
                        backgroundPart.Remove(firstChild);
                        backgroundPart.Add(firstChild);

                        print("Works");

                    }

                }
            }
        }
    }
}