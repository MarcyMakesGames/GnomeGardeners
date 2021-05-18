using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public class Wind : MonoBehaviour
    {
        public Vector3 despawnLocation;
        public float moveSpeed;

        private void Start()
        {
            RotateTowardDespawn();
        }

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

        private void RotateTowardDespawn()
        {
            var despawnVector = despawnLocation - transform.position;

            if (despawnVector.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                return;
            }
            if (despawnVector.x < 0)
                return;
            if (despawnVector.y > 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                return;
            }
            if (despawnVector.y < 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                return;
            }
        }
    }
}
