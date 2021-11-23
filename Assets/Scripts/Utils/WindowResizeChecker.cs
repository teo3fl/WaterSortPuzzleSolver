using UnityEngine;

public class WindowResizeChecker : MonoBehaviour
{
    Vector2 windowSize;


    void Start()
    {
        windowSize.x = Screen.width;
        windowSize.y = Screen.height;
    }

    void Update()
    {
        if (windowSize.x != Screen.width || windowSize.y != Screen.height)
        {
            windowSize.x = Screen.width;
            windowSize.y = Screen.height;

            BeakerContainer.Instance.OnWindowResize();
            ColorContainer.Instance.OnWindowResize();
        }
    }
}
