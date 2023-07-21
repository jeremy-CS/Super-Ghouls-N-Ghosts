using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    //Monster Data
    private int damage = 1;
    private int hp = 1;
    private int points_awarded;
    private float lifetime;

    [SerializeField] private Monster_data data;
    [SerializeField] private Score_manager score_Manager;

    //Death animation
    [SerializeField] private GameObject death_prefab;
    [SerializeField] private GameObject timed_out_prefab;

    private Player player_ref;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
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
                timed_out_prefab.GetComponent<SpriteRenderer>().flipX = this.transform.position.x - player.transform.position.x < 0;
                Instantiate(timed_out_prefab, this.transform.position, Quaternion.identity);
                Destroy(gameObject);    // kill me
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

                points_awarded = Random.Range(1, 4);
                score_Manager.AddPoints(points_awarded);

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

    //Set monster data
    private void SetMonsterData()
    {
        damage = data.damage;
        hp = data.hp;
        lifetime = data.lifetime;
    }
}
