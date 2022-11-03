using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject holder;
    public bool toChangeRound;

    private Vector3 startPosition;
    private bool replacing;
    private Rigidbody rigidBody;

    void Start()
    {
        replacing = false;
        toChangeRound = true;
        startPosition = transform.position;
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (gameObject.CompareTag("Branca") && replacing)
        {
            Vector3 position = transform.position;
            float offset_x = position.x, offset_z = position.z;
            Mathf.Clamp(offset_x, -1.06f, 0.8f);
            Mathf.Clamp(offset_z, -0.44f, 0.44f);
            if (Input.GetKey(KeyCode.UpArrow))
                offset_z += 0.5f * Time.deltaTime;
            if (Input.GetKey(KeyCode.DownArrow))
                offset_z -= 0.5f * Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftArrow))
                offset_x -= 0.5f * Time.deltaTime;
            if (Input.GetKey(KeyCode.RightArrow))
                offset_x += 0.5f * Time.deltaTime;
                

            transform.position = new Vector3(offset_x, position.y, offset_z);

            if (Input.GetKey(KeyCode.Escape))
            {
                holder.SetActive(true);
                Physics.IgnoreLayerCollision(7, 8, false);
                replacing = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        toChangeRound = true;
        if (other.gameObject.CompareTag("Hole"))
        {
            if (gameObject.CompareTag("Branca"))
            {
                holder.SetActive(false);
                CameraFollow followScript = Camera.main.GetComponent<CameraFollow>();
                followScript.setNewTarget(GameObject.Find("Table Play Anchor"));
                followScript.setNewOffset(new Vector3(0.0f, 0.75f, 0.0f));
                transform.position = startPosition;
                rigidBody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
                Physics.IgnoreLayerCollision(7, 8, true);
                replacing = true;
                toChangeRound = true;
            }
            else if (gameObject.CompareTag("Oito"))
            {
                int playerId = holder.GetComponent<Stick>().playerId;
                int player1Points = holder.GetComponent<Stick>().player1Points;
                int player2Points = holder.GetComponent<Stick>().player2Points;
                if (playerId == 1 && player1Points == 7)
                {
                    Debug.Log("Player1 ganhou");
                }
                else if (playerId == 2 && player2Points == 7)
                {
                    Debug.Log("Player2 ganhou");
                }
                else {
                    Debug.Log("Perdeu");
                    holder.GetComponent<Stick>().changeRound();
                    holder.SetActive(false);
                    toChangeRound = true;
                }
            }
            else
            {
                int playerId = holder.GetComponent<Stick>().playerId;
                if (other.CompareTag("Lisa") && playerId == 1)
                {
                    holder.GetComponent<Stick>().increasePlayer1Points();
                    toChangeRound = false;
                }
                if (other.CompareTag("Listrada") && playerId == 2)
                {
                    holder.GetComponent<Stick>().increasePlayer2Points();
                    toChangeRound = false;
                }
                if (other.CompareTag("Listrada") && playerId == 1)
                {
                    holder.GetComponent<Stick>().increasePlayer2Points();
                    toChangeRound = true;
                }
                if (other.CompareTag("Lisa") && playerId == 2)
                {
                    holder.GetComponent<Stick>().increasePlayer1Points();
                    toChangeRound = true;
                }
                Destroy(gameObject);
            }
        }
    }
}
