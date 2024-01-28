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
        
        [Tooltip("Turret Horizontal Swivel Point.")]
        [SerializeField] private Transform turretSwivel;
        [Tooltip("Turret Vertical Swivel Point.")]
        [SerializeField] private Transform turretVerticalSwivel;

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
        
        /// <summary>
        /// Method <b>OrientTowardsTarget</b> orients the muzzle of the turret towards the target..
        /// </summary>
        private void OrientTowardsTarget()
        {
            Vector3 direction = _targetPosition - turretSwivel.position;
            direction.y = 0f; // Ensure only rotate on X-axis
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            turretSwivel.rotation = targetRotation;
            turretVerticalSwivel.rotation = targetRotation;
        }
        
        /// <summary>
        /// Method <b>SearchForTarget</b> searches through the trigger collision objects to find the nearest to the turret..
        /// </summary>
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

            if (objectsInRange.Count > 0)
            {
                _targetPosition = objectsInRange[distances.IndexOf(distances.Min())].transform.position;
            }
            else
            {
                _targetPosition = Vector3.zero;
            }
        }

        /// <summary>
        /// Method <b>ShootAtTarget</b> uses WeaponController to at the target.
        /// </summary>
        private void ShootAtTarget()
        {
            m_weapon.HandleShootInputs(false, true, false);
        }
    }
}
