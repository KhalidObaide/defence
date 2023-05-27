using UnityEngine;

[CreateAssetMenu(fileName = "New Runner", menuName = "Runner")]
public class Runner : ScriptableObject
{
    public GameObject runnerPrefab;
    public float speed;
}
