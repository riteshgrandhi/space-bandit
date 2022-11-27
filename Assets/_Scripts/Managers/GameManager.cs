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
    public float currentSpeed;
    private PlayerController spawnedPlayer;
    private ParticleSystem backgroundParticleSystem;
    private ParticleSystem.MainModule main;
    private ParticleSystem.TrailModule trails;
    protected override void Awake()
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
            yield return new WaitForSeconds(3);

            // transition to normal speed
            currentSpeed = normalSpeedMultiplier;
            StartCoroutine(StartTransitionToNormal(3));
            yield return new WaitForSeconds(3);

            // Spawn wave
            templateWaveController.waveConfig = waveConfig;
            WaveController instantiatedWave = Instantiate(templateWaveController);
            yield return new WaitUntil(() => instantiatedWave.isDone);

            // transition to hyperspeed
            currentSpeed = hyperSpeedMultiplier;
            StartCoroutine(StartTransitionToHyperSpeed(3));
            yield return new WaitForSeconds(3);
        }
    }

    private IEnumerator StartTransitionToNormal(float transitionTime)
    {
        float t = 0;
        float startSpeed = main.startSpeedMultiplier;
        float trailLifetime = trails.lifetimeMultiplier;
        while (t < 1)
        {
            t += Time.deltaTime / transitionTime;
            main.startSpeedMultiplier = Mathf.Lerp(startSpeed, normalSpeedMultiplier, t);
            trails.lifetimeMultiplier = Mathf.Lerp(trailLifetime, 0, t);
            yield return new WaitForEndOfFrame();
        }
        main.startSpeedMultiplier = normalSpeedMultiplier;
        trails.lifetimeMultiplier = 0;
        trails.enabled = true;
    }

    private IEnumerator StartTransitionToHyperSpeed(float transitionTime)
    {
        float t = 0;
        float startSpeed = main.startSpeedMultiplier;
        float trailLifetime = trails.lifetimeMultiplier;
        trails.enabled = true;
        while (t < 1)
        {
            t += Time.deltaTime / transitionTime;
            main.startSpeedMultiplier = Mathf.Lerp(startSpeed, hyperSpeedMultiplier, t);
            trails.lifetimeMultiplier = Mathf.Lerp(trailLifetime, hyperSpeedTrailLifetimeMultiplier, t);
            yield return new WaitForEndOfFrame();
        }
        main.startSpeedMultiplier = hyperSpeedMultiplier;
        trails.lifetimeMultiplier = hyperSpeedTrailLifetimeMultiplier;
    }

    public void AddScore(int increment)
    {
        score += increment;
    }
}