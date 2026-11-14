using UnityEngine;
using PirateGame.Feel;
namespace PirateGame
{
    [RequireComponent(typeof(Rigidbody2D), typeof(ShipHealth))]
    public sealed class ShipMotor : MonoBehaviour
    {
        public ShipHandlingSettings handling;
        float currentSpeed;
        public float forwardSpeed = 3.5f;
        public float reverseSpeed = 1.75f;
        public float turnSpeed = 110f;
        public Vector2 arenaHalfSize = new Vector2(12, 6.5f);
        Rigidbody2D body;
        ShipHealth health;
        float throttle, steering;
        void Awake() { body = GetComponent<Rigidbody2D>(); health = GetComponent<ShipHealth>(); }
        public void SetInput(float forward, float turn) { throttle = Mathf.Clamp(forward, -1, 1); steering = Mathf.Clamp(turn, -1, 1); }
        void FixedUpdate()
        {
            if (!health.IsAlive) return;
            float dt = Time.fixedDeltaTime;
            float actualSpeed = Vector2.Dot(body.linearVelocity, transform.up);
            currentSpeed = actualSpeed;
            float desired = throttle * (throttle >= 0 ? forwardSpeed : reverseSpeed);
            float rate = 0;
            if (handling)
            {
                bool opposing = Mathf.Abs(currentSpeed) > 0.05f && currentSpeed * throttle < 0;
                rate = opposing ? handling.braking : Mathf.Abs(throttle) < 0.01f ? handling.deceleration : handling.acceleration;
            }
            currentSpeed = handling ? Mathf.MoveTowards(currentSpeed, desired, Mathf.Max(0, rate) * dt) : desired;
            float speed01 = Mathf.Clamp01(Mathf.Abs(currentSpeed) / Mathf.Max(0.01f, forwardSpeed));
            float turning = handling ? Mathf.Lerp(handling.lowSpeedTurn, handling.highSpeedTurn, speed01) : turnSpeed;
            float angle = body.rotation - steering * turning * dt;
            body.MoveRotation(angle);
            Vector2 forward = Quaternion.Euler(0, 0, angle) * Vector2.up;
            body.linearVelocity = forward * currentSpeed;
            Vector2 p = body.position;
            Vector2 bounded = new Vector2(Mathf.Clamp(p.x, -arenaHalfSize.x + 0.6f, arenaHalfSize.x - 0.6f), Mathf.Clamp(p.y, -arenaHalfSize.y + 0.6f, arenaHalfSize.y - 0.6f));
            if (p != bounded) body.position = bounded;
        }
        void OnDisable() { if (body) { body.linearVelocity = Vector2.zero; body.angularVelocity = 0; } }
    }
}