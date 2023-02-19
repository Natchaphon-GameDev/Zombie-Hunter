using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class CameraFollow : MonoBehaviour {
 
    public Transform target;
 
    public float smoothSpeed;
    public Vector3 offset;
    // private float minRoate = -7;
    // private float maxRoate = 4;
 
    private void LateUpdate()
    {
        if (!GameManager.isGameStart)
        {
            return;
        }
        
        if (State.isGameEnd)
        {
            return;
        }
        
        //A - , D +
        // var horizontalInput = Input.GetAxis("Horizontal");
        //
        // if (horizontalInput < 0 && offset.x >= minRoate)
        // {
        //     offset.x -= Time.deltaTime;
        // }
        // else if (horizontalInput > 0 && offset.x <= maxRoate)
        // {
        //     offset.x += Time.deltaTime;
        // }
       
        var desiredPosition = target.position + offset;
        var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
 
        transform.LookAt(target);
    }
 
}
