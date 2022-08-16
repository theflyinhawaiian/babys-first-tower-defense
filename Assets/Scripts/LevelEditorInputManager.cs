
using UnityEngine;

namespace Assets.Scripts
{
    class LevelEditorInputManager : MonoBehaviour
    {
        public Camera mainCamera;
        public GameObject indicatorSquare;

        LevelEditorManager manager;

        int lastX, lastY;

        private void Start()
        {
            manager = GetComponent<LevelEditorManager>();
        }

        private void Update()
        {
            var mousePos = Input.mousePosition;

            var point = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y));

            var placementX = Mathf.FloorToInt(point.x);
            var placementY = Mathf.FloorToInt(point.y);

            if (lastX != placementX || lastY != placementY) {
                indicatorSquare.transform.position = new Vector3(Mathf.FloorToInt(point.x) + 0.5f, Mathf.FloorToInt(point.y) + 0.5f, -1);

                lastX = placementX;
                lastY = placementY;
            }

            if (!Input.GetButton("Fire1") || !IsInbounds(placementX, placementY))
                return;

            manager.SetGridValue(placementX, placementY);
        }

        private bool IsInbounds(int x, int y) => x >= 0 && x < GameConstants.Width && y >= 0 && y < GameConstants.Height;
    }
}
