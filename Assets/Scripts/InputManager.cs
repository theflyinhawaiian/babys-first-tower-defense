using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject indicatorSquare;
    public GameObject towerPrefab;
    public GameManager gameManager;

    int lastX, lastY;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    private void Update()
    {
        var mousePos = Input.mousePosition;

        var point = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y));

        var placementX = Mathf.FloorToInt(point.x);
        var placementY = Mathf.FloorToInt(point.y);

        if(lastX != placementX || lastY != placementY)
        {
            indicatorSquare.transform.position = new Vector3(Mathf.FloorToInt(point.x) + 0.5f, Mathf.FloorToInt(point.y) + 0.5f, -1);

            var indicatorRenderer = indicatorSquare.GetComponent<SpriteRenderer>();
            indicatorRenderer.color = gameManager.CanPlaceTowerAt(placementX, placementY) ? Color.green : Color.red;
        }

        if (!Input.GetButtonDown("Fire1"))
            return;

        if (!gameManager.TryPlaceTowerAt(placementX, placementY))
            return;

        Instantiate(towerPrefab, new Vector3(placementX + 0.5f, placementY + 0.5f, 0), Quaternion.identity);
    }
}
