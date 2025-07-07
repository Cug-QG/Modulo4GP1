using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class Player : Entity
{
    [Header("Player Settings")]
    [SerializeField] float speed;
    [SerializeField] float sprintSpeed;

    [SerializeField] Transform gun;

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

    protected override void Update()
    {
        base.Update();
        Rotate();
        Shoot(Input.GetMouseButton(0));
    }

    private void Shoot(bool input)
    {
        if (!input || !enableShoot || !IsAlive) return;
        enableShoot = false;
        Instantiate(projectilePrefab, firePoint.position, gun.rotation);
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
        Plane groundPlane = new(Vector3.up, Vector3.zero); // piano orizzontale

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            RotateTransform(hitPoint, transform);
            RotateTransform(hitPoint, gun);
        }
    }

    private void RotateTransform(Vector3 hitPoint, Transform toRotate)
    {
        Vector3 direction = (hitPoint - toRotate.position).normalized;
        direction.y = 0f; // mantieni solo rotazione orizzontale

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            toRotate.rotation = targetRotation;
        }
    }
}
