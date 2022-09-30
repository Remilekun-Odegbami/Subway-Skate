using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform followPlayer; // player
    public Vector3 offset = new Vector3(0, 2.5f, -4.0f);
                             
    // Start is called before the first frame update
    void Start()
    {
        transform.position = followPlayer.position + offset;
    }

    // LateUpdate is called after the player has moved
    void LateUpdate()
    {
        // where the player should be
        Vector3 desiredPosition = followPlayer.position + offset;
        desiredPosition.x = 0;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
    }
}
