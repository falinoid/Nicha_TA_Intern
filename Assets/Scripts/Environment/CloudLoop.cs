using UnityEngine;

[AddComponentMenu("Custom/Cloud Loop")]
public class CloudLoop : MonoBehaviour
{
    [Tooltip("X position where the cloud reappears.")]
    public float spawnX = -15f;

    [Tooltip("X position where the cloud disappears.")]
    public float endX = 15f;

    [Tooltip("Movement speed in units per second.")]
    [Min(0f)]
    public float speed = 1f;

    void Update()
    {
        // Stop if the start and end positions are the same.
        if (spawnX == endX)
            return;

        // Move right or left based on the loop positions.
        float direction = spawnX < endX ? 1f : -1f;

        // Start moving from the cloud's current scene position.
        Vector3 position = transform.position;
        position.x += direction * speed * Time.deltaTime;

        // Return to Spawn X after reaching End X.
        if ((direction > 0f && position.x >= endX) ||
            (direction < 0f && position.x <= endX))
        {
            position.x = spawnX;
        }

        // Keep the current Y and Z positions.
        transform.position = position;
    }
}