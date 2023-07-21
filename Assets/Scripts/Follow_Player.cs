using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow_Player : MonoBehaviour
{
    public GameObject playerObject; //Player object to follow
    public float offset = 1.155f;

    private float left_bound = 0.0f;
    [SerializeField] private float right_bound;

    // Update is called once per frame
    void Update()
    {
        if (playerObject != null)
        {
            //Restric camera's movement
            float horizontal_clamp = Mathf.Clamp(playerObject.transform.position.x+offset, left_bound, right_bound);
            Vector3 limit_Camera_Movement = new Vector3(horizontal_clamp, transform.position.y, transform.position.z);
            transform.position = limit_Camera_Movement;
        }
    }
}
