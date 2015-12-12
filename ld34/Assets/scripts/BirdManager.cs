using UnityEngine;
using System.Collections;

public class BirdManager : MonoBehaviour {
    public static BirdManager instance;

    public float reactionSpeed = 3.76f;

    [Header("Swarm Weights")]
    public float centerMassWeight = 1f;
    public float avoidBirdsWeight = 1f;
    public float velocityWeight = 1f;
    public float aboidObstacleWeight = 1f;
    public float foodWeight = 1f;
    public float dangerDistance;

    void Awake() {
        instance = this;
    }
}
