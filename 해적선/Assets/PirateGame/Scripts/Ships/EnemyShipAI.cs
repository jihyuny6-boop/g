using UnityEngine;
using PirateGame.Feel;
namespace PirateGame
{
    [RequireComponent(typeof(ShipMotor), typeof(CannonWeapon), typeof(ShipHealth))]
    public sealed class EnemyShipAI : MonoBehaviour
    {
        public ShipHealth target;
        public float preferredDistance = 4.2f;
        ShipMotor motor; CannonWeapon weapon; ShipHealth health;
        EnemyAttackController attack;
        void Awake() { motor = GetComponent<ShipMotor>(); weapon = GetComponent<CannonWeapon>(); health = GetComponent<ShipHealth>(); attack = GetComponent<EnemyAttackController>(); }
        void Update()
        {
            if (!health.IsAlive || !target || !target.IsAlive) { motor.SetInput(0, 0); if (attack) attack.Cancel(); return; }
            Vector2 delta = target.transform.position - transform.position;
            float angle = Vector2.SignedAngle(transform.up, delta);
            float turn = -Mathf.Clamp(angle / 35f, -1, 1);
            float forward = delta.magnitude > preferredDistance ? (Mathf.Abs(angle) < 70 ? 1f : 0.2f) : 0f;
            motor.SetInput(forward, turn);
            if (attack && attack.enabled) attack.TickAttack(target);
            else weapon.TryFire(target.transform.position);
        }
    }
}