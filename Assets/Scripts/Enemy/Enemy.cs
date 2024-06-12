using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    public static void Create(Vector3 position)
    {
        Transform pfEnemyTransform = Resources.Load<Transform>("pfEnemy");
        Instantiate(pfEnemyTransform, position, quaternion.identity);
    }

    private IObjectPool<Enemy> objectPool;

    public IObjectPool<Enemy> ObjectPool
    {
        set => objectPool = value;
    }

    private Rigidbody2D rb;

    private Transform targetTransform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (targetTransform == null)
            targetTransform = BuildingManager.Instance.GetHQTransform();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (targetTransform != null)
        {
            Vector2 moveDir = (targetTransform.position - transform.position).normalized;

            float speed = 6f;

            rb.velocity = moveDir * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BuildingTypeHolder>() != null)
        {
            objectPool.Release(this);
        }
    }
}
