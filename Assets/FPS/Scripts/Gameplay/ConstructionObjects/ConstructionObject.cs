using System;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ConstructionObject : MonoBehaviour
{
    public enum ConstructionStatus
    {
        Ghost,
        Built
    }
    
    public int constructionId;
    protected SphereCollider _collider;
    [SerializeField] protected Collider rigidCollider;
    public ConstructionStatus Status;
    protected ActorsManager m_ActorsManager;
    
    [Header("Parameters")]
    [Tooltip("The range is the distance from the turret in which it works.")]
    [SerializeField] protected float range;

    private void Awake()
    {
        m_ActorsManager = GameObject.FindObjectOfType<ActorsManager>();
        rigidCollider.enabled = false;
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
        _collider.radius = range;
        Status = ConstructionStatus.Ghost;
    }

    public void SetupPlacement()
    {
        Status = ConstructionStatus.Built;
        if (rigidCollider)
        {
            rigidCollider.enabled = true;
        }
    }
}
