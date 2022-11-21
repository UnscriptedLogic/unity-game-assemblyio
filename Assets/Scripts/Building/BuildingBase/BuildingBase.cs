using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildManagement.Buildings
{
    public class BuildingBase : MonoBehaviour
    {
        private BuildingSO buildingSO;

        public BuildingSO BuildingDetails => buildingSO;
    }
}