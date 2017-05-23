using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScaler : MonoBehaviour
{
    public Vector2 StandardSize = new Vector2(1920, 1080);

    // Use this for initialization
    void Start()
    {
        var w = Screen.width;
        var h = Screen.height;
        var factor = 1f;
        if (w / h > StandardSize.x / StandardSize.y)
        {
            factor = w / StandardSize.x;
        }
        else
            factor = h / StandardSize.y;

        GetComponent<CanvasScaler>().scaleFactor = factor;
    }
}
