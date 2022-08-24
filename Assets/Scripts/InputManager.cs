using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject indicatorSquare;
    public GameManager gameManager;

    int lastX, lastY;
    int UILayer;

    private void Start()
    {
        UILayer = LayerMask.NameToLayer("UI");
        gameManager = GetComponent<GameManager>();
    }

    private void Update()
    {
        var mousePos = Input.mousePosition;

        var point = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y));

        var placementX = Mathf.FloorToInt(point.x);
        var placementY = Mathf.FloorToInt(point.y);

        if(gameManager.placeMode && (lastX != placementX || lastY != placementY))
        {
            indicatorSquare.transform.position = new Vector3(Mathf.FloorToInt(point.x) + 0.5f, Mathf.FloorToInt(point.y) + 0.5f, -1);

            var indicatorRenderer = indicatorSquare.GetComponent<SpriteRenderer>();
            indicatorRenderer.color = gameManager.CanPlaceTowerAt(placementX, placementY) ? Color.green : Color.red;
        }

        if (!Input.GetButtonDown("Fire1"))
            return;

        if (IsPointerOverUIElement())
            return;

        gameManager.ProcessClick(placementX, placementY);
    }

    private bool IsPointerOverUIElement()
    {
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        for (int index = 0; index < raycastResults.Count; index++) {
            var r = raycastResults[index];
            if (r.gameObject.layer == UILayer)
                return true;
        }
        return false;
    }
}
