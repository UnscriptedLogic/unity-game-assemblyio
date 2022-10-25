using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LevelManagement
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Construct Mode")]
        [SerializeField] private Button constructMode;
        [SerializeField] private Button bluePrintsMode;
        [SerializeField] private Button settingsMode;

        private void Start()
        {
            constructMode.onClick.AddListener(OnConstructMode);
            bluePrintsMode.onClick.AddListener(OnBlueprintMode);
            settingsMode.onClick.AddListener(OnSettingsMode);
        }

        private void OnConstructMode()
        {
            UINavigator.PopAndPush("ConstructPersistent");
            UINavigator.Push("Construct");
        }

        private void OnBlueprintMode()
        {

        }

        private void OnSettingsMode()
        {
            
        }
    }
}