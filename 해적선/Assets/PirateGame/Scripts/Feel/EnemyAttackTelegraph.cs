using PirateGame.Feel;
using UnityEngine;
namespace PirateGame.Feel
{
    public sealed class EnemyAttackTelegraph : MonoBehaviour
    {
        // 실습 TODO: Docs/Tutorial02의 해당 단원을 따라 직접 구현합니다.
        // 저장한 목표 위치까지 적 공격 예고선을 표시하고 숨깁니다.
        public LineRenderer line;
        public void Show(Vector2 origin, Vector2 target)
        {
            line.enabled = true; line.positionCount = 2;
            line.SetPosition(0, origin); line.SetPosition(1, target);
        }
        public void Hide() { if (line) line.enabled = false; }
        void OnDisable() => Hide();
    }
}

