using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildManagement
{
    public class MaterialDispenser : MonoBehaviour
    {
        [SerializeField] protected float dispenseInterval;
        [SerializeField] protected GameObject dispenseObject;
        [SerializeField] protected Transform[] dispenseLocations;

        protected float _interval;

        protected virtual void Start()
        {
            //Short delay before spawning
            _interval = 1f;
        }

        protected virtual void Update()
        {
            if (_interval <= 0f)
            {
                DispenseMaterial(dispenseLocations[0].position);
                _interval = dispenseInterval;
            }

            _interval -= Time.deltaTime;
        }

        protected virtual GameObject CreateMaterial()
        {
            return dispenseObject;
        }

        protected virtual void DispenseMaterial(Vector3 dispenseLocation)
        {
            Instantiate(CreateMaterial(), dispenseLocation, transform.rotation);
        }
    }
}