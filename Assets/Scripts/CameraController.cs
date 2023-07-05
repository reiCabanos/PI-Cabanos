using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Objeto do personagem
    public float sensitivity = 2f; // Sensibilidade do mouse
    public float smoothing = 1f; // Suaviza��o de movimento da c�mera
    public Vector3 offset = new Vector3(0f, 1.5f, -5f); // Posi��o offset da c�mera em rela��o ao personagem

    private Vector2 smoothedVelocity; // Velocidade suavizada
    private Vector2 currentLookingPosition; // Posi��o atual de olhar

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Trava o cursor no centro da tela
    }

    void Update()
    {
        // Movimento da c�mera usando o mouse
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothedVelocity.x = Mathf.Lerp(smoothedVelocity.x, mouseDelta.x, 1f / smoothing);
        smoothedVelocity.y = Mathf.Lerp(smoothedVelocity.y, mouseDelta.y, 1f / smoothing);
        currentLookingPosition += smoothedVelocity;

        // Rota��o vertical da c�mera
        currentLookingPosition.y = Mathf.Clamp(currentLookingPosition.y, -90f, 90f);

        // Aplica a rota��o da c�mera
        transform.localRotation = Quaternion.AngleAxis(-currentLookingPosition.y, Vector3.right);
        target.rotation = Quaternion.AngleAxis(currentLookingPosition.x, target.up);

        // Atualiza a posi��o da c�mera em rela��o ao personagem
        transform.position = target.position + offset;
    }
}
