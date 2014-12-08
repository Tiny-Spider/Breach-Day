using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
    public float force = 2;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            /*
            RaycastHit raycastHit;

            Debug.DrawRay(transform.position, transform.forward * 100, Color.red, 5);

            if (Physics.Raycast(transform.position, transform.forward, out raycastHit, 100)) {
                if (raycastHit.collider) {
                    if (raycastHit.transform.CompareTag(TagManager.wallPartTag))
                        raycastHit.rigidbody.isKinematic = false;
                }
            }

            if (raycastHit.rigidbody) {
                raycastHit.rigidbody.AddForceAtPosition(transform.forward * force, raycastHit.point, ForceMode.Impulse);
            }
            */

            GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
            bullet.transform.position = transform.position;

            Rigidbody rigidbody = bullet.AddComponent<Rigidbody>();
            rigidbody.velocity = -transform.forward * force;
        }

    }
}
