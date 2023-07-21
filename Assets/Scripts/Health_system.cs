using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_system : MonoBehaviour
{
    int mMaxHealth;
    int mIndex;

    Image[] mFilled;

    Player player_ref;

    // Start is called before the first frame update
    void Start()
    {
        player_ref = GameObject.Find("Player").GetComponent<Player>();
        mFilled = transform.GetComponentsInChildren<Image>();
        mMaxHealth = mFilled.Length - 1;
        mIndex = mFilled.Length - 1;
    }

    public void AddHealth(int x)
    {
        for (int i = 0; i < x; i++)
        {
            mFilled[mIndex].enabled = true;

            if (mIndex == mMaxHealth)
            {
                break;
            }
            mIndex++;
        }
    }

    public void DeductHealth(int x)
    {
        for (int i = 0; i < x; i++)
        {
            mFilled[mIndex].enabled = false;
            mIndex--;
            if (mIndex < 0)
            {
                // Kill Megaman
                player_ref.Die();
                break;
            }
        }
    }
}
