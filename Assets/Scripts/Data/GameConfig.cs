using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : ScriptableObject
{
    public float durationSec = 60f;
    public int visibleCount = 7;
    public int maxMissAllowed = 3;
    public int scorePerHit = 1;
    public int initialQueueSize = 7;
}
