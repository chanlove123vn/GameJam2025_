using UnityEngine;
using System.Collections;

public class EnemyPlaneSpawner : PoolCtrl<Formation>
{
    public GameObject[] formationPrefabs;
    public float spawnDelay = 5f;

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
        foreach (GameObject formationPrefab in formationPrefabs)
        {
            if (formationPrefab == null) continue;

            Formation formationComponent = formationPrefab.GetComponent<Formation>();
            if (formationComponent == null)
            {
                Debug.LogError($"{formationPrefab.name} không có Formation component!");
                continue;
            }

            Vector3 spawnPos = GetSpawnPosition(formationComponent.direction);
            Vector3 targetPos = GetTargetPosition(formationComponent.direction);

            GameObject formationGo = this.Spawn(formationComponent, spawnPos, Quaternion.identity);
            Formation spawnedFormation = formationGo.GetComponent<Formation>();

            if (spawnedFormation != null)
            {
                spawnedFormation.SetTarget(targetPos);
                Debug.Log($"Spawned {formationPrefab.name} at {spawnPos}, target: {targetPos}");
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private Vector3 GetSpawnPosition(FormationDirection direction)
    {
        Vector3 topRight = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0));
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);

        switch (direction)
        {
            case FormationDirection.Right:
                return new Vector3(bottomLeft.x - 10, 0, 0);
            case FormationDirection.Left:
                return new Vector3(topRight.x + 10, 0, 0);
            case FormationDirection.Down:
                return new Vector3(0, topRight.y + 10, 0);
            default:
                return Vector3.zero;
        }
    }

    // ⭐ Overload GetTargetPosition theo direction
    private Vector3 GetTargetPosition(FormationDirection direction)
    {
        Vector3 topRight = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0));
        float targetY = topRight.y - 2;

        switch (direction)
        {
            case FormationDirection.Right:
            case FormationDirection.Left:
            case FormationDirection.Down:
                return new Vector3(0, targetY, 0);
            default:
                return Vector3.zero;
        }
    }
}
