using UnityEngine;

public class Orbit_Camera : MonoBehaviour
{
    public Transform target;          //The object the camera orbits around
    public float distance = 5f;       //Initial distance from the target
    public float orbitSpeed = 100f;   //Speed of orbit rotation
    public float scrollSpeed = 2f;    //Speed of zooming in and out
    public float minDistance = 2f;    //Minimum distance to the target
    public float maxDistance = 15f;   //Maximum distance to the target
    public float zoomSpeed = 5f;      //Speed of smooth zooming
    public LayerMask collisionLayers; //Layers to consider as obstacles

    private float currentX = 0f;      //Horizontal rotation
    private float currentY = 0f;      //Vertical rotation
    private float yMinLimit = -20f;   //Minimum vertical angle
    private float yMaxLimit = 80f;    //Maximum vertical angle
    private float desiredDistance;    //Desired distance based on user input

    void Start()
    {
        desiredDistance = distance; 
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        //Rotate camera based on mouse movement
        if /*(Input.GetKey(KeyCode.Mouse2) ||*/ (Input.GetMouseButton(1)) /*Right mouse button*//*Middle mouse*/
        {
            currentX += Input.GetAxis("Mouse X") * orbitSpeed * Time.deltaTime;
            currentY -= Input.GetAxis("Mouse Y") * orbitSpeed * Time.deltaTime;

            //Clamp the vertical rotation
            currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);
        }

        //Zoom in and out using the mouse scroll wheel
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);

        //Calculate the desired camera position
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = target.position - (rotation * Vector3.forward * desiredDistance);

        //Check for obstacles between the target and the desired camera position
        RaycastHit hit;
        if (Physics.Raycast(target.position, desiredPosition - target.position, out hit, desiredDistance, collisionLayers))
        {
            //If there's an obstacle, set the distance to the hit point minus a small offset
            distance = Mathf.Lerp(distance, hit.distance - 0.1f, Time.deltaTime * zoomSpeed);
        }
        else
        {
            //No obstacles, smoothly return to the desired distance
            distance = Mathf.Lerp(distance, desiredDistance, Time.deltaTime * zoomSpeed);
        }

        //Apply the calculated position and rotation
        Vector3 finalPosition = target.position - (rotation * Vector3.forward * distance);
        transform.position = finalPosition;
        transform.LookAt(target);
    }
}
