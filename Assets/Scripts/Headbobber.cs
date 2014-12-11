using UnityEngine;
using System.Collections;

public class Headbobber : MonoBehaviour {
    public float bobbingSpeed   = 0.18F;
    public float bobbingAmount  = 0.2F;

    private float timer         = 0;
    private float midpoint      = 0;

    void Start() {
        midpoint = transform.localPosition.y;
    }

    void Update() {
        float waveslice = 0;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0) {
            timer = 0;
        }
        else {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;

            if (timer > Mathf.PI * 2) {
                timer = timer - (Mathf.PI * 2);
            }
        }

        Vector3 localPosition = transform.localPosition;

        if (waveslice != 0) {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

            totalAxes = Mathf.Clamp(totalAxes, 0, 1);

            translateChange = totalAxes * translateChange;
            localPosition.y = midpoint + translateChange;
        }
        else {
            localPosition.y = midpoint;
        }

        transform.localPosition = localPosition;
    }
}
