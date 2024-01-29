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
            if (Status == ConstructionStatus.Built)
            {
                SearchForTarget();
                if (_targetPosition != Vector3.zero)
                {
                    OrientTowardsTarget();
                    ShootAtTarget();
                }
            }
        }
        
        /// <summary>
        /// Method <b>OrientTowardsTarget</b> orients the muzzle of the turret towards the target..
        /// </summary>
        private void OrientTowardsTarget()
        {
            // Horizontal
            Vector3 direction = _targetPosition - turretSwivel.position;
            direction.y = 0f; // Ensure only rotate on X-axis
            Quaternion targetHorizontalRotation = Quaternion.LookRotation(direction);
            turretSwivel.rotation = targetHorizontalRotation;
            
            // Vertical
            Vector3 directionV = _targetPosition - turretVerticalSwivel.position;
            Quaternion targetVerticalRotation = Quaternion.LookRotation(directionV);
            turretVerticalSwivel.rotation = targetVerticalRotation;
        }
        
        /// <summary>
        /// Method <b>SearchForTarget</b> searches through the trigger collision objects to find the nearest to the turret..
        /// </summary>
        private void SearchForTarget()
        {
            List<Actor> actors = new List<Actor>();
            List<float> distances = new List<float>();
            for (int i = 0; i < m_ActorsManager.Actors.Count; i++)
            {
                Actor actor = m_ActorsManager.Actors[i];
                if (actor.Affiliation == 0)
                {
                    float dist = Vector3.Distance(transform.position, actor.transform.position);
                    if (dist < range)
                    {
                        actors.Add(actor);
                        distances.Add(dist);
                    }
                }
            }
            if (distances.Count > 0)
            {
                _targetPosition = actors[distances.IndexOf(distances.Min())].AimPoint.transform.position;
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
