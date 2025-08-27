using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float offset;
    public float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private Transform target;
    private Vector3 playerPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player == null)
        {
            Debug.LogError("CameraController: Player GameObject is not assigned.");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player != null)
        {
            // Get position of player in x and y
            playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + offset, transform.position.z);

            //set camera position
            transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, smoothTime);

            //transform.position = playerPosition;   
        }
    }
}
