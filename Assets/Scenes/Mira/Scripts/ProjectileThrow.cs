using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TrajectoryPredictor))]
public class ProjectileThrow : MonoBehaviour
{
    public TrajectoryPredictor trajectoryPredictor;

    [SerializeField]
   public  Rigidbody objectToThrow;
    public bool _sandaliaOn;
    [SerializeField, Range(0.0f, 50.0f)]
   public float force;

    [SerializeField]
    public Transform StartPosition;

    public InputAction fire;


    void OnEnable()
    {
        trajectoryPredictor = GetComponent<TrajectoryPredictor>();

        if (StartPosition == null)
            StartPosition = transform;

        fire.Enable();
        fire.performed += ThrowObject;
    }

    void Update()
    {
        Predict();
    }

    void Predict()
    {
        trajectoryPredictor.PredictTrajectory(ProjectileData());
    }

    ProjectileProperties ProjectileData()
    {
        ProjectileProperties properties = new ProjectileProperties();
        Rigidbody r = objectToThrow.GetComponent<Rigidbody>();
        

        properties.direction = StartPosition.forward;
        properties.initialPosition = StartPosition.position;
        properties.initialSpeed = force;
        properties.mass = r.mass;
        properties.drag = r.drag;

        return properties;
    }

    void ThrowObject(InputAction.CallbackContext ctx)
    {
        Rigidbody thrownObject = Instantiate(objectToThrow, StartPosition.position, Quaternion.identity);
        thrownObject.AddForce(StartPosition.forward * force, ForceMode.Impulse);

    }
}