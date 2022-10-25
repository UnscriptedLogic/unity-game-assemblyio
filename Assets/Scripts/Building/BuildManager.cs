using LevelManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BuildManagement
{
    public class BuildManager : MonoBehaviour
    {
        [Header("Build")]
        [SerializeField] private BuildingSO[] availableBuildings;
        private BuildingSO currentBuilding;

        [Header("Settings")]
        [SerializeField] private LayerMask buildLayer;
        [SerializeField] private LayerMask buildingLayer;

        private bool canInspect;
        private bool canBuild;
        private bool canRemove;
        private bool canRotate;

        [Header("Components")]
        private InputManager inputManager;
        [SerializeField] LevelManager levelManager;
        [SerializeField] private Grid grid;
        [SerializeField] private Camera cam;
        [SerializeField] private GameObject buildingButtonPrefab;
        [SerializeField] private Transform buildingParent;
        [SerializeField] private Button buildButton;
        [SerializeField] private Button removeButton;
        [SerializeField] private Button rotateButton;
        [SerializeField] private Button doneButton;
        [SerializeField] private GameObject gridVisual;

        private void Start()
        {
            doneButton.onClick.AddListener(OnDoneButtonPressed);

            buildButton.onClick.AddListener(OnBuildMode);
            rotateButton.onClick.AddListener(OnRotateMode);
            removeButton.onClick.AddListener(OnRemoveMode);

            for (int i = 0; i < buildingParent.childCount; i++)
            {
                Destroy(buildingParent.GetChild(i));
            }

            for (int i = 0; i < availableBuildings.Length; i++)
            {
                GameObject buildingButton = Instantiate(buildingButtonPrefab, buildingParent);
                BuildingButton buildingButtonScript = buildingButton.GetComponent<BuildingButton>();
                BuildingSO buildingSO = availableBuildings[i];
                buildingButtonScript.UpdateButton(buildingSO, () =>
                {
                    currentBuilding = buildingSO;
                    canBuild = true;
                });
            }
        }

        private void OnEnable()
        {
            inputManager = InputManager.instance;
            inputManager.OnStartBuildTapContact += OnStartTapContact;
        }

        private void OnDisable()
        {
            inputManager.OnStartBuildTapContact -= OnStartTapContact;
        }

        public void OnEnterConstructMode()
        {
            gridVisual.SetActive(true);
        }

        private void OnExitConstructMode()
        {
            gridVisual.SetActive(false);
        }

        private void OnDoneButtonPressed()
        {
            canBuild = false;
            canRotate = false;
            canRemove = false;

            if (UINavigator.GetTopPageName() == "Construct")
            {
                UINavigator.Pop(); //Pops construct page
                UINavigator.PopAndPush("Main"); //Pops persistent page
                OnExitConstructMode();
                return;
            }

            UINavigator.PopAndPush("Construct");
        }

        private void OnBuildMode()
        {
            UINavigator.PopAndPush("BuildMode");
        }

        private void OnRotateMode()
        {
            UINavigator.PopAndPush("RotateMode");
            canRotate = true;
        }

        private void OnRemoveMode()
        {
            UINavigator.PopAndPush("Remove");
        }

        private void OnStartTapContact(Vector2 position)
        {
            if (!inputManager.IsTapOverUI(position))
            {
                Vector3 potentialBuildPos = GetWorldPosition(position);

                Ray ray = cam.ScreenPointToRay(new Vector3(position.x, position.y, cam.transform.position.z));

                if (canInspect)
                {

                }

                if (canBuild)
                {

                    BuildTower(potentialBuildPos, ray);
                }

                if (canRotate)
                {
                    RotateTower(potentialBuildPos);
                }

                if (canRemove)
                {

                }
            }
        }

        private void BuildTower(Vector3 potentialBuildPos, Ray ray)
        {
            //Uses existing building as correction point
            if (Physics.Raycast(ray, out RaycastHit hitinfo, 100f, buildingLayer))
            {
                potentialBuildPos = grid.CellToWorld(grid.WorldToCell(hitinfo.transform.position + hitinfo.normal.normalized)) + new Vector3(0.5f, 0f, 0.5f);
            }

            if (!Physics.CheckSphere(potentialBuildPos, 0.25f, buildingLayer))
            {
                GameObject building = Instantiate(currentBuilding.Prefab, potentialBuildPos, Quaternion.identity);
            }
        }

        private void RotateTower(Vector3 potentialTowerPosition)
        {
            Physics.OverlapSphere(potentialTowerPosition, 0.25f, buildingLayer)[0].transform.rotation *= Quaternion.Euler(0, 90, 0);
        }

        private Vector3 GetWorldPosition(Vector2 tapPos)
        {
            Ray ray = cam.ScreenPointToRay(new Vector3(tapPos.x, tapPos.y, cam.transform.position.z));
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, buildLayer))
            {
                Vector3Int cellPos = grid.WorldToCell(hit.point);
                return grid.CellToWorld(cellPos) + new Vector3(0.5f, 0f, 0.5f);
            }

            return Vector3.zero;
        }
    }
}