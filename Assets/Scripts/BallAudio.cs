using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAudio : MonoBehaviour
{
    void OnCollisionEnter(Collision collision) {
        gameObject.GetComponent<AudioSource>().volume = Mathf.Clamp(collision.relativeVelocity.magnitude, 0.01f, 0.25f);
        gameObject.GetComponent<AudioSource>().Play();
    }
}
