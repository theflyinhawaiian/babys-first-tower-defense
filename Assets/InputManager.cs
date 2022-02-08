using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject indicatorSquare;
    public GameObject towerPrefab;

    private int[,] gameGrid;

    private void Start()
    {
        var width = GameState.width;
        var height = GameState.height;

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

        var placementX = Mathf.FloorToInt(point.x);
        var placementY = Mathf.FloorToInt(point.y);

        var gridX = GameState.GetGridX(placementX);
        var gridY = GameState.GetGridY(placementY);

        if (gameGrid[gridX, gridY] == 1)
            return;

        gameGrid[gridX, gridY] = 1;

        Instantiate(towerPrefab, new Vector3(placementX + 0.5f, placementY + 0.5f, 0), Quaternion.identity);
    }
}
