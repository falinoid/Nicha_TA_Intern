using UnityEngine;

public class CaveFloatingDots : MonoBehaviour
{
    [Header("Appearance")]
    public Sprite dotSprite;
    public Color dotColor = Color.white;

    [Min(0)]
    public int dotCount = 20;

    public Vector2 sizeRange = new Vector2(0.05f, 0.15f);

    [Header("Area - Relative to This Object")]
    public Vector2 minPosition = new Vector2(-5f, -2f);
    public Vector2 maxPosition = new Vector2(5f, 2f);

    [Header("Movement")]
    public Vector2 speedRange = new Vector2(0.05f, 0.2f);

    [Min(0f)]
    public float sidewaysSpeed = 0.08f;

    [Min(0f)]
    public float swaySpeed = 0.3f;

    [Header("Sorting")]
    public string sortingLayerName = "Default";
    public int orderInLayer = 0;

    private Transform[] dots;
    private float[] speeds;
    private float[] seeds;

    void Start()
    {
        if (dotSprite == null)
        {
            Debug.LogWarning("Please assign a dot sprite.", this);
            enabled = false;
            return;
        }

        dots = new Transform[dotCount];
        speeds = new float[dotCount];
        seeds = new float[dotCount];

        for (int i = 0; i < dotCount; i++)
        {
            // Create a sprite object inside this group.
            GameObject dot = new GameObject("Dot_" + i);
            dot.transform.SetParent(transform, false);

            SpriteRenderer renderer = dot.AddComponent<SpriteRenderer>();
            renderer.sprite = dotSprite;
            renderer.color = dotColor;
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = orderInLayer;

            dots[i] = dot.transform;

            // Start each dot at a random position inside the area.
            dots[i].localPosition = new Vector3(
                Random.Range(minPosition.x, maxPosition.x),
                Random.Range(minPosition.y, maxPosition.y),
                0f
            );

            seeds[i] = Random.Range(0f, 1000f);
            RandomizeDot(i);
        }
    }

    void Update()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            Vector3 position = dots[i].localPosition;

            // Use smooth noise instead of changing direction abruptly.
            float drift = Mathf.PerlinNoise(
                seeds[i],
                Time.time * swaySpeed
            ) * 2f - 1f;

            position.x += drift * sidewaysSpeed * Time.deltaTime;
            position.y += speeds[i] * Time.deltaTime;

            // Reappear at the bottom after leaving the top.
            if (position.y > maxPosition.y)
            {
                position.y = minPosition.y;
                position.x = Random.Range(minPosition.x, maxPosition.x);
                RandomizeDot(i);
            }

            // Wrap around the left and right edges.
            if (position.x > maxPosition.x)
                position.x = minPosition.x;
            else if (position.x < minPosition.x)
                position.x = maxPosition.x;

            dots[i].localPosition = position;
        }
    }

    void RandomizeDot(int index)
    {
        // Give each dot its own size and upward speed.
        float size = Random.Range(sizeRange.x, sizeRange.y);
        dots[index].localScale = Vector3.one * size;
        speeds[index] = Random.Range(speedRange.x, speedRange.y);
    }

    void OnDrawGizmosSelected()
    {
        // Draw the spawn area in the Scene view.
        Vector2 center = (minPosition + maxPosition) * 0.5f;
        Vector2 size = maxPosition - minPosition;

        Gizmos.color = Color.cyan;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(
            new Vector3(center.x, center.y, 0f),
            new Vector3(size.x, size.y, 0f)
        );
    }
}