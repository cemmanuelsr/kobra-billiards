using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    public GameObject targetBall;
    public GameObject balls;

    private SphereCollider ballCollider;
    private CapsuleCollider capsuleCollider;
    private GameObject targetHolder;
    private float signal;
    private float playForce;
    private float angle;
    private float zoom;
    private bool isGamePlaying;

    void Start() {
        capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        ballCollider = targetBall.GetComponent<SphereCollider>();

        signal = 1.0f;
        playForce = 0.0f;
        angle = 0.0f;
        zoom = 0.5f;
        isGamePlaying = false;

        Physics.IgnoreLayerCollision(6, 0, true);
    }

    void Update() {        
        if (!isGamePlaying) {
            gameObject.GetComponent<Renderer>().enabled = true;
            Vector3 ballPosition = targetBall.transform.position;

            if (Input.GetKey(KeyCode.UpArrow))
                zoom += 0.1f * Time.deltaTime;
            if (Input.GetKey(KeyCode.DownArrow))
                zoom -= 0.1f * Time.deltaTime;
            zoom = Mathf.Clamp(zoom, 0.5f, 1.5f);

            CameraFollow followScript = Camera.main.GetComponent<CameraFollow>();
            followScript.setNewOffset(new Vector3(0.0f, zoom, 0.25f));

            if (Input.GetKey(KeyCode.LeftArrow))
                angle += 0.5f * Time.deltaTime;
            if (Input.GetKey(KeyCode.RightArrow))
                angle -= 0.5f * Time.deltaTime;

            // Lock stick at target ball
            float radius = 1.15f * ballCollider.radius + playForce / 100.0f;
            Vector3 newPosition = new Vector3(radius * Mathf.Sin(angle), 0.0f, radius * Mathf.Cos(angle)) + ballPosition;
            transform.position = newPosition;

            transform.LookAt(ballPosition);
            
            if (Input.GetKey(KeyCode.Space)) {
                playForce += signal * 5.0f * Time.deltaTime;
                playForce = Mathf.Clamp(playForce, 0.0f, 15.0f);

                if (playForce >= 14.9f) {
                    signal = -1.0f;
                } else if (playForce <= 0.1f) {
                    signal = 1.0f;
                }
            }

            // Make play
            if (Input.GetKeyUp(KeyCode.Space)) {
                Rigidbody ballBody = targetBall.GetComponent<Rigidbody>();
                Vector3 forceDir = ballPosition - transform.position;
                ballBody.velocity = forceDir * playForce;
                dettachTargetBall();

                followScript.setNewTarget(GameObject.Find("Table Play Anchor"));
                followScript.setNewOffset(new Vector3(0.0f, 1.0f, 0.0f));

                isGamePlaying = true;
                playForce = 1.0f;
            }
        }
        else {
            gameObject.GetComponent<Renderer>().enabled = false;
            if (isGameStatic()) {
                attachTargetBall();
                
                CameraFollow followScript = Camera.main.GetComponent<CameraFollow>();
                followScript.setNewTarget(GameObject.Find("Stick Anchor"));
                followScript.setNewOffset(new Vector3(0.0f, zoom, 0.25f));

                isGamePlaying = false;                
            }
        }
    }

    void dettachTargetBall() {
        targetHolder = targetBall;
        targetBall = null;
    }

    void attachTargetBall() {
        targetBall = targetHolder;
        targetHolder = null;
    }

    bool isGameStatic() {
        for (int i = 0; i < balls.transform.childCount; i++) {
            Rigidbody ballBody = balls.transform.GetChild(i).GetComponent<Rigidbody>();
            if (!Utils.isCloseToZero(ballBody.velocity))
                return false;
        }
        return true;
    }
}
