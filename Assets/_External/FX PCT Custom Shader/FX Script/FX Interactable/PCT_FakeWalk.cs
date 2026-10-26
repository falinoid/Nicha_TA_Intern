using UnityEngine;
using UnityEngine.InputSystem;

public class PCT_FakeWalk : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.isPressed)  input.x -= 1f;
            if (Keyboard.current.rightArrowKey.isPressed) input.x += 1f;
            if (Keyboard.current.downArrowKey.isPressed)  input.y -= 1f;
            if (Keyboard.current.upArrowKey.isPressed)    input.y += 1f;
        }

        if (input != Vector2.zero)
        {
            input.Normalize();
            Vector3 moveDelta = new Vector3(input.x, input.y, 0f) * moveSpeed * Time.deltaTime;
            transform.Translate(moveDelta, Space.World);
        }
    }
}
