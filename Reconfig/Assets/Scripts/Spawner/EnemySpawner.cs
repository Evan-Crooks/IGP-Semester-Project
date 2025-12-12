using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{

    private int NumberOfEnemiesSpawned = 0;
    public int MaxEnemies = 10;
    public Tilemap tilemap;
    public GameObject enemyMeleePrefab;
    public GameObject enemyRangedPrefab;
    public float timeSinceLastSpawn = 0f;
    public float spawnTime = 0.1f;
    private bool spawnMelee = true;

    public static EnemySpawner instance { get; set; }

    private void Awake()
    {
        instance = this;
        if (tilemap == null) tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    }

    private void spawnEnemy()
    {
        for (int tries = 0; tries < 1000; tries++)
        {
            int x = UnityEngine.Random.Range(0, tilemap.cellBounds.size.x);
            int y = UnityEngine.Random.Range(0, tilemap.cellBounds.size.y);
            Vector3Int cellPos = new Vector3Int(x, y, 0);

            //spawn if empty tile
            if (tilemap.GetTile(cellPos) == null)
            {
                Vector3 worldPos = tilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0.5f, 0);
                if (spawnMelee)
                {
                    Instantiate(enemyMeleePrefab, worldPos, Quaternion.identity);
                } else
                {
                    Instantiate(enemyRangedPrefab, worldPos, Quaternion.identity);
                }
                spawnMelee = !spawnMelee;
                break;
            }
        }
    }

    public void DecrementEnemies()
    {
        NumberOfEnemiesSpawned--;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If there are not 10 enemies
        if (NumberOfEnemiesSpawned < MaxEnemies && timeSinceLastSpawn > spawnTime)
        {
            timeSinceLastSpawn = 0;
            spawnEnemy();
            NumberOfEnemiesSpawned++;
        }
        timeSinceLastSpawn += Time.deltaTime;
    }
}
