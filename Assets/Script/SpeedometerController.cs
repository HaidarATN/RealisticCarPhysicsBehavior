using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedometerController : MonoBehaviour
{
    [SerializeField] EngineController carEngineController;
    [SerializeField] WheelController carWheelController;
    [SerializeField] Transform rpmArrow, speedArrow;

    float showedRPMValue;
    float rpmArrowRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        showedRPMValue = carEngineController.GetEngineRPM() * 7;
        rpmArrow.localRotation = Quaternion.Euler(0,0, Mathf.Lerp(0f, -210f, Mathf.InverseLerp(0f, 1300f, Mathf.Abs(carEngineController.GetEngineRPM()))));
        speedArrow.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0f, -260f, Mathf.InverseLerp(0f, 200f, Mathf.Abs(carWheelController.SpeedCalculation()))));
    }
}
