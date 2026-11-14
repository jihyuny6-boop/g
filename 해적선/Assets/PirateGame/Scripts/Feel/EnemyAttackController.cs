using UnityEngine;
namespace PirateGame.Feel
{
    [RequireComponent(typeof(CannonWeapon), typeof(ShipHealth))]
    public sealed class EnemyAttackController : MonoBehaviour
    {
        public EnemyAttackTelegraph telegraph;
        public float warningSeconds = 0.55f;
        CannonWeapon weapon;
        ShipHealth health, target;
        EnemyShipAI ai;
        BattleController battle;
        Vector2 lockedTarget;
        float fireAt, retryAt;
        bool pending;
        void Awake()
        {
            weapon = GetComponent<CannonWeapon>(); health = GetComponent<ShipHealth>();
            ai = GetComponent<EnemyShipAI>(); battle = FindFirstObjectByType<BattleController>();
        }
        public void TickAttack(ShipHealth value)
        {
            if (!isActiveAndEnabled || !health.IsAlive || !value || !value.IsAlive || !battle || !battle.Running)
            { Cancel(); return; }
            target = value;
            if (pending)
            {
                telegraph.Show(transform.position, lockedTarget);
                if (Time.time < fireAt) return;
                bool fired = weapon.TryFire(lockedTarget);
                Cancel(); retryAt = Time.time + (fired ? 0 : 0.2f);
                return;
            }
            float distance = Vector2.Distance(transform.position, value.transform.position);
            if (Time.time < retryAt || weapon.Reload01 < 1 || distance < 0.7f || distance > weapon.range) return;
            lockedTarget = value.transform.position;
            fireAt = Time.time + Mathf.Max(0.05f, warningSeconds);
            pending = true; telegraph.Show(transform.position, lockedTarget);
        }
        void Update()
        {
            if (pending && (!health.IsAlive || !target || !target.IsAlive || !ai || !ai.enabled || !battle || !battle.Running)) Cancel();
        }
        public void Cancel() { pending = false; target = null; if (telegraph) telegraph.Hide(); }
        void OnDisable() => Cancel();
    }
}