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
        [SerializeField] private float cost;

        public Sprite Icon => buildingIcon;
        public GameObject Prefab => prefab;
        public float Cost => cost;
    }
}