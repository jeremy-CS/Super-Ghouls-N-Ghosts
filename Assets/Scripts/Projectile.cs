using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float weapon_speed;
    [SerializeField] int weapon_force;
    Rigidbody2D mRigidBody2D;

    [SerializeField] bool isBossProjectile = false;

    //Variables for Boss projectiles
    Player player_ref;
    Vector3 direction_boss = new(-1f,0f,0f);
    [SerializeField] GameObject explosion_prefab;

    void Awake()
    {
        if(!isBossProjectile)
        {
            mRigidBody2D = this.gameObject.GetComponent<Rigidbody2D>();

            // Set a default direction
            mRigidBody2D.velocity = Vector2.right * weapon_speed;
        }

        player_ref = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if(isBossProjectile)
        {
            this.gameObject.transform.Translate(direction_boss * weapon_speed  * Time.deltaTime);
        }
    }

    public void SetDirection(Vector3 direction)
    {
        mRigidBody2D.velocity = direction * weapon_speed;

        //Change sprite rendition based on direction
        if (direction == Vector3.right || direction == -Vector3.right)
        {
            GetComponent<SpriteRenderer>().flipX = direction != Vector3.right;
        }

        if (direction == Vector3.up)
        {
            transform.Rotate(Vector3.forward*90);
        }
    }

    public void SetDirectionBoss(Vector3 direction)
    {
        direction_boss = direction;
    }

    // --- MONSTER PROJECTILE ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Destroy(collision.gameObject);
            DestroyBossProjectile();
        }
    }

    // Check if projectile hit player
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            player_ref.TakeDamage(weapon_force);
            DestroyBossProjectile();
        }
    }

    public void DestroyBossProjectile()
    {
        Instantiate(explosion_prefab, this.gameObject.transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
