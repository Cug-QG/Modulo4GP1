using UnityEngine;

public class Player : Entity
{
    [Header("Player Settings")]
    [SerializeField] float speed;
    [SerializeField] float sprintSpeed;

    Rigidbody rb;
    Camera mainCamera;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void Update()
    {
        Rotate();
    }

    private void Move(float inputX, float inputZ)
    {
        Vector3 inputDirection = new Vector3(inputX, 0, inputZ).normalized;

        float actualSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;
        
        Vector3 moveDirection = inputDirection * actualSpeed;
        
        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
    }

    private void Rotate()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // piano orizzontale

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 direction = hitPoint - transform.position;
            direction.y = 0f; // mantieni solo rotazione orizzontale

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = targetRotation;
            }
        }
    }
}
