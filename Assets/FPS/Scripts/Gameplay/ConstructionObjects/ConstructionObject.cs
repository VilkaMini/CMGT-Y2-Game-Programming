using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ConstructionObject : MonoBehaviour
{
    protected SphereCollider _collider;
    [SerializeField]protected List<GameObject> objectsInRange = new List<GameObject>();
    
    [SerializeField] protected float range;

    private void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
        _collider.radius = range;   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            objectsInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            objectsInRange.Remove(other.gameObject);
        }
    }
}
