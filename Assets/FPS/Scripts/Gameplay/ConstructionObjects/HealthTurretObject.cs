using Unity.FPS.Game;
using UnityEngine;

public class HealthTurretObject : ConstructionObject
{
    ActorsManager m_ActorsManager;
    private Vector3 _playerPos;
    private Health _playerHealth;
    
    [Tooltip("The healAmount is the amount the player is going to be healed per frame.")]
    [SerializeField]
    private float healAmount = 0.1f;
    
    void Start()
    {
        m_ActorsManager = GameObject.FindObjectOfType<ActorsManager>();
        _playerHealth = m_ActorsManager.Player.GetComponent<Health>();
    }
    
    void Update()
    {
        HealPlayer();
    }

    void HealPlayer()
    {
        _playerPos = m_ActorsManager.Player.transform.position;
        if (Vector3.Distance(transform.position, _playerPos) < range && _playerHealth.CanPickup())
        {
            _playerHealth.Heal(healAmount);
        }
    }
}
