using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float spawn_range;
    [SerializeField] private float spawn_intereval;
    public Transform[] spawn_points;
    public GameObject[] monster_prefabs;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy());
    }

    private IEnumerator spawnEnemy()
    {
        //Wait timer out before spawning more monsters
        yield return new WaitForSeconds(spawn_intereval);

        if (player != null)
        {
            //Get the location and type of monster randomly
            int randMonster = Random.Range(0, monster_prefabs.Length);
            int randSpawnPoint = Random.Range(0, spawn_points.Length);

            //if range between monster and player is acceptable, permit spawn
            if (Vector3.Magnitude(player.transform.position - spawn_points[randSpawnPoint].position) <= spawn_range)
            {
                Instantiate(monster_prefabs[randMonster], spawn_points[randSpawnPoint].position, Quaternion.identity);
            }

            StartCoroutine(spawnEnemy());
        }
        else
        {
            StopCoroutine(spawnEnemy());
        }
        
    }
}
