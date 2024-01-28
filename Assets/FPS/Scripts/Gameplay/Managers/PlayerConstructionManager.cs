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
            Holding
        }
        
        private PlayerInputHandler m_InputHandler;
        private PlayerCharacterController m_CharacterController;
        
        [Header("Settings")] 
        [Tooltip("Distance from player to place the construction.")]
        public float ConstructionReach = 10f;
        
        [Tooltip("Construction Objects to choose from.")]
        [SerializeField] private List<GameObject> constructionObjectsList;
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
            else if (m_InputHandler.GetInteractPlaceInput()) RemoveConstruction();
        }
        
        /// <summary>
        /// Method <b>SwichOnOffConstruction</b> turns on or off construction system. Locks the ability to fire.
        /// </summary>
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

        /// <summary>
        /// Method <b>DrawConstructionobject</b> draws the ghost of the construction object if mesh is hit.
        /// <param name="objectPrefabId">int index representing object prefab id in the list.</param>>
        /// </summary>
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
                else { 
                    tempObject = Instantiate(constructionObjectsList[objectPrefabId]);
                    tempObject.GetComponent<ConstructionObject>().constructionId = objectPrefabId;
                }

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

        private GameObject CheckIfHitsConstruction()
        {
            RaycastHit hit;
            if (Physics.Raycast(m_CharacterController.PlayerCamera.transform.position, m_CharacterController.PlayerCamera.transform.forward, out hit, ConstructionReach, (1<<13)))
            {
                return hit.transform.gameObject;
            }
            return null;
        }
        
        /// <summary>
        /// Method <b>CheckforHeldObjectChange</b> checks if the player switches to another construction object.
        /// </summary>
        private void RemoveConstruction()
        {
            GameObject construction = CheckIfHitsConstruction();
            if (construction)
            {
                constructionObjectPlacedList[construction.GetComponent<ConstructionObject>().constructionId] = false;
                Destroy(construction);
            }
        }
        
        /// <summary>
        /// Method <b>CheckforHeldObjectChange</b> checks if the player switches to another construction object.
        /// </summary>
        private void CheckforHeldObjectChange()
        {
            int constructionObjectChanged = m_InputHandler.GetConstructionObjectInput();
            if (constructionObjectChanged != 0)
            {
                _constructionObjectId = CheckForAvailableConstruction(_constructionObjectId + constructionObjectChanged);
            }
        }

        /// <summary>
        /// Method <b>CheckForAvailableConstruction</b> checks the next available construction object.
        /// <param name="indexToCheck">int index to check next cosntruction object.</param>>
        /// </summary>
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
