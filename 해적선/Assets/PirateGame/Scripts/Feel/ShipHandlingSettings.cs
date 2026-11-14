using UnityEngine;
namespace PirateGame.Feel
{
    [CreateAssetMenu(menuName = "Pirate Game/Feel/Ship Handling")]
    public sealed class ShipHandlingSettings : ScriptableObject
    {
        public float acceleration = 3f, deceleration = 4f, braking = 6f;
        public float lowSpeedTurn = 110f, highSpeedTurn = 65f;
    }
}
