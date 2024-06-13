using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 5f;
    [SerializeField] float baseFiringRate = 0.2f;

    [Header("AI")]
    [SerializeField] float firingRateVariance = 0f;
    [SerializeField] float minimumFiringRate = 0.1f;
    [SerializeField] bool useAI;

    [HideInInspector] public bool isFiring;

    PlayerShip player;
    Coroutine firingCoroutine;
    AudioPlayer audioPlayer;
    Health health;
    Vector3 laserOffset = new(0.15f, 0.0f, 0.0f);

    void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
        health = FindObjectOfType<Health>();
        player = FindObjectOfType<PlayerShip>();
    }

    void Start()
    {
        if (useAI)
        {
            isFiring = true;
        }
    }

    void Update()
    {
        if (!health.GetIsDead()) { Fire(); }
    }

    void Fire()
    {
        if (player.GetVersionNumber() == 3 && health.GetIsPlayer() && isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(DoubleFireContinuously());
        }
        if (isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if (!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject instance = Instantiate(projectilePrefab,
                                              transform.position,
                                              Quaternion.identity);

            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = transform.up * projectileSpeed;
            }

            Destroy(instance, projectileLifetime);

            float timeToNextProjectile = Random.Range(baseFiringRate
                                                        - firingRateVariance,
                                                        baseFiringRate
                                                        + firingRateVariance);

            timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, minimumFiringRate, float.MaxValue);

            audioPlayer.PlayShootingClip();

            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }

    IEnumerator DoubleFireContinuously()
    {
        while (true)
        {
            GameObject instance = Instantiate(projectilePrefab,
                                              transform.position + laserOffset,
                                              Quaternion.identity);

            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = transform.up * projectileSpeed;
            }

            GameObject instance2 = Instantiate(projectilePrefab,
                                              transform.position - laserOffset,
                                              Quaternion.identity);

            Rigidbody2D rb2 = instance2.GetComponent<Rigidbody2D>();

            if (rb2 != null)
            {
                rb2.velocity = transform.up * projectileSpeed;
            }

            Destroy(instance, projectileLifetime);
            Destroy(instance2, projectileLifetime);

            float timeToNextProjectile = Random.Range(baseFiringRate
                                                        - firingRateVariance,
                                                        baseFiringRate
                                                        + firingRateVariance);

            timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, minimumFiringRate, float.MaxValue);

            audioPlayer.PlayShootingClip();

            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }

}
