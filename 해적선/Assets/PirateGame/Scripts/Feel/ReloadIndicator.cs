using UnityEngine;
namespace PirateGame.Feel
{
    public sealed class ReloadIndicator : MonoBehaviour
    {
        // 실습 TODO: Docs/Tutorial02의 해당 단원을 따라 직접 구현합니다.
        // 조준점 주변에 재장전을 표시하고 완료음을 한 번 재생합니다.
        public CannonAim aim;
        public CannonWeapon weapon;
        public ShipHealth health;
        public LineRenderer ring;
        public AudioSource audioSource;
        public AudioClip readySound;
        public float radius = 0.36f;
        public int segments = 40;
        CombatFeedbackEvents events;
        bool wasReloading;
        void OnEnable()
        {
            events = FindFirstObjectByType<CombatFeedbackEvents>();
            if (events) events.ShotFired += OnShot;
            wasReloading = false;
        }
        void OnShot(ShotFeedback shot) { if (shot.Shooter == transform) wasReloading = true; }
        void LateUpdate()
        {
            bool visible = aim && aim.enabled && health.IsAlive;
            ring.enabled = visible;
            if (!visible) return;
            float progress = weapon.Reload01;
            if (wasReloading && progress >= 1)
            {
                wasReloading = false;
                if (audioSource && readySound) audioSource.PlayOneShot(readySound, 0.4f);
            }
            Vector3 center = (Vector2)transform.position + aim.Direction * Mathf.Min(aim.Distance, aim.range);
            int points = Mathf.Max(8, segments);
            ring.loop = false; ring.positionCount = points + 1;
            for (int i = 0; i <= points; i++)
            {
                float angle = Mathf.PI * 0.5f + (float)i / points * Mathf.PI * 2 * progress;
                ring.SetPosition(i, center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius);
            }
            Color color = !aim.InRange ? new Color(1, 0.35f, 0.25f) :
                wasReloading ? new Color(1, 0.78f, 0.37f) : new Color(0.6f, 1, 0.8f);
            ring.startColor = ring.endColor = color;
        }
        void OnDisable()
        {
            if (events) events.ShotFired -= OnShot;
            if (ring) ring.enabled = false;
            wasReloading = false;
        }
    }
}
