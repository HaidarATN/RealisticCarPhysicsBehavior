using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] WheelCollider FrontRightCollider, FrontLeftCollider, RearRightCollider, RearLeftCollider;
    [SerializeField] Transform FrontRightTransform, FrontLeftTransform, RearRightTransform, RearLeftTransform;
    [SerializeField] Rigidbody carRb;
    [SerializeField] EngineController engineControllerScript;
    [SerializeField] AudioSource wheelSoundSource;

    public float accelerationForce = 100f;
    public float brakingForce = 300f;
    public float maxTurnAngle = 15f;
    [HideInInspector]
    public float currentTurnAngle;
    public Vector3 carCenterOfMass;
    public GameController gc;

    float currentAccelerationForce;
    float currentBrakingForce;
    float currentExtremumSlip;
    float refVelo = 0f;

    WheelFrictionCurve wfc,wfc_back;

    private void Start()
    {
        wfc = FrontRightCollider.sidewaysFriction;
        wfc_back = RearRightCollider.sidewaysFriction;
        currentExtremumSlip = FrontRightCollider.sidewaysFriction.extremumSlip;
        carRb.centerOfMass = carCenterOfMass;
        currentTurnAngle = Mathf.Lerp(currentTurnAngle, 5, 3f);
    }

    private void FixedUpdate()
    {
        if(gc.time <= 0 && !gc.isWrecked)
        {
            //accelerating input
            float refVelo = 0;
            //currentAccelerationForce = accelerationForce * Input.GetAxis("Vertical");
            currentAccelerationForce = engineControllerScript.EngineTorque();

            //braking input
            if (Input.GetKey(KeyCode.Space))
            {
                currentBrakingForce = brakingForce;
                carRb.drag = 1;
            }

            else
            {
                currentBrakingForce = 0;
                carRb.drag = 0;
            }

            //steering input
            currentTurnAngle = Mathf.SmoothDamp(currentTurnAngle, (maxTurnAngle * Input.GetAxis("Horizontal")), ref refVelo, 0.1f);

            //apply acceleration to rear wheels (RWD setup)
            if (engineControllerScript.GetEngineRPM() > engineControllerScript.MaxEndEngineRPM)
            {
                RearRightCollider.motorTorque = 0;
                RearLeftCollider.motorTorque = 0;
            }

            else
            {
                RearRightCollider.motorTorque = currentAccelerationForce;
                RearLeftCollider.motorTorque = currentAccelerationForce;
            }

            //apply braking
            FrontLeftCollider.brakeTorque = currentBrakingForce;
            FrontRightCollider.brakeTorque = currentBrakingForce;

            //apply steering
            FrontRightCollider.steerAngle = currentTurnAngle;
            FrontLeftCollider.steerAngle = currentTurnAngle;

            //apply rotation on wheel meshes
            UpdateWheelTransform(FrontRightTransform, FrontRightCollider);
            UpdateWheelTransform(FrontLeftTransform, FrontLeftCollider);
            UpdateWheelTransform(RearRightTransform, RearRightCollider);
            UpdateWheelTransform(RearLeftTransform, RearLeftCollider);

            //manage downforce at highspeed (prevent understeering)
            FrontRightCollider.GetGroundHit(out WheelHit wheelData);
            float sideSlip = wheelData.sidewaysSlip;
            float frontStiffness = Mathf.Lerp(1f, 3f, Mathf.InverseLerp(0f, 0.81f, Mathf.Abs(sideSlip)));
            wfc.stiffness = frontStiffness;
            wfc_back.stiffness = frontStiffness * 2f;
            FrontRightCollider.sidewaysFriction = wfc;
            FrontLeftCollider.sidewaysFriction = wfc;
            RearRightCollider.sidewaysFriction = wfc_back;
            RearLeftCollider.sidewaysFriction = wfc_back;

            if (SpeedCalculation() > 80f)
            {
                maxTurnAngle = 15f;
            }

            else
                maxTurnAngle = 45f;

            //print("Engine Torque = " + engineControllerScript.EngineTorque() + " || " + "Engine RPM = " + engineControllerScript.EngineRPM + " || " + "Speed = " + SpeedCalculation());

            WheelSlideSound();
        }

        //car is wrecked
        if (gc.isWrecked)
        {
            currentBrakingForce = brakingForce;

            RearRightCollider.motorTorque = 0;
            RearLeftCollider.motorTorque = 0;
            FrontLeftCollider.brakeTorque = currentBrakingForce;
            FrontRightCollider.brakeTorque = currentBrakingForce;

        }
    }

    void UpdateWheelTransform(Transform wheelTransform, WheelCollider wheelCol)
    {
        Vector3 wheelPosition;
        Quaternion wheelRotation;

        wheelCol.GetWorldPose(out wheelPosition, out wheelRotation);

        wheelTransform.position = wheelPosition;
        wheelTransform.rotation = wheelRotation;
    }

    public float SpeedCalculation()
    {
        return carRb.velocity.magnitude * 3.6f;
    }

    public float EngineRPM()
    {
        return RearLeftCollider.rpm;
    }

    void WheelSlideSound()
    {
        FrontRightCollider.GetGroundHit(out WheelHit wheelData);
        float sideSlip = wheelData.sidewaysSlip;

        wheelSoundSource.volume = Mathf.Lerp(0f, 1f, Mathf.InverseLerp(0.1f, 0.5f, Mathf.Abs(sideSlip)));
    }
}
