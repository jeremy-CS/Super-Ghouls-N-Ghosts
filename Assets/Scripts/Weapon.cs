using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private bool mAttacking;
    private bool mAttackingUp;

    private float attack_Duration = 0.222f;
    private float time_01;
    private float time_02;

    private Animator mAnimator;

    [SerializeField] GameObject projectile_prefab;
    private Player player_Ref;

    private Vector3 up_projectile_offset = new(-0.2f,0.38f,0f);
    private Vector3 up_projectile_offset_left = new(0.2f, 0.38f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        //Get a reference to the player and its animator
        player_Ref = transform.parent.gameObject.GetComponent<Player>();
        mAnimator = transform.parent.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !mAttacking)
        {
            var projectile = Instantiate(projectile_prefab, this.gameObject.transform.position, Quaternion.identity);
            Projectile projectile_ref = projectile.GetComponent<Projectile>();

            //Set the direction of the weapon thrown
            projectile_ref.SetDirection(player_Ref.GetFacingDirection());
            
            //Play the some throwing sound!
            //mBusterSound.PlayOneShot(mBusterSound.clip);


            // Set animation params
            mAttacking = true;
            time_01 = 0.0f;
        }

        if (Input.GetButtonDown("Fire2") && !mAttacking)
        {
            Vector3 offset = up_projectile_offset;

            if (player_Ref.GetFacingDirection() == -Vector3.right)
            {
                offset = up_projectile_offset_left;
            }

            var projectile = Instantiate(projectile_prefab, this.gameObject.transform.position + offset, Quaternion.identity);
            Projectile projectile_ref = projectile.GetComponent<Projectile>();

            //Set the direction of the weapon thrown
            projectile_ref.SetDirection(Vector3.up);

            //Play the some throwing sound!
            //mBusterSound.PlayOneShot(mBusterSound.clip);


            // Set animation params
            mAttackingUp = true;
            time_02 = 0.0f;
        }


        if (mAttacking)
        {
            time_01 += Time.deltaTime;
            if (time_01 > attack_Duration)
            {
                mAttacking = false;
            }
        }

        if (mAttackingUp)
        {
            time_02 += Time.deltaTime;
            if (time_02 > attack_Duration)
            {
                mAttackingUp = false;
            }
        }

        UpdateAnimator();
    }
    
    private void UpdateAnimator()
    {
        mAnimator.SetBool("isAttacking", mAttacking);
        mAnimator.SetBool("isAttackingUp", mAttackingUp);
    }
}
