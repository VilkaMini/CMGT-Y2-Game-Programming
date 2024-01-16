using System.Collections.Generic;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(PlayerCharacterController))]
    public class PlayerConstructionManager : MonoBehaviour
    {
        public enum ConstructionState
        {
            Off,
            Empty,
            Holding
        }
        
        private PlayerInputHandler m_InputHandler;
        private PlayerCharacterController m_CharacterController;
        
        [Header("Settings")] [Tooltip("Distance from player to place the construction.")]
        public float ConstructionReach = 10f;
        
        [SerializeField]
        private List<GameObject> constructionObjectsList;
        private List<bool> constructionObjectPlacedList = new List<bool>(){};
        private ConstructionState contructionState;
        private GameObject tempObject;
        private Vector3 _constructionHitLoc;
        private Vector3 _constructionHitNormal;
        private int _constructionObjectId;
        private int _previousDrawnObjectId = 0;

        private void Start()
        {
            m_InputHandler = GetComponent<PlayerInputHandler>();
            m_CharacterController = GetComponent<PlayerCharacterController>();
            for (int i = 0; i < constructionObjectsList.Count; i++)
            {
                constructionObjectPlacedList.Add(false);
            }
        }

        void Update()
        {
            if (m_InputHandler.GetInteractInput()) SwichOnOffConstruction();
            if (contructionState == ConstructionState.Holding)
            {
                CheckforHeldObjectChange();
                DrawConstructionobject(_constructionObjectId);
            }
            if (m_InputHandler.GetInteractPlaceInput() && tempObject) PlaceConstruction();
        }
        
        private void SwichOnOffConstruction()
        {
            if (contructionState == ConstructionState.Off)
            {
                contructionState = ConstructionState.Holding;
                m_InputHandler.LockFire(true);
                return;
            }
            contructionState = ConstructionState.Off;
            m_InputHandler.LockFire(false);
            if (tempObject) Destroy(tempObject);
        }

        /// <summary>
        /// Method <b>PlaceConstruction</b> calls a place method on the construction object and cleans the temp object.
        /// </summary>
        private void PlaceConstruction()
        {
            constructionObjectPlacedList[_constructionObjectId] = true;
            tempObject = null;
        }

        private void DrawConstructionobject(int objectPrefabId)
        {
            // if construction already placed, do not draw and destroy draw if exists
            if (constructionObjectPlacedList[objectPrefabId])
            {
                if (tempObject) Destroy(tempObject);
                return;
            }
            
            (_constructionHitLoc, _constructionHitNormal) = CheckIfHitsmesh();
            
            // if draw construction has changed, draw new one
            if (_previousDrawnObjectId != objectPrefabId)
            {
                _previousDrawnObjectId = objectPrefabId;
                Destroy(tempObject);
            }

            if (_constructionHitLoc != Vector3.zero)
            {
                if (tempObject) { tempObject.transform.position = _constructionHitLoc; }
                else { tempObject = Instantiate(constructionObjectsList[objectPrefabId]); }

                tempObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, _constructionHitNormal);
            }
            else if (_constructionHitLoc == Vector3.zero)
            {
                if (tempObject) Destroy(tempObject);
            }
        }

        /// <summary>
        /// Method <b>CheckIfHitsmesh</b> checks if there is a mesh in front of the camera.
        /// </summary>
        private (Vector3, Vector3) CheckIfHitsmesh()
        {
            RaycastHit hit;
            if (Physics.Raycast(m_CharacterController.PlayerCamera.transform.position, m_CharacterController.PlayerCamera.transform.forward, out hit, ConstructionReach, ~(1<<13)))
            {
                return (hit.point, hit.normal);
            }
            return (Vector3.zero, Vector3.zero);
        }

        private void CheckforHeldObjectChange()
        {
            int constructionObjectChanged = m_InputHandler.GetConstructionObjectInput();
            if (constructionObjectChanged != 0)
            {
                _constructionObjectId = CheckForAvailableConstruction(_constructionObjectId + constructionObjectChanged);
            }
        }

        private int CheckForAvailableConstruction(int indexToCheck)
        {
            if (constructionObjectPlacedList.Count <= indexToCheck)
            {
                return  0;
            }
            if (indexToCheck < 0)
            {
                return constructionObjectsList.Count-1;
            }
            return indexToCheck;
        }
    }
}
