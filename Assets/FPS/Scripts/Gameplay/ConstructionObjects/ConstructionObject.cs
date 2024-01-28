using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class ConstructionObject : MonoBehaviour
{
    public int constructionId;
    protected SphereCollider _collider;
    protected List<GameObject> objectsInRange = new List<GameObject>();
    
    [Header("Parameters")]
    [Tooltip("The range is the distance from the turret in which it works.")]
    [SerializeField] protected float range;

    private void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true;
        _collider.radius = range;   
    }

    /// <summary>
    /// Method <b>OnTriggerEnter</b> adds an object to the nearbyObject list.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            objectsInRange.Add(other.gameObject);
        }
    }

    /// <summary>
    /// Method <b>OnTriggerExit</b> removes an object from the nearbyObject list.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            objectsInRange.Remove(other.gameObject);
        }
    }
}
