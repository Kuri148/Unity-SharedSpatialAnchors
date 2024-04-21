using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
    private float _endX;
    private float _startX;
    private const float TRAVEL_DISTANCE = 10.0f;
    void Start()
    {
        _startX = transform.position.x;
        _endX = _startX + TRAVEL_DISTANCE;
        Debug.Log("MovingCubeIsAwake");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(MasterManager.SpeedManager.MoveRate * Time.deltaTime, 0, 0);
        if (transform.position.x >= _endX)
        {
            transform.position = new Vector3(_startX, transform.position.y, transform.position.z);
        }
    }
}
