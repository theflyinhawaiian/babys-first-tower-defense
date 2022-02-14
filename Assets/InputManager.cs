using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject indicatorSquare;
    public GameObject towerPrefab;

    int lastX, lastY;

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
            indicatorRenderer.color = GameState.Instance.isOccupied(placementX, placementY) ? Color.red : Color.green;
        }

        if (!Input.GetButtonDown("Fire1"))
            return;

        if (!GameState.Instance.TryPlace(placementX, placementY))
            return;

        Instantiate(towerPrefab, new Vector3(placementX + 0.5f, placementY + 0.5f, 0), Quaternion.identity);
    }
}
