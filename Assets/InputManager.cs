using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject indicatorSquare;
    public GameObject towerPrefab;
    public int width;
    public int height;

    private int[,] gameGrid;

    private void Start()
    {
        gameGrid = new int[width,height];

        for(var i = 0; i < height; i++)
        {
            for(var j = 0; j < width; j++)
            {
                gameGrid[j,i] = 0;
            }
        }
    }

    private void Update()
    {
        var mousePos = Input.mousePosition;

        var point = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y));
        indicatorSquare.transform.position = new Vector3(Mathf.FloorToInt(point.x) + 0.5f, Mathf.FloorToInt(point.y) + 0.5f, 0);

        if (!Input.GetButtonDown("Fire1"))
            return;

        Instantiate(towerPrefab, new Vector3(Mathf.FloorToInt(point.x) + 0.5f, Mathf.FloorToInt(point.y) + 0.5f, 0), Quaternion.identity);
    }
}
