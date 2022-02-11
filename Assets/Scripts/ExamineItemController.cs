using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExamineItemController : MonoBehaviour
{
    [Header("Transforms")]
    public Transform cameraTransform;
    public Transform playerControllerTransform;
    [Space(10)]
    public string itemName;
    [Space(10)]
    [Header("Distance of examine object from camera")]
    public float distance = 0.4f;
    public float zoomDistance = 0.1f;
    [Space(10)]
    [Header("UI Text element")]
    public TMP_Text text;
    [Space(10)]
    public float rotationSpeed = 3f;
    public float zoomSpeed = 0.2f;

    private float currentDistance;
    private Vector2 zoomRange;
    private Vector3 oldPosition;
    private Quaternion oldRotation;
    private Vector3 oldLocalScale;
    private Quaternion startExamineRotation;    

    void Start()
    {
        if (!cameraTransform) Debug.LogError("Camera error!");
        if (!playerControllerTransform) Debug.LogError("Player controller error!");
        if (itemName == "") Debug.LogError("Name error!");
        if (!text) Debug.LogError("Text object error!");
    }

    void CalculateTransform()
    {
        CalculatePosition();
        transform.eulerAngles = new Vector3(cameraTransform.rotation.eulerAngles.x, playerControllerTransform.rotation.eulerAngles.y, 0f);
    }

    void CalculatePosition()
    {
        float y = playerControllerTransform.position.y - currentDistance * Mathf.Sin((cameraTransform.rotation.eulerAngles.x * Mathf.PI) / 180);
        float hypotenuse = currentDistance * Mathf.Cos((cameraTransform.rotation.eulerAngles.x * Mathf.PI) / 180);

        float x = playerControllerTransform.position.x + hypotenuse * Mathf.Sin((playerControllerTransform.rotation.eulerAngles.y * Mathf.PI) / 180);
        float z = playerControllerTransform.position.z + hypotenuse * Mathf.Cos((playerControllerTransform.rotation.eulerAngles.y * Mathf.PI) / 180);

        transform.position = new Vector3(x, y, z);
    }

    void OnMouseMove()
    {
        float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
        float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;

        Vector3 right = Vector3.Cross(cameraTransform.transform.up, transform.position - cameraTransform.transform.position);

        Quaternion calculatedRotation = Quaternion.AngleAxis(rotationY, right) * transform.rotation;
        if (Vector3.Angle(startExamineRotation * Vector3.up, calculatedRotation * Vector3.up) <= 90f)
        {
            transform.rotation = calculatedRotation;
        }
        transform.RotateAround(transform.position, transform.up, -rotationX);
    }

    void OnEnable()
    {
        zoomRange = new Vector2(distance - zoomDistance, distance);
        currentDistance = distance;

        oldPosition = transform.position;
        oldRotation = transform.rotation;
        oldLocalScale = transform.localScale;

        text.text = itemName;
        CalculateTransform();

        startExamineRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            OnMouseMove();
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float calculatedDistance = currentDistance - zoomSpeed * Input.GetAxis("Mouse ScrollWheel");
            if (calculatedDistance >= zoomRange.x && calculatedDistance <= zoomRange.y)
            {
                currentDistance = calculatedDistance;
                CalculatePosition();
            }
        }
    }
    void OnDisable()
    {
        transform.position = oldPosition;
        transform.rotation = oldRotation;
        transform.localScale = oldLocalScale;
    }
}
