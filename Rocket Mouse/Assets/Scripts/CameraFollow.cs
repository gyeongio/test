using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetObject;

    private float distanceToTarget;

    private void Start()
    {
        distanceToTarget = transform.position.x - targetObject.transform.position.x;
    }

    private void Update()
    {
        float targetObjectX = targetObject.transform.position.x;
        Vector3 newCaneraPosition = transform.position;
        newCaneraPosition.x = targetObjectX + distanceToTarget;
        transform.position = newCaneraPosition;
        
    }
    
}
