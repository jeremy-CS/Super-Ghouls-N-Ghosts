using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timed_expiration : MonoBehaviour
{
    [SerializeField] float lifetime;

    // Update is called once per frame
    void Update()
    {
        // decrease our life timer
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            // we have ran out of life
            Destroy(gameObject);    // kill me
        }
    }
}
