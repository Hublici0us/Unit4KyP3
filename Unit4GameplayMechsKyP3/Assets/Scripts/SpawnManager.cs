
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> enemyPrefab;
    public List<GameObject> bossPrefab;
    public List<GameObject> powerUpPrefabs;

    private float spawnPos = 9.0f;
    public int enemyCount;
    public int waveCount = 1;
    public int pastBossWaves = 1;
    public bool bossWave;

    // Start is called before the first frame update
    void Start()
    {
        bossWave = false;
        SpawnEnemyWave(waveCount);
        SpawnPowerUp();
    }

    // Update is called once per frame
    void Update()
    {
        //finds any object that has enemy controlller script.
        enemyCount = FindObjectsOfType<EnemyController>().Length;

        if (enemyCount == 0 )
        {
            waveCount++;
            if (waveCount == pastBossWaves * 5)
            {
                bossWave = true;
                SpawnBossWave();
                pastBossWaves++;
            }
            else
            {
                SpawnEnemyWave(waveCount);
            }
            SpawnPowerUp();

            if (GameObject.FindGameObjectWithTag("Boss").gameObject != null)
            {
                InvokeRepeating("SpawnBossMinions", 5, 3);
            }
            else if (GameObject.FindGameObjectWithTag("Boss").gameObject == null) 
            {
                bossWave = false;
                CancelInvoke("SpawnBossMinions");
            }
        }
    }

    private Vector3 CreateRandomSpawn()
    {
        float spawnPositionX = Random.Range(-spawnPos, spawnPos);
        float spawnPositionZ = Random.Range(-spawnPos, spawnPos);

        Vector3 RandomPosition = new Vector3(spawnPositionX, 0, spawnPositionZ);

        return RandomPosition;
    }

    private void SpawnPowerUp()
    {
        int powerUpSelected = Random.Range(0, powerUpPrefabs.Count);

        Instantiate(powerUpPrefabs[powerUpSelected], CreateRandomSpawn(), Quaternion.identity);
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int chooseEnemy;
            float enemyDifficulty = (waveCount * 1.25f);
            if (enemyDifficulty < enemyPrefab.Count)
            {
                chooseEnemy = Random.Range(0, Mathf.FloorToInt(enemyDifficulty));
            }
            else
            {
                chooseEnemy = Random.Range(0, enemyPrefab.Count);
            }
            Instantiate(enemyPrefab[chooseEnemy], CreateRandomSpawn(), Quaternion.identity);

            
        }
    }

    void SpawnBossWave()
    {
        
        int chooseBoss = Random.Range(0, bossPrefab.Count);
        Instantiate(bossPrefab[chooseBoss], CreateRandomSpawn(), Quaternion.identity);
    }

    void SpawnBossMinions()
    {
        Instantiate(enemyPrefab[2], CreateRandomSpawn(), Quaternion.identity);
    }
}
