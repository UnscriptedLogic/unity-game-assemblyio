using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildManagement
{
    public class ConveyorBelt : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float speed = 10f;

        private void FixedUpdate()
        {
            Vector3 pos = rb.position;
            rb.position += -transform.forward * speed * Time.deltaTime;
            rb.MovePosition(pos);
        }
    }
}