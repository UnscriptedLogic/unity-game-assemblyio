using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BuildManagement
{
    public class BuildManager : MonoBehaviour
    {
        [Header("Build Content")]
        [SerializeField] private BuildingSO[] availableBuildings;

        [Header("Settings")]
        [SerializeField] private LayerMask buildLayer;
        [SerializeField] private LayerMask buildingLayer;

        [Header("Components")]
        private InputManager inputManager;
        [SerializeField] private Grid grid;
        [SerializeField] private Camera cam;

        private void OnEnable()
        {
            inputManager = InputManager.instance;
            inputManager.OnStartBuildTapContact += OnStartBuildTapContact;
        }

        private void OnDisable()
        {
            inputManager.OnStartBuildTapContact -= OnStartBuildTapContact;
        }

        private void OnStartBuildTapContact(Vector2 position)
        {
            if (!inputManager.IsTapOverUI(position))
            {
                Vector3 potentialBuildPos = GetWorldPosition(position);

                Ray ray = cam.ScreenPointToRay(new Vector3(position.x, position.y, cam.transform.position.z));
                if (Physics.Raycast(ray, out RaycastHit hitinfo, 100f, buildingLayer))
                {
                    potentialBuildPos = grid.CellToWorld(grid.WorldToCell(hitinfo.transform.position + hitinfo.normal.normalized));
                }

                if (!Physics.CheckSphere(potentialBuildPos, 0.25f, buildingLayer))
                {
                    GameObject building = Instantiate(availableBuildings[0].Prefab, potentialBuildPos, Quaternion.identity);
                }
            }
        }

        private Vector3 GetWorldPosition(Vector2 tapPos)
        {
            Ray ray = cam.ScreenPointToRay(new Vector3(tapPos.x, tapPos.y, cam.transform.position.z));
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, buildLayer))
            {
                Vector3Int cellPos = grid.WorldToCell(hit.point);
                return grid.CellToWorld(cellPos);
            }

            return Vector3.zero;
        }
    }
}