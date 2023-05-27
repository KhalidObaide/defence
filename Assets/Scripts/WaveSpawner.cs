using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaveSpawner : MonoBehaviour
{
    #if UNITY_EDITOR
        [Tag]
    #endif
    public string player1LaneTag;
    #if UNITY_EDITOR
        [Tag]
    #endif
    public string player2LaneTag;

    public List<Runner> runners;
    public bool autoWaveMode;
    public int numberOfWaves = 5;
    public float initialDelay = 2f;
    public float waveInterval = 5f;
    public int initialRunnerCount = 1;
    public int additionalRunnersPerWave = 1;

    private Transform spawnPoint;
    private Transform deathPoint;

    void Start()
    {
        bool isPlayer1 = PhotonNetwork.LocalPlayer.ActorNumber == PhotonNetwork.PlayerList[0].ActorNumber;
        GameObject activePlayerLane = GameObject.FindWithTag(isPlayer1 ? player1LaneTag : player2LaneTag);
        spawnPoint = FindInHierarchy(activePlayerLane, "SpawnPoint").transform;
        deathPoint = FindInHierarchy(activePlayerLane, "DeathPoint").transform;

        if ( autoWaveMode )
        {
            StartCoroutine(SpawnWaves());
        }
    }

    private void Update()
    {
        if ( autoWaveMode ) { return; }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int randomIndex = Random.Range(0, runners.Count);
            Runner randomRunner = runners[randomIndex];
            SpawnRunner(randomRunner);
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(initialDelay);

        for (int wave = 1; wave <= numberOfWaves; wave++)
        {
            int runnerCount = initialRunnerCount + (wave - 1) * additionalRunnersPerWave;

            foreach (var runner in runners)
            {
                for (int i = 0; i < runnerCount; i++)
                {
                    SpawnRunner(runner);
                    yield return new WaitForSeconds(0.5f);
                }
            }

            yield return new WaitForSeconds(waveInterval);
        }
    }

    void SpawnRunner(Runner runner)
    {
        GameObject runnerInstance = PhotonNetwork.Instantiate(runner.runnerPrefab.name, spawnPoint.position, spawnPoint.rotation);

        RunnerMovement runnerMovement = runnerInstance.GetComponent<RunnerMovement>();
        if (runnerMovement != null)
        {
            runnerMovement.Target = deathPoint; // Set the target for the runner
            runnerMovement.Speed = runner.speed; // Set the speed for the runner
        }
    }

    public GameObject FindInHierarchy(GameObject parent, string name)
    {
        if (parent.name == name)
        {
            return parent;
        }

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject result = FindInHierarchy(parent.transform.GetChild(i).gameObject, name);

            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}
