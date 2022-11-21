using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildManagement
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Create New Building", fileName = "New Building")]
    public class BuildingSO : ScriptableObject
    {
        [SerializeField] private Sprite buildingIcon;
        [SerializeField] private GameObject prefab;
        [SerializeField] private int cost;

        [Header("UI")] 
        [SerializeField] private Color bgColor;
        [SerializeField] private Color bgPriceColor;
        [SerializeField] private Color borderColor;

        public Sprite Icon => buildingIcon;
        public GameObject Prefab => prefab;
        public int Cost => cost;

        public Color BGColor => bgColor;
        public Color BGPriceColor => bgPriceColor;
        public Color BorderColor => borderColor;
    }
}