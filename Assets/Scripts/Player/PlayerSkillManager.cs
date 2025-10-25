using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public SkillDash dash { get; private set; }

    private void Awake()
    {
        dash = GetComponentInChildren<SkillDash>();
    }
}
