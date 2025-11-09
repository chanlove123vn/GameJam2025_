using UnityEngine;
using System.Collections;

public class EnemyPlaneSpawner : PoolCtrl<Formation>
{
    public GameObject[] formationPrefabs;
    public float spawnDelay = 2f;

    void Start()
    {
        if (formationPrefabs == null || formationPrefabs.Length == 0)
        {
            Debug.LogError("Formation prefabs không được assign!");
            return;
        }
        StartCoroutine(SpawnFormations());
    }

    IEnumerator SpawnFormations()
    {
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        float screenLeft = bottomLeft.x;
        float spawnY = 0f;
        float targetX = 0f;

        foreach (GameObject formationPrefab in formationPrefabs)
        {
            if (formationPrefab == null) continue;

            Vector3 spawnPos = new Vector3(screenLeft - 20f, spawnY, 0);
            Vector3 targetPos = new Vector3(targetX, spawnY, 0);

            // Get Formation component từ prefab
            Formation formationComponent = formationPrefab.GetComponent<Formation>();
            if (formationComponent == null)
            {
                Debug.LogError($"Formation prefab {formationPrefab.name} không có Formation component!");
                continue;
            }

            // Spawn formation từ pool
            GameObject formationGo = this.Spawn(formationComponent, spawnPos, Quaternion.identity);
            Formation formation = formationGo.GetComponent<Formation>();

            if (formation != null)
            {
                formation.SetTarget(targetPos);
                Debug.Log($"Spawned formation at {spawnPos}, target: {targetPos}");
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
