using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float player_speed = 5.0f;
    public float hp = 16.0f;
    public int attack = 2;
    public float up_Boundary = 0.25f;
    public float down_Boundary = -0.25f;
    public Vector3 facing_direction;

    //death variables
    [SerializeField] Transform camera_ref;
    [SerializeField] Vector3 text_offset;
    [SerializeField] Vector3 flame_offset;
    [SerializeField] Vector3 book_offset;
    [SerializeField] GameObject gameOverText_prefab;
    [SerializeField] GameObject gameOverFire_prefab;
    [SerializeField] GameObject gameOverBook_prefab;
    [SerializeField] GameObject deathPrefab;

    [SerializeField] Health_system health;
    [SerializeField] GameOver_Manager gameover_manager;

    // Animator boolean
    bool mRunning;

    // References to other components and game objects
    Animator mAnimator;
    Rigidbody2D mRigidBody2D;

    // Invincibility timer
    float kInvincibilityDuration = 1.0f;
    float mInvincibleTimer;
    bool mInvincible;

    // Damage effects
    float kDamagePushForce = 25f;

    // Start is called before the first frame update
    void Start()
    {
        // Get references to other components and game objects
        mAnimator = GetComponent<Animator>();
        mRigidBody2D = GetComponent<Rigidbody2D>();

        facing_direction = Vector3.right;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Die();
        }

        playerMovement();
    }

    void playerMovement()
    {
        playerBoundary();

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            //convert user input into world movement
            float horizontalMovement = Input.GetAxisRaw("Horizontal");
            float verticalMovement = Input.GetAxisRaw("Vertical");

            //assign movement to a vector3
            Vector3 movementDirection = new Vector3(horizontalMovement, verticalMovement, 0).normalized;

            //apply movement to player
            gameObject.transform.Translate(movementDirection * player_speed * Time.deltaTime);
            mRunning = true;

            //Update the facing direction of the player
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                FaceDirection(Vector3.right);
            }
            else if (Input.GetAxisRaw("Horizontal") == -1)
            {
                FaceDirection(-Vector3.right);
            }
        }
        else
        {
            mRunning = false;
        }

        UpdateAnimator();

        // Upddate the invincibility timer
        if (mInvincible)
        {
            mInvincibleTimer += Time.deltaTime;
            if (mInvincibleTimer >= kInvincibilityDuration)
            {
                mInvincible = false;
                mInvincibleTimer = 0.0f;
            }
        }

    }

    void playerBoundary()
    {
        //Restric player's movement in y only
        float vertical_Movement_clamp = Mathf.Clamp(transform.position.y, down_Boundary, up_Boundary);
        Vector3 limit_Player_Movement = new Vector3(transform.position.x, vertical_Movement_clamp, 0);
        transform.position = limit_Player_Movement;
    }

    private void FaceDirection(Vector3 direction)
    {
        facing_direction = direction;

        Transform weapon_Transform = GetComponentInChildren<Weapon>().transform;
        weapon_Transform.localPosition = new Vector2((direction == Vector3.right ? 1 : -1) * Mathf.Abs(weapon_Transform.localPosition.x), weapon_Transform.localPosition.y);
        GetComponent<SpriteRenderer>().flipX = direction != Vector3.right;
    }

    public void Die()
    {
        Instantiate(deathPrefab, this.gameObject.transform.position, Quaternion.identity);

        Instantiate(gameOverFire_prefab, camera_ref.position + flame_offset, Quaternion.identity);
        Instantiate(gameOverText_prefab, camera_ref.position + text_offset, Quaternion.identity);
        Instantiate(gameOverBook_prefab, camera_ref.position + book_offset, Quaternion.identity);

        gameover_manager.SetGameOver();
        Destroy(this.gameObject);
    }

    public void TakeDamage(int dmg)
    {
        if (!mInvincible)
        {
            Vector2 forceDirection = new Vector2(-facing_direction.x, 1.0f) * kDamagePushForce;
            mRigidBody2D.velocity = Vector2.zero;
            mRigidBody2D.AddForce(forceDirection, ForceMode2D.Impulse);
            mInvincible = true;
            //mTakeDamageSound.Play();
            health.DeductHealth(dmg);
        }
    }

    public void GainHealth(int hp_bonus)
    {
        health.AddHealth(hp_bonus);
        hp += hp_bonus;
    }

    public Vector3 GetFacingDirection()
    {
        return facing_direction;
    }

    private void UpdateAnimator()
    {
        mAnimator.SetBool("isRunning", mRunning);
        //mAnimator.SetBool("isHurt", mInvincible);
    }
}
