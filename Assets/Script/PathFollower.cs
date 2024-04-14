using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PathFollower : MonoBehaviour
{
    [SerializeField] PathCreator track;

    public GameController gc;
    public float maxSpeed, minSpeed;

    float speed = 50;
    float acceleration = 0;
    float distanceTraveled;
    bool isAccelerate;

    private void Start()
    {
        StartCoroutine(RandomizeSpeed());
    }
    // Update is called once per frame
    void Update()
    {
        print(acceleration);
        if (gc.time <= 0)
        {
            if (!isAccelerate)
            {
                StartCoroutine(Accelerate());
                isAccelerate = true;
            }
            //acceleration = Mathf.Lerp(0.0f, 1.0f, 0.1f);
            distanceTraveled += speed * acceleration * Time.deltaTime;

            transform.position = track.path.GetPointAtDistance(distanceTraveled);
            transform.rotation = track.path.GetRotationAtDistance(distanceTraveled);
        }
    }

    IEnumerator RandomizeSpeed()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        yield return new WaitForSeconds(5f);

        StartCoroutine(RandomizeSpeed());
    }

    IEnumerator Accelerate()
    {
        if(acceleration < 1)
        acceleration += 0.1f;

        yield return new WaitForSeconds(0.2f);

        if (acceleration >= 1)
            yield break;

        StartCoroutine(Accelerate());
    }
}
