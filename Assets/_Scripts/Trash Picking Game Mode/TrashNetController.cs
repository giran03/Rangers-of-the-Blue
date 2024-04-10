using UnityEngine;
using UnityEngine.InputSystem.Android;
using UnityEngine.XR.ARFoundation;

public class TrashNetController : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    FixedJoystick fixedJoystick;
    Rigidbody rb;
    Quaternion lookRotation;

    private void OnEnable()
    {
        fixedJoystick = FindObjectOfType<FixedJoystick>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float xVal = fixedJoystick.Horizontal;
        float yVal = fixedJoystick.Vertical;

        Vector3 movement = new(xVal, 0, yVal);
        rb.velocity = movement * moveSpeed;

        lookRotation = Quaternion.LookRotation(movement, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(lookRotation, transform.rotation, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trash"))
        {
            GameObject trash = other.gameObject;
            Destroy(trash);
        }
    }
}
