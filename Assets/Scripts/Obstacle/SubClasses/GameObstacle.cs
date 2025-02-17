﻿using UnityEngine;

/// <summary>
/// Obstacle class
/// </summary>
public class GameObstacle : Obstacle, IDestroyEffect
{
    //USING CUSTOM EDITOR
    public float minDensity;
    public float maxDensity;
    public GameObject deathEffect;
    public float density;
    public float mass;

    public new Rigidbody rigidbody;

    public ObstacleGravity gravityComponent;

    private new void Awake()
    {
        base.Awake();
        gravityComponent = GetComponentInChildren<ObstacleGravity>();
        rigidbody = GetComponent<Rigidbody>();
    }
    //Collision and triggers

    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        string tag = collision.collider.tag;
        if (tag.Equals("Player"))
        {
            Destroy(true);
        }
    }

    protected new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
    //

    public override void SetupObstacle()
    {
        int layerMask = LayerMask.GetMask("Obstacles");
        Collider[] colliders = Physics.OverlapSphere(transform.position, targetScale / 2, layerMask);
        if(colliders.Length > 1)
        {
            base.DeactivateObstacle();
            return;
        }

        density = Random.Range(minDensity, maxDensity);
        mass = GameplayMath.GetInstance().GetObstacleMass(this);

        SphereCollider dangerZone = SharedUtilities.GetInstance().GetFirstComponentInChildrenWithTag<SphereCollider>(gameObject, "DangerZone");
        SphereCollider gravityField = SharedUtilities.GetInstance().GetFirstComponentInChildrenWithTag<SphereCollider>(gameObject, "GravityField");
        gravityField.radius = 0.3E-2f * Mathf.Sqrt(mass) / targetScale;

        if (gravityField.radius <= 0.8f)
        {
            gravityField.radius = 2f;
        }

        dangerZone.radius = gravityField.radius / 14f;
        if (dangerZone.radius <= 0.6f)
        {
            dangerZone.radius = 0.62f;
        }

        base.SetupObstacle();
    }

    public void Destroy(bool instantiateEffect)
    {
        if (instantiateEffect)
        {
            string deString = "Planet";
            switch(type)
            {
                case ObstacleType.PLANET:
                    deString = "Planet";
                    break;
                case ObstacleType.STAR:
                    deString = "Star";
                    break;
                case ObstacleType.WHITE_DWARF:
                    deString = "WhiteDwarf";
                    break;
                case ObstacleType.NEUTRON_STAR:
                    deString = "NeutronStar";
                    break;
            }
            gameMode.currentLevel.poolManager.Spawn("DeathEffects", deString, transform.position, Quaternion.identity);
            //switch (gameMode.GetType().Name)
            //{
            //    case "LinearMode":
            //        effect.AddComponent<LinearMovementComponent>();
            //        break;
            //}
        }
        base.DeactivateObstacle();
    }
}

