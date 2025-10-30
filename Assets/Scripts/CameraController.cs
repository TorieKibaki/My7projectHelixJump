using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public BallController target; //public variable of type BallController
    private float offset; //initial vertical gap between the camera and the target, used to keep that gap consistent as the target moves.

    // Use this for initialization
    void Awake()
    {
        offset = transform.position.y - target.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 curPos = transform.position;
        curPos.y = target.transform.position.y + offset; // always maintain the distance to the ball
        transform.position = curPos;
    }
}
