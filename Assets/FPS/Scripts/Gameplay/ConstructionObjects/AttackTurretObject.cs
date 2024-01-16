using System;
using UnityEngine;

namespace FPS.Scripts.Gameplay.ConstructionObjects
{
    public class AttackTurretObject : ConstructionObject
    {
        private Vector3 _targetPosition;
        private float _fireSpeed;

        private void Update()
        {
            SearchForTarget();
            if (_targetPosition != Vector3.zero)
            {
                ShootAtTarget();
            }
        }

        private void SearchForTarget()
        {
            for(int i=0; i <objectsInRange.Count; i++)
            {
                if (!objectsInRange[i])
                {
                    objectsInRange.RemoveAt(i);
                    _targetPosition = Vector3.zero;
                    return;
                }
                if (Vector3.Distance(transform.position, objectsInRange[i].transform.position) < Vector3.Distance(transform.position, _targetPosition))
                {
                    _targetPosition = objectsInRange[i].transform.position;
                }
            }
        }

        private void ShootAtTarget()
        {
            Debug.Log("Shooting at target");
        }
    }
}
