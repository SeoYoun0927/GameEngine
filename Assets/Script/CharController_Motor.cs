using UnityEngine;

public class CharController_Motor : MonoBehaviour
{
    public float speed = 10.0f;
    public float sensitivity = 60.0f;
    public float jumpForce = 5.0f;
    public GameObject cam;

    private CharacterController character;
    private float moveFB, moveLR;
    private float rotX, rotY;
    private float gravity = -16.0f;
    private bool isGrounded = true;
    private float verticalVelocity = 0.0f;

    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    void Update()
    {
        moveFB = Input.GetAxis("Horizontal") * speed;
        moveLR = Input.GetAxis("Vertical") * speed;

        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY = Input.GetAxis("Mouse Y") * sensitivity;

        Vector3 movement = new Vector3(moveFB, verticalVelocity, moveLR);

        CameraRotation(cam, rotX, rotY);

        movement = transform.rotation * movement;

        // 스페이스바를 누르고 있는 동안 점프 유지
        if (character.isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                isGrounded = false;
            }
        }
        else
        {
            // 중력 적용
            verticalVelocity += gravity * Time.deltaTime;
        }

        // 캐릭터 이동
        character.Move(movement * Time.deltaTime);
    }

    void CameraRotation(GameObject cam, float rotX, float rotY)
    {
        transform.Rotate(0, rotX * Time.deltaTime, 0);
        cam.transform.Rotate(-rotY * Time.deltaTime, 0, 0);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y > 0.7f)
        {
            isGrounded = true;
            verticalVelocity = 0.0f;
        }
    }
}
