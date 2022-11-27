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
    public float normalSpeedTrailLifetimeMultiplier = 0;
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
            StartCoroutine(StartTransition(3, hyperSpeedMultiplier, normalSpeedMultiplier, hyperSpeedTrailLifetimeMultiplier, normalSpeedTrailLifetimeMultiplier));
            yield return new WaitForSeconds(3);
            trails.enabled = false;

            // Spawn wave
            templateWaveController.waveConfig = waveConfig;
            WaveController instantiatedWave = Instantiate(templateWaveController);
            yield return new WaitUntil(() => instantiatedWave.isDone);

            // transition to hyperspeed
            currentSpeed = hyperSpeedMultiplier;
            trails.enabled = true;
            StartCoroutine(StartTransition(3, normalSpeedMultiplier, hyperSpeedMultiplier, normalSpeedTrailLifetimeMultiplier, hyperSpeedTrailLifetimeMultiplier));
            yield return new WaitForSeconds(3);
        }
    }

    private IEnumerator StartTransition(
        float transitionTime,
        float fromSpeed,
        float toSpeed,
        float fromTrailSpeed,
        float toTrailSpeed)
    {
        float t = 0;
        ParticleSystem.Particle[] m_Particles = new ParticleSystem.Particle[backgroundParticleSystem.main.maxParticles];
        while (t < 1)
        {
            t += Time.deltaTime / transitionTime;
            SetParticleSpeed(Vector3.left * Mathf.Lerp(fromSpeed, toSpeed, t), backgroundParticleSystem, m_Particles);
            trails.lifetimeMultiplier = Mathf.Lerp(fromTrailSpeed, toTrailSpeed, t);
            yield return new WaitForEndOfFrame();
        }
        SetParticleSpeed(Vector3.left * toSpeed, backgroundParticleSystem, m_Particles);
        main.startSpeedMultiplier = toSpeed;
        trails.lifetimeMultiplier = toTrailSpeed;
    }

    private static void SetParticleSpeed(Vector3 velocity, ParticleSystem backgroundParticleSystem, ParticleSystem.Particle[] m_Particles)
    {
        int numParticlesAlive = backgroundParticleSystem.GetParticles(m_Particles);
        for (int i = 0; i < numParticlesAlive; i++)
        {
            m_Particles[i].velocity = velocity;
        }
        backgroundParticleSystem.SetParticles(m_Particles, numParticlesAlive);
    }

    public void AddScore(int increment)
    {
        score += increment;
    }
}