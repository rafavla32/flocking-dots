﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;
using Unity.Mathematics;

public class FlockManager : MonoBehaviour
{
    public static EntityManager entityManager;

    [Header ("Flocking Algorithm Attributes")]
    [SerializeField] private int numberOfEntities = 10;
    [SerializeField] private float boundSize = 10;

    [Space] [Header ("Fish Rendering")]
    [SerializeField] private Mesh fishMesh;
    [SerializeField] private Material fishMat;
    [Space] [Header ("Boid Properties")]
    [SerializeField] private float boidSpeed = 1f;
    [SerializeField] private float boidMaxSpeed = 5f;
    [SerializeField] private float boidRotSpeed = 0.5f;
    [SerializeField] private float coheDis = 3f;
    [SerializeField] private float avoidDis = 1.5f;

    void Start()
    {
        entityManager = World.Active.EntityManager;

        EntityArchetype entityArchetype = entityManager.CreateArchetype (
            typeof (BoidComponent),
            typeof (Translation),
            typeof (Rotation),
            typeof (RenderMesh),
            typeof (LocalToWorld)
        );
        NativeArray <Entity> entityArray = new NativeArray<Entity> (numberOfEntities,Allocator.Temp);
        entityManager.CreateEntity (entityArchetype, entityArray);
        foreach (var entity in entityArray)
        {
            entityManager.SetComponentData (entity, 
                new BoidComponent {
                       speed = boidSpeed,
                       maxSpeed = boidMaxSpeed,
                       rotationSpeed = boidRotSpeed, 
                       cohesionDistance = coheDis, 
                       avoidanceDistance = avoidDis, 
                       returning = false, 
                    }
                );
            entityManager.SetComponentData (entity, 
                new Translation {
                    Value = new float3 (
                        UnityEngine.Random.Range (-boundSize, boundSize),
                        UnityEngine.Random.Range (-boundSize, boundSize),
                        UnityEngine.Random.Range (-boundSize, boundSize)
                    )
                }
            );
            entityManager.SetSharedComponentData (entity, new RenderMesh {
                mesh = fishMesh,
                material = fishMat
            });
        }
    }
}