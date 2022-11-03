using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stick : MonoBehaviour
{
    public GameObject targetBall;
    public GameObject balls;
    public int playerId;
    public TextMeshProUGUI playerText;
    public int player1Points;
    public int player2Points;

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

        playerId = 1;
        setPlayerText();
        signal = 1.0f;
        playForce = 0.0f;
        angle = 0.0f;
        zoom = 0.5f;
        isGamePlaying = false;
        player1Points = 0;
        player2Points = 0;

        Physics.IgnoreLayerCollision(6, 7, true);
        Physics.IgnoreLayerCollision(6, 8, true);
    }

    void Update() {
        if (!isGamePlaying) {
            Vector3 ballPosition = targetBall.transform.position;
            CameraFollow followScript = Camera.main.GetComponent<CameraFollow>();

            // Player's round
            if (playerId == 1) {
                executeAction(ballPosition, followScript);
            }
            // AI's round
            else {
                executeAction(ballPosition, followScript);
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

                if (hasToChangeRound()) {
                    changeRound();
                }
            }
        }
    }

    void executeAction(Vector3 ballPosition, CameraFollow followScript) {
        gameObject.GetComponent<Renderer>().enabled = true;

        if (Input.GetKey(KeyCode.UpArrow))
            zoom += 0.1f * Time.deltaTime;
        if (Input.GetKey(KeyCode.DownArrow))
            zoom -= 0.1f * Time.deltaTime;
        zoom = Mathf.Clamp(zoom, 0.5f, 1.5f);

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

        if (Input.GetKey(KeyCode.Space))
        {
            playForce += signal * 5.0f * Time.deltaTime;
            playForce = Mathf.Clamp(playForce, 0.0f, 15.0f);

            if (playForce >= 14.9f)
            {
                signal = -1.0f;
            }
            else if (playForce <= 0.1f)
            {
                signal = 1.0f;
            }
        }

        // Make play
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Rigidbody ballBody = targetBall.GetComponent<Rigidbody>();
            Vector3 forceDir = ballPosition - transform.position;
            ballBody.velocity = forceDir * playForce;
            dettachTargetBall();

            followScript.setNewTarget(GameObject.Find("Table Play Anchor"));
            followScript.setNewOffset(new Vector3(0.0f, 1.0f, 0.0f));

            isGamePlaying = true;
            playForce = 0.0f;
        }
    }

    void dettachTargetBall() {
        targetHolder = targetBall;
        targetBall = null;
    }

    public void changeRound() {
        if (playerId == 1)
            playerId = 2;
        else
            playerId = 1;

        setPlayerText();
    }

    void setPlayerText() {
        if (playerId == 1)
            playerText.text = "Player " + playerId.ToString() + ": " + player1Points.ToString() + " points";
        else
            playerText.text = "Player " + playerId.ToString() + ": " + player2Points.ToString() + " points";
    }

    public void increasePlayer1Points() {
        player1Points++;
    }

    public void increasePlayer2Points() {
        player2Points++;
    }

    void attachTargetBall() {
        targetBall = targetHolder;
        targetHolder = null;
    }

    bool isGameStatic() {
        for (int i = 0; i < balls.transform.childCount; i++) {
            Rigidbody ballBody = balls.transform.GetChild(i).GetComponent<Rigidbody>();
            if (ballBody.velocity.magnitude > 0.01f)
                return false;
        }
        return true;
    }

    bool hasToChangeRound() {
        for (int i = 0; i < balls.transform.childCount; i++) {
            bool hasToChangeBall = balls.transform.GetChild(i).GetComponent<Ball>().toChangeRound;
            if (hasToChangeBall)
                return true;
        }
        return false;
    }
}
