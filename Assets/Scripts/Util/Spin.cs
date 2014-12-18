using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {
    public Vector3 rotationsPerSecond = new Vector3(0f, 0.1f, 0f);

    Rigidbody mRb;
    Transform mTrans;
	Quaternion startRotation;

    void Start() {
		startRotation = transform.rotation;

        mTrans = transform;
        mRb = rigidbody;
    }

    void Update() {
        if (mRb == null) {
            ApplyDelta(Time.deltaTime);
        }
    }

    void FixedUpdate() {
        if (mRb != null) {
            ApplyDelta(Time.deltaTime);
        }
    }

	public void Reset() {
		transform.rotation = startRotation;
	}

    public void ApplyDelta(float delta) {
        delta *= Mathf.Rad2Deg * Mathf.PI * 2f;
        Quaternion offset = Quaternion.Euler(rotationsPerSecond * delta);

        if (mRb == null) {
            mTrans.rotation = mTrans.rotation * offset;
        }
        else {
            mRb.MoveRotation(mRb.rotation * offset);
        }
    }
}
