using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase_player : MonoBehaviour
{
    [SerializeField] private GameObject player_target;
    private float follow_speed = 1f;
    private float follow_range = 10.0f;

    [SerializeField] private Monster_data data;

    private void Start()
    {
        SetTarget(GameObject.FindGameObjectWithTag("Player"));
        SetFollowData();
    }

    // Update is called once per frame
    void Update()
    {
        if (player_target != null)
        {
            Chase();
        }
    }

    private void Chase()
    {
        //Chase player if in range (should change to give more movement)
        if (Vector3.Magnitude(player_target.transform.position - transform.position) < follow_range)
        {
            transform.position = Vector3.MoveTowards(transform.position, player_target.transform.position, follow_speed * Time.deltaTime);
            this.gameObject.GetComponent<SpriteRenderer>().flipX = transform.position.x - player_target.transform.position.x < 0;
        }
    }

    public void SetTarget(GameObject target)
    {
        player_target = target;
    }

    public void SetFollowData()
    {
        follow_speed = data.speed;
        follow_range = data.follow_range;
    }
}
