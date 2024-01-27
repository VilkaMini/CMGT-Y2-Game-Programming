using System;
using System.Collections.Generic;
using System.Linq;
using Unity.FPS.Game;
using UnityEngine;

namespace FPS.Scripts.Gameplay.ConstructionObjects
{
    public class AttackTurretObject : ConstructionObject
    {
        private Vector3 _targetPosition;
        private float _fireSpeed;
        private WeaponController m_weapon;
        [SerializeField] private Transform turretSwivel;

        void Start()
        {
            m_weapon = GetComponentInChildren<WeaponController>();
            m_weapon.Owner = gameObject;
        }
        
        private void Update()
        {
            SearchForTarget();
            if (_targetPosition != Vector3.zero)
            {
                OrientTowardsTarget();
                ShootAtTarget();
            }
        }

        private void OrientTowardsTarget()
        {
            Vector3 direction = _targetPosition - turretSwivel.position;
            direction.y = 0f; // Ensure only rotate on X-axis
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            turretSwivel.rotation = targetRotation;
        }

        private void SearchForTarget()
        {
            List<float> distances = new List<float>();
            for(int i=0; i <objectsInRange.Count; i++)
            {
                // Special check for destroyed objects
                if (objectsInRange[i] == null)
                {
                    objectsInRange.RemoveAt(i);
                    return;
                }
                distances.Add(Vector3.Distance(transform.position, objectsInRange[i].transform.position));
            }
            if (objectsInRange.Count > 0){_targetPosition = objectsInRange[distances.IndexOf(distances.Min())].transform.position;}
            else
            {
                _targetPosition = Vector3.zero;
            }
        }

        private void ShootAtTarget()
        {
            // Shoot the weapon
            bool didFire = m_weapon.HandleShootInputs(false, true, false);
        }
    }
}
