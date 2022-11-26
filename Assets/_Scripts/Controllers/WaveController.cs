using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public WaveConfig waveConfig;
    private List<Damageable> allSpawnedEnemies = new List<Damageable>();
    private List<Damageable> currentPhaseEnemies = new List<Damageable>();

    void Start() => StartCoroutine(StartWavePhases());

    private IEnumerator StartWavePhases()
    {
        Debug.Log("In StartWavePhases");

        foreach (Phase currentPhase in waveConfig.phases)
        {
            currentPhaseEnemies.Clear();
            currentPhaseEnemies.AddRange(SpawnEnemiesForPhase(currentPhase));

            switch (currentPhase.onPhaseSpawningDone)
            {
                case OnPhaseSpawningDone.WAIT_FOR_ALL_CURRENT_DONE:
                    yield return new WaitUntil(() => allSpawnedEnemies.Count == 0);
                    break;
                case OnPhaseSpawningDone.WAIT_FOR_THIS_PHASE_DONE:
                    yield return new WaitUntil(() => currentPhaseEnemies.Count == 0);
                    break;
                case OnPhaseSpawningDone.CONTINUE_TO_NEXT_PHASE:
                    break;
            }

            yield return new WaitForSeconds(currentPhase.waitBeforeNextPhaseSeconds);
        }
    }

    private List<Damageable> SpawnEnemiesForPhase(Phase currentPhase)
    {
        Debug.Log("In SpawnEnemiesForPhase");
        for (int i = 0; i < currentPhase.count; i++)
        {
            Vector2 spawnPosition = new Vector3(10, Random.Range(7, -7));
            Enemy spawnedEnemy = Instantiate(currentPhase.enemy, spawnPosition, Quaternion.identity);
            spawnedEnemy.OnDeathHandler += OnSpawnedEnemyDeath;
            currentPhaseEnemies.Add(spawnedEnemy);
            allSpawnedEnemies.Add(spawnedEnemy);
        }
        return currentPhaseEnemies;
    }

    private void OnSpawnedEnemyDeath(Damageable enemy)
    {
        Debug.Log(enemy.name + " Dead!");
        bool resultAll = allSpawnedEnemies.Remove(enemy);
        Debug.Log("resultAll: " + resultAll);

        bool resultCurrent = currentPhaseEnemies.Remove(enemy);
        Debug.Log("resultCurrent: " + resultCurrent);
    }
}