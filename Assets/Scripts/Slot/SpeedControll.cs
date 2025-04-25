using UnityEngine;

[System.Serializable]
public class SpeedControll
{
    public float speed = 30f;
    public float timeSpin = 3f;
    [Range(0, 10)] public float endSpeed = 5;
    public float minSpeed = 5;
}
