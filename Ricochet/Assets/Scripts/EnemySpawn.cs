using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate;
    GameObject[] spawnPoints;
    int enemyCount = 0;
    float timer = 0f;

    private void Awake()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        getEnemyCount();
        if (timer >= spawnRate)
        {
            timer = 0;
            SpawnEnemy();
        }
    }

    void getEnemyCount()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    void SpawnEnemy()
    {
        if (enemyCount < 10)
        {
            int i = Random.Range(0, spawnPoints.Length - 1);
            Instantiate(enemyPrefab, spawnPoints[i].transform.position, Quaternion.identity);
        }
    }
}
