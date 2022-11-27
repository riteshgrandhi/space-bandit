using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class GameManager : Singleton<GameManager>
{
    public int score;
    public float normalSpeedMultiplier = 5;
    public float hyperSpeedMultiplier = 40;
    public float hyperSpeedTrailLifetimeMultiplier = 0.04f;
    public List<WaveConfig> waveConfigs;
    public PlayerController playerTemplate;
    public WaveController templateWaveController;
    private PlayerController spawnedPlayer;
    private ParticleSystem backgroundParticleSystem;
    private ParticleSystem.MainModule main;
    private ParticleSystem.TrailModule trails;

    new void Awake()
    {
        base.Awake();
        backgroundParticleSystem = GetComponent<ParticleSystem>();
        main = backgroundParticleSystem.main;
        trails = backgroundParticleSystem.trails;

        main.startSpeedMultiplier = hyperSpeedMultiplier;
        trails.lifetimeMultiplier = hyperSpeedTrailLifetimeMultiplier;
        trails.enabled = true;
    }

    void Start()
    {
        spawnedPlayer = Instantiate(playerTemplate);

        StartCoroutine(StartWaves());
    }

    private IEnumerator StartWaves()
    {
        // time before spawning waves
        yield return new WaitForSeconds(2);

        foreach (WaveConfig waveConfig in waveConfigs)
        {
            // time to wait in hyperspeed
            yield return new WaitForSeconds(5);

            // transition to normal speed
            StartCoroutine(StartTransition(normalSpeedMultiplier, 5, false));
            yield return new WaitForSeconds(5);

            // Spawn waves
            templateWaveController.waveConfig = waveConfig;
            WaveController instantiatedWave = Instantiate(templateWaveController);
            yield return new WaitUntil(() => instantiatedWave.isDone);

            // transition to hyperspeed
            StartCoroutine(StartTransition(hyperSpeedMultiplier, 5, true));
            yield return new WaitForSeconds(5);
        }
    }

    private IEnumerator StartTransition(float targetSpeed, float transitionTime, bool trailsEnabledOnDone)
    {
        float t = 0;
        float startSpeed = main.startSpeedMultiplier;
        float trailLifetime = trails.lifetimeMultiplier;
        while (t < 1)
        {
            t += Time.deltaTime / transitionTime;
            main.startSpeedMultiplier = Mathf.Lerp(startSpeed, targetSpeed, t);
            trails.lifetimeMultiplier = Mathf.Lerp(trailLifetime, trailsEnabledOnDone ? hyperSpeedTrailLifetimeMultiplier : 0, t);
            yield return new WaitForEndOfFrame();
        }
        main.startSpeedMultiplier = targetSpeed;
        trails.lifetimeMultiplier = trailsEnabledOnDone ? hyperSpeedTrailLifetimeMultiplier : 0;
        trails.enabled = trailsEnabledOnDone;
    }

    public void AddScore(int increment)
    {
        score += increment;
    }
}