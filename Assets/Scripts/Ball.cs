using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject holder;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool replacing;

    void Start()
    {
        replacing = false;
        startPosition = transform.position;
        startRotation = transform.rotation;
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
                offset_x += 0.5f * Time.deltaTime;
            if (Input.GetKey(KeyCode.DownArrow))
                offset_x -= 0.5f * Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftArrow))
                offset_z += 0.3f * Time.deltaTime;
            if (Input.GetKey(KeyCode.RightArrow))
                offset_z -= 0.3f * Time.deltaTime;

            transform.position = new Vector3(offset_x, position.y, offset_z);

            if (Input.GetKey(KeyCode.Space))
            {
                holder.SetActive(true);
                Physics.IgnoreLayerCollision(7, 0, false);
                replacing = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hole"))
        {
            if (gameObject.CompareTag("Branca"))
            {
                holder.SetActive(false);
                CameraFollow followScript = Camera.main.GetComponent<CameraFollow>();
                followScript.setNewTarget(gameObject);
                followScript.setNewOffset(new Vector3(1.0f, 0.0f, 1.0f));
                transform.position = startPosition;
                transform.rotation = startRotation;
                Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
                rigidBody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
                Physics.IgnoreLayerCollision(7, 0, true);
                replacing = true;
            }
            else if (gameObject.CompareTag("Oito"))
            {
                Debug.Log("Perdeu");
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
