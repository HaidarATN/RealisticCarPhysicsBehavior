using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineController : MonoBehaviour
{
    [SerializeField]
    float accelerationForce;
    [SerializeField]
    float MaxEngineRPM;
    [SerializeField]
    float MinEngineRPM;
    float prevRPM, currentRPM, diffRPM;
    bool isRPMSpiking;
    
    float EngineRPM = 0f;
    int currentGear = 0;

    public float MaxEndEngineRPM;
    public float[] gearRatio;

    [SerializeField] WheelCollider FrontRightCollider, FrontLeftCollider, RearRightCollider, RearLeftCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ControlEngineRPM();
        StartCoroutine(Shifting());
        //print(FrontRightCollider.rpm + " ___ " + FrontLeftCollider.rpm);
        //print(diffRPM);

    }

    public float EngineTorque()
    {
        return accelerationForce / gearRatio[currentGear] * Input.GetAxis("Vertical");
    }

    IEnumerator Shifting()
    {
        int correctCurrentGear = currentGear;
        if(EngineRPM >= MaxEngineRPM && Input.GetAxis("Vertical") != 0)
        {
            for(int i = 0; i < gearRatio.Length; i++)
            {
                if(FrontRightCollider.rpm * gearRatio[i] < MaxEngineRPM)
                {
                    correctCurrentGear = i;
                    break;
                }
            }

            currentGear = correctCurrentGear;
            //print(currentGear + " || " + FrontRightCollider.rpm * gearRatio[currentGear] + " || " + EngineRPM + " : " + FrontRightCollider.rpm * gearRatio[0] + " || " + FrontRightCollider.rpm * gearRatio[1] + " || " + FrontRightCollider.rpm * gearRatio[2] + " || " + FrontRightCollider.rpm * gearRatio[3] + " || " + FrontRightCollider.rpm * gearRatio[4] + " || " + FrontRightCollider.rpm * gearRatio[5] + " || ");

        }

        if (EngineRPM < MinEngineRPM && Input.GetAxis("Vertical") <= 0)
        {
            //print("hehe");
            for (int i = gearRatio.Length - 1; i >= 0; i--)
            {
                if (FrontRightCollider.rpm * gearRatio[i] > MinEngineRPM)
                {
                    correctCurrentGear = i;
                    break;
                }
            }

            currentGear = correctCurrentGear;
            //print("hehe");
        }

        yield return new WaitForSeconds(0.1f);
    }

    //stablize engine RPM when random spike happen so the engine rpm value are more reasonable
    void ControlEngineRPM()
    {
        currentRPM = ((FrontRightCollider.rpm + FrontLeftCollider.rpm) / 2f) * gearRatio[currentGear];
        //print("Engine RPM = " + EngineRPM + " || " + prevRPM + " || " + currentRPM + " || CurrentGear : " + diffRPM);

        if(prevRPM != 0)
        {
            diffRPM = Mathf.Abs(prevRPM - currentRPM);

            if (diffRPM > 50)
            {
                isRPMSpiking = true;
                if (currentRPM > prevRPM)
                    EngineRPM = prevRPM + 20;

                else
                    EngineRPM = prevRPM - 20;
            }

            else
            {
                EngineRPM = currentRPM;
                isRPMSpiking = false;
            }
                
        }

        if (isRPMSpiking && currentRPM > prevRPM)
        {
            prevRPM = EngineRPM;
        }

        else
            prevRPM = currentRPM;
    }

    public float GetEngineRPM()
    {
        return EngineRPM;
    }

    public float GetMaxEngineRPM()
    {
        return MaxEngineRPM;
    }
}
