using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    [SerializeField] AudioSource collisionAudioSource;

    public GameObject sparkEffect;
    public int carHealth, maxCarHealth;

    // Start is called before the first frame update
    void Start()
    {
        maxCarHealth = carHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Calculate collision force
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 collisionForce = collision.impulse / Time.fixedDeltaTime;

        if (Mathf.Abs(collisionForce.z) >= 150000)
        {
            collisionAudioSource.Play();
            GameObject sparkEffectObject = Instantiate(sparkEffect, new Vector3(collision.contacts[0].point.x, collision.contacts[0].point.y + 0.5f, collision.contacts[0].point.z), Quaternion.identity);
            StartCoroutine(DestroyEffect(sparkEffectObject));
            carHealth--;
        }
            
    }

    IEnumerator DestroyEffect(GameObject spark)
    {
        yield return new WaitForSeconds(1f);

        Destroy(spark);

    }
}
