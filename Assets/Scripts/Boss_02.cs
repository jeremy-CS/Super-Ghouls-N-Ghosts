using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_02 : MonoBehaviour
{
    //Animator Booleans
    private bool mIdle = true;
    private bool mHurt = false;

    //Hurt/invincible timer
    float mHurtTimer = 0f;
    float mHurtDuration = 0.8f;

    //Boss Data
    private int hp;
    private int attack;
    private float speed;
    private float range;
    private int pointsAwarded;
    private float attack_frequency;
    private float move_frequency;
    private float time_attack = 0f;
    private float time_move = 0f;
    [SerializeField] private Monster_data data;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private GameObject bomb_hit;
    [SerializeField] private GameObject monster_spawn;
    [SerializeField] private Transform[] monster_spawnpoints;

    //References for Boss
    private GameObject player;
    private Player player_ref;
    private Animator Boss_animator;
    private Score_manager score;
    [SerializeField] Level_complete level_Complete;

    //Boss music
    [SerializeField] private GameObject level_theme;
    private bool theme_changed = false;

    //Boss Animation
    [SerializeField] private GameObject death_prefab;

    // Start is called before the first frame update
    void Start()
    {
        SetBossData();
        player = GameObject.FindGameObjectWithTag("Player");
        player_ref = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Boss_animator = this.GetComponent<Animator>();
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score_manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (this.transform.position.x - player.transform.position.x <= range)
            {
                if (!theme_changed)
                {
                    ChangeTheme();
                }

                MoveAround();
                Attack();

                if (mHurt)
                {
                    mHurtTimer += Time.deltaTime;
                    if (mHurtTimer >= mHurtDuration)
                    {
                        mHurt = false;
                        mHurtTimer = 0.0f;
                    }
                }

                UpdateAnimator();
            }
        }
    }

    // Check if hit by player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            hp -= player_ref.attack;
            mHurt = true;
            Instantiate(bomb_hit, collision.transform.position, Quaternion.identity);

            if (hp <= 0)
            {
                Die();
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }

    // Check if Boss(body) hit player
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            player_ref.TakeDamage(attack);
        }
    }

    public void MoveAround()
    {
        time_move += Time.deltaTime;

        if (time_move >= move_frequency)
        {
            mIdle = true;
            int random_waypoint = Random.Range(0, 10);
            transform.position = Vector3.MoveTowards(transform.position, waypoints[random_waypoint].transform.position, speed * Time.deltaTime);
            time_move = 0f;
        }
    }

    //Attack
    public void Attack()
    {
        time_attack += Time.deltaTime;

        //Attack after certain time has passed
        if (time_attack >= attack_frequency)
        {
            int mon01 = Random.Range(0,monster_spawnpoints.Length);
            int mon02 = Random.Range(0, monster_spawnpoints.Length);

            Instantiate(monster_spawn, monster_spawnpoints[mon01].position, Quaternion.identity);
            Instantiate(monster_spawn, monster_spawnpoints[mon02].position, Quaternion.identity);

            //Reset attack timer
            time_attack = 0f;
        }
    }

    //Die
    public void Die()
    {
        Destroy(this.gameObject);
        score.AddPoints(pointsAwarded);

        //Instantiate deathSequence
        Instantiate(death_prefab, this.transform.position, Quaternion.identity);
        level_Complete.SetLevelComplete();
    }

    //Set Boss Data
    public void SetBossData()
    {
        hp = data.hp;
        attack = data.damage;
        speed = data.speed;
        range = data.follow_range;
        pointsAwarded = 250;
        attack_frequency = 3.5f;
    }

    //Update Animator of Boss
    public void UpdateAnimator()
    {
        Boss_animator.SetBool("isIdle", mIdle);
        Boss_animator.SetBool("isHurt", mHurt);
    }

    //Change theme for boss instead of level theme
    public void ChangeTheme()
    {
        //Stop level music and start boss music
        AudioSource[] level_themes = level_theme.GetComponents<AudioSource>();
        level_themes[0].mute = true;
        level_themes[1].Play();
        theme_changed = true;
    }
}