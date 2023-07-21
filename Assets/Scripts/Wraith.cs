using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wraith : MonoBehaviour
{
    //Monster Data
    private int hp;
    private int damage;
    private float speed;
    private float range;
    private int points_awarded = 50;
    private float lifetime;

    [SerializeField] private Monster_data data;
    [SerializeField] private Score_manager score_Manager;

    //Death animation
    [SerializeField] private GameObject death_prefab;
    [SerializeField] private GameObject timed_out_prefab;

    private Player player_ref;
    private GameObject player;
    private bool playerDetected = false;
    private Animator mAnimator;
    private Vector3 runningDirection = Vector3.right;


    // Start is called before the first frame update
    void Start()
    {
        mAnimator = GetComponent<Animator>();
        player_ref = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player = GameObject.FindGameObjectWithTag("Player");
        score_Manager = GameObject.FindGameObjectWithTag("Score").GetComponent<Score_manager>();
        SetMonsterData();
    }

    private void Update()
    {
        if (player != null)
        {
            // decrease our life timer
            lifetime -= Time.deltaTime;
            if (lifetime <= 0f)
            {
                // we have ran out of life
                timed_out_prefab.GetComponent<SpriteRenderer>().flipX = this.transform.position.x - player.transform.position.x > 0;
                Instantiate(timed_out_prefab, this.transform.position, Quaternion.identity);
                Destroy(gameObject);    // kill me
            }

            RunAway();
            UpdateAnimator();
        }
    }

    public void RunAway()
    {
        //Chase player if in range (should change to give more movement)
        if (Vector3.Magnitude(player.transform.position - transform.position) < range || playerDetected)
        {
            if(this.transform.position.x - player.transform.position.x > 0)
            {
                flipSprite(true);
                runningDirection = Vector3.right;
            }
            else
            {
                flipSprite(false);
                runningDirection = Vector3.left;
            }

            //Run away from the player
            this.gameObject.transform.Translate(runningDirection * speed * Time.deltaTime);

            if(!playerDetected)
            {
                playerDetected = true;
            }
        }
    }

    // Check if monster hit by player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            hp -= player_ref.attack;

            if (hp <= 0)
            {
                Destroy(collision.gameObject);
                Destroy(this.gameObject);

                score_Manager.AddPoints(points_awarded);

                //Restore Health to player
                player_ref.GainHealth(4);

                //Instantiate deathSequence
                death_prefab.GetComponent<SpriteRenderer>().flipX = this.transform.position.x - player.transform.position.x < 0;
                Instantiate(death_prefab, this.transform.position, Quaternion.identity);
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }

    // Check if monster hit player
    // NOTE: I didn't put the automatic damage from the monster since some of them can spawn far away from the player
    void OnTriggerStay2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player")
        {
            player_ref.TakeDamage(damage);
            Instantiate(death_prefab, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void flipSprite(bool flip)
    {
        this.GetComponent<SpriteRenderer>().flipX = flip;
    }

    private void UpdateAnimator()
    {
        mAnimator.SetBool("playerDetected", playerDetected);
    }

    //Set monster data
    private void SetMonsterData()
    {
        hp = data.hp;
        damage = data.damage;
        speed = data.speed;
        range = data.follow_range;
        lifetime = data.lifetime;
    }
}
