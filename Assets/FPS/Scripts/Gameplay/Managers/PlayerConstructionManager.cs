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
        private List<bool> constructionObjectPlacedList;
        private ConstructionState contructionState;
        private GameObject tempObject;
        private Vector3 _constructionHitLoc;

        private void Start()
        {
            m_InputHandler = GetComponent<PlayerInputHandler>();
            m_CharacterController = GetComponent<PlayerCharacterController>();
            for (int i = 0; i < constructionObjectsList.Count; i++)
            {
                constructionObjectPlacedList.Add(true);
            }
        }

        void Update()
        {
            if (m_InputHandler.GetInteractInput()) SwichOnOffConstruction();
            if (contructionState == ConstructionState.Empty) DrawConstructionobject(0);
            if (m_InputHandler.GetInteractPlaceInput() && tempObject) PlaceConstruction();
        }
        
        private void SwichOnOffConstruction()
        {
            if (contructionState == ConstructionState.Off)
            {
                contructionState = ConstructionState.Empty;
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
            tempObject = null;
        }

        private void DrawConstructionobject(int objectPrefabId)
        {
            _constructionHitLoc = CheckIfHitsmesh();
            if (_constructionHitLoc != Vector3.zero && !tempObject)
            {
                tempObject = Instantiate(constructionObjectsList[objectPrefabId]);
            }
            else if (_constructionHitLoc != Vector3.zero && tempObject)
            {
                tempObject.transform.position = _constructionHitLoc;
            }
            else if (_constructionHitLoc == Vector3.zero)
            {
                if (tempObject) Destroy(tempObject);
            }
        }

        /// <summary>
        /// Method <b>CheckIfHitsmesh</b> checks if there is a mesh in front of the camera.
        /// </summary>
        private Vector3 CheckIfHitsmesh()
        {
            RaycastHit hit;
            if (Physics.Raycast(m_CharacterController.PlayerCamera.transform.position, m_CharacterController.PlayerCamera.transform.forward, out hit, ConstructionReach))
            {
                return hit.point;
            }
            return Vector3.zero;
        }
    }
}
