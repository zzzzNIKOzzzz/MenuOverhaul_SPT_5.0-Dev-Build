using UnityEngine;
using UnityEngine.EventSystems;

namespace MenuOverhaul.Helpers
{
    internal class PlayerModelDragRotator : MonoBehaviour
    {
        public Transform Target;
        public float RotationSpeed = 0.3f;
        public float CurrentYaw;

        private bool _dragging;

        private void Update()
        {
            if (Target == null) return;

            if (Input.GetMouseButtonDown(0))
            {
                EventSystem es = EventSystem.current;
                if (es == null || !es.IsPointerOverGameObject())
                {
                    _dragging = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _dragging = false;
            }

            if (!_dragging || !Input.GetMouseButton(0)) return;

            float dx = Input.GetAxis("Mouse X") * 30f;
            if (Mathf.Approximately(dx, 0f)) return;

            CurrentYaw -= dx * RotationSpeed;
            if (CurrentYaw >= 360f) CurrentYaw -= 360f;
            else if (CurrentYaw < 0f) CurrentYaw += 360f;

            Target.localRotation = Quaternion.Euler(0f, CurrentYaw, 0f);
        }

        public void SetYaw(float yaw)
        {
            CurrentYaw = yaw;
            if (Target != null)
            {
                Target.localRotation = Quaternion.Euler(0f, CurrentYaw, 0f);
            }
        }
    }
}
