using UnityEngine;
using System.Collections;

namespace CameraManagement
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private float panspeed = 5f;
        [SerializeField] private float zoomSpeed = 5f;
        [SerializeField] private float deltaZoomCap = 200f;
        [SerializeField] private Vector2 xpanLimits;
        [SerializeField] private Vector2 zpanLimits;
        [SerializeField] private Vector2 zoomLimits;

        private InputManager inputManager;
        private Coroutine panCoroutine;
        private Coroutine zoomCoroutine;

        private void Awake()
        {
            inputManager = InputManager.instance;
        }

        private void OnEnable()
        {
            inputManager.OnStartPrimaryTapContact += StartPrimaryTapContact;
            inputManager.OnEndPrimaryTapContact += OnEndPrimaryTapContact;

            inputManager.OnStartSecondaryTapContact += StartSecondaryTapContact;
            inputManager.OnEndSecondaryTapContact += EndSecondaryTapContact;
        }

        private void OnDisable()
        {
            inputManager.OnStartPrimaryTapContact -= StartPrimaryTapContact;
            inputManager.OnEndPrimaryTapContact -= OnEndPrimaryTapContact;
        }

        private void StartSecondaryTapContact(Vector2 tapPos)
        {
            StopAllCoroutines();
            zoomCoroutine = StartCoroutine(ZoomCamera());
        }

        private void EndSecondaryTapContact()
        {
            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
            }
        }

        private void StartPrimaryTapContact(Vector2 tapPos)
        {
            StopAllCoroutines();
            panCoroutine = StartCoroutine(PanCamera());

        }
 
        private void OnEndPrimaryTapContact()
        {
            if (panCoroutine != null)
                StopCoroutine(panCoroutine);
        }

        private IEnumerator PanCamera()
        {
            while (true)
            {
                Vector3 pos = transform.position;
                pos.x -= inputManager.PrimaryHoldDeltaAction.ReadValue<Vector2>().x * panspeed * Time.deltaTime;
                pos.z -= inputManager.PrimaryHoldDeltaAction.ReadValue<Vector2>().y * panspeed * Time.deltaTime;

                pos.x = Mathf.Clamp(pos.x, xpanLimits.x, xpanLimits.y);
                pos.z = Mathf.Clamp(pos.z, zpanLimits.x, zpanLimits.y);

                transform.position = pos;

                yield return null;
            }
        }

        private IEnumerator ZoomCamera()
        {
            float prevFingerDist = 0f;
            while (true)
            {
                float currFingerDist = Vector2.Distance(inputManager.SecondaryTapPositionAction.ReadValue<Vector2>(), inputManager.PrimaryTapPositionAction.ReadValue<Vector2>());
                float deltaSpeed = deltaZoomCap * 0.01f * Mathf.Max(inputManager.PrimaryTapPositionAction.ReadValue<Vector2>().magnitude, inputManager.SecondaryTapPositionAction.ReadValue<Vector2>().magnitude);

                //Zoom out
                if (currFingerDist < prevFingerDist)
                {
                    Vector3 targetPos = transform.position - transform.forward * currFingerDist * (zoomSpeed * (deltaSpeed * 0.01f)) * Time.deltaTime;
                    targetPos.y = Mathf.Clamp(targetPos.y, zoomLimits.x, zoomLimits.y);
                    targetPos.z = Mathf.Clamp(targetPos.z, zpanLimits.x, zpanLimits.y);
                    transform.position = targetPos;
                }

                //Zoom in
                else if (currFingerDist > prevFingerDist)
                {
                    Vector3 targetPos = transform.position + transform.forward * currFingerDist * (zoomSpeed * (deltaSpeed * 0.01f)) * Time.deltaTime;
                    targetPos.y = Mathf.Clamp(targetPos.y, zoomLimits.x, zoomLimits.y);
                    targetPos.z = Mathf.Clamp(targetPos.z, zpanLimits.x, zpanLimits.y);
                    transform.position = targetPos;
                }

                prevFingerDist = currFingerDist;
                yield return null;
            }
        }
    }
}