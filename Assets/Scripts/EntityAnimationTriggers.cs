using UnityEngine;

public class EntityAnimationTriggers : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    /// <summary>
    /// Get access to entity and let current entity's state know that we want to exit state
    /// </summary>
    private void CurrentStateTrigger()
    {
        entity.CurrentStateAnimationTrigger();
    }
}
