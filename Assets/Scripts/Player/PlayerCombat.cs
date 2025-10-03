using UnityEngine;

public class PlayerCombat : EntityCombat
{
    [Header("Counter attack details")] 
    [SerializeField] private float counterRecovery = 0.1f;

    public bool IsCounterAttackPerformed()
    {
        bool hasPerformedCounter = false;

        foreach (var target in GetDetectedColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();
            if (counterable == null)
            {
                continue;
            }

            if (counterable.CanBeCountered)
            {
                counterable.HandleCounter();
                hasPerformedCounter = true;
            }
        }

        return hasPerformedCounter;
    }

    public float GetCounterRecoveryDuration()
    {
        return counterRecovery;
    }
}
