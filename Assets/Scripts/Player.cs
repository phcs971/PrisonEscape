using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody rb;

    [Header("Player Step:")]
    public GameObject stepRayLower;
    public GameObject stepRayUpper;
    public float stepSmooth = 0.1f;

    [Header("Movement Control:")]
    public float sensitivity = 10;
    public float speed = 20;

    private GameManager manager;


    void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();

        manager = GameObject.FindObjectOfType<GameManager>();
    }

    void Update()
    {
        float rotateHorizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        float fixedSpeed = speed * Time.deltaTime;

        float angle = mainCamera.transform.eulerAngles.y * Mathf.PI / 180;
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        Vector3 forward = (vertical * cos - horizontal * sin) * fixedSpeed * Vector3.forward;
        Vector3 right = (horizontal * cos + vertical * sin) * fixedSpeed * Vector3.right;


        mainCamera.transform.RotateAround(transform.position, Vector3.up, rotateHorizontal * sensitivity);
        rb.MovePosition(transform.position + forward + right);

        StepClimb();
    }

    void StepClimb()
    {
        Transform lowerTransform = stepRayLower.transform;
        if (Physics.Raycast(lowerTransform.position, lowerTransform.TransformDirection(Vector3.forward), out RaycastHit _, 0.1f))
        {
            Transform upperTransform = stepRayUpper.transform;
            if (!Physics.Raycast(upperTransform.position, upperTransform.TransformDirection(Vector3.forward), out RaycastHit _, 0.2f))
            {
                rb.position -= new Vector3(0, -stepSmooth, 0);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponentInParent<Cell>())
        {
            manager.DidEnterCell(other.GetComponentInParent<Cell>());
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.GetComponentInParent<Cell>())
        {
            manager.DidExitCell(other.GetComponentInParent<Cell>());
        }
    }
}
