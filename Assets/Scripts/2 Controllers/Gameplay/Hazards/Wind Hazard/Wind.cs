using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Wind : MonoBehaviour
    {
        public Vector3 despawnLocation;
        public float moveSpeed;

        private void Update()
        {
            MoveToDespawn();
        }

        private void MoveToDespawn()
        {
            transform.position = Vector3.MoveTowards(transform.position, despawnLocation, moveSpeed * Time.deltaTime);

            if (transform.position == despawnLocation)
                Destroy(gameObject);
        }
    }
}
