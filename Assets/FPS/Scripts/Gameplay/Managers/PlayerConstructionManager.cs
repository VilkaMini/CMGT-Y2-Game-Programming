using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Game;

namespace Unity.FPS.Gameplay
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerConstructionManager : MonoBehaviour
    {
        public enum ConstructionState
        {
            Empty,
            Holding
        }
        
        PlayerInputHandler m_InputHandler;
        private ConstructionState contructionState;

        private void Start()
        {
            m_InputHandler = GetComponent<PlayerInputHandler>();
        }

        void Update()
        {
            if (m_InputHandler.GetInteractInput())
            {
                print("Interacted");
            }
        }
    }
}