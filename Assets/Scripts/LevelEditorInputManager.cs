
using UnityEngine;

namespace Assets.Scripts
{
    class LevelEditorInputManager : MonoBehaviour
    {
        public Camera mainCamera;
        public GameObject indicatorSquare;

        int lastX, lastY;

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

            if (!Input.GetButtonDown("Fire1"))
                return;

            // Modify map state and re-render here
        }
    }
}
