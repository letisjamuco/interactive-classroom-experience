using UnityEngine;

public class DoorAutoOpen : MonoBehaviour
{
    public Transform door;
    public float openAngle = -100f;
    public float speed = 2f;               

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = door.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        // Smooth rotation
        if (isOpen)
        {
            door.localRotation = Quaternion.Lerp(
                door.localRotation,
                openRotation,
                Time.deltaTime * speed
            );
        }
        else
        {
            door.localRotation = Quaternion.Lerp(
                door.localRotation,
                closedRotation,
                Time.deltaTime * speed
            );
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
    }

    public void CloseDoor()
    {
        isOpen = false;
    }
}
