using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    /// <summary>
    /// Get access to player and let current player's state know that we want to exit state
    /// </summary>
    private void CurrentStateTrigger()
    {
        player.CallAnimationTrigger();
    }
}
