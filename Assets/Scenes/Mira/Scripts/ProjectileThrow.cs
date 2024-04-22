using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TrajectoryPredictor))]
public class ProjectileThrow : MonoBehaviour
{
    TrajectoryPredictor trajectoryPredictor;

    [SerializeField]
    Rigidbody objectToThrow;

    [SerializeField, Range(0.0f, 50.0f)]
    float force;

    [SerializeField]
    Transform StartPosition;

    public InputAction fire;
    [SerializeField] MoveNew _move;
    public Transform _sandalia;
    public bool _sandaliaOn;


    public void DesativarSandalia()
    {
        _sandalia.gameObject.SetActive(false);
        _sandaliaOn = true;
    }
    void MiraFalse()
    {
        _move._anim.SetBool("atirar", false);
    }
        private void Start()
    {
        _sandaliaOn = true;
    }
    public void OnEnable()
    {
        trajectoryPredictor = GetComponent<TrajectoryPredictor>();

        if (StartPosition == null)
            StartPosition = transform;

        fire.Enable();
        fire.performed += ThrowObject;

    }

    public void Fire()
    {
        if (_sandaliaOn)
        {
            _sandaliaOn = false;
            GameObject bullet = PoolingMira.SharedInstance.GetPooledObject();
            if (bullet != null)
            {
                bullet.transform.position = StartPosition.position;
                bullet.transform.localRotation = Quaternion.identity;
                bullet.SetActive(true);
                Invoke("DesativarSandalia", 5f);
                _sandalia = bullet.transform;
            }
            bullet.GetComponent<Rigidbody>().AddForce(StartPosition.forward * force, ForceMode.Impulse);
        }
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
        if (_move._mira1)
        { // Rigidbody thrownObject = Instantiate(objectToThrow, StartPosition.position, Quaternion.identity);

            if (_sandaliaOn && objectToThrow)
            {
                _move._anim.SetBool("atirar", true);
                Invoke("MiraFalse", 0.5f);
                Invoke("DesativarSandalia", 5f);
                
            }
            GameObject bullet = PoolingMira.SharedInstance.GetPooledObject();
            if (bullet != null)
            {
                bullet.transform.position = StartPosition.position;
                bullet.transform.localRotation = Quaternion.identity;
                bullet.SetActive(true);
            }
            bullet.GetComponent<Rigidbody>().AddForce(StartPosition.forward * force, ForceMode.Impulse);
        }
    }

}