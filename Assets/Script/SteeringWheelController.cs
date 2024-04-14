using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheelController : MonoBehaviour
{
    [SerializeField] WheelController wheelControllerScript;
    float maxTurn = 90f, minTurn = -90f, currentTurn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTurn = Mathf.Lerp(minTurn, maxTurn, Mathf.InverseLerp(-wheelControllerScript.maxTurnAngle, wheelControllerScript.maxTurnAngle,wheelControllerScript.currentTurnAngle));
        transform.localRotation = Quaternion.Euler(23.253f, 0, -currentTurn);
    }
}
