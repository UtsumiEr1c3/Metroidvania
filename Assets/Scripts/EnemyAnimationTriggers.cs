using UnityEngine;

public class EnemyAnimationTriggers : EntityAnimationTriggers
{
    private Enemy enemy;
    private EnemyVFX enemyVfx;

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponentInParent<Enemy>();
        enemyVfx = GetComponentInParent<EnemyVFX>();
    }

    private void EnableCounterWindow()
    {
        enemy.EnableConterWindow(true);
        enemyVfx.EnableAttackAlert(true);
    }

    private void DisableCounterWindow()
    {
        enemy.EnableConterWindow(false);
        enemyVfx.EnableAttackAlert(false);
    }
}
