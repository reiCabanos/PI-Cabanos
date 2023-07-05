using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Objeto do personagem
    public float sensitivity = 2f; // Sensibilidade do mouse
    public float smoothing = 1f; // Suavização de movimento da câmera
    public Vector3 offset = new Vector3(0f, 1.5f, -5f); // Posição offset da câmera em relação ao personagem

    private Vector2 smoothedVelocity; // Velocidade suavizada
    private Vector2 currentLookingPosition; // Posição atual de olhar

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Trava o cursor no centro da tela
    }

    void Update()
    {
        // Movimento da câmera usando o mouse
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothedVelocity.x = Mathf.Lerp(smoothedVelocity.x, mouseDelta.x, 1f / smoothing);
        smoothedVelocity.y = Mathf.Lerp(smoothedVelocity.y, mouseDelta.y, 1f / smoothing);
        currentLookingPosition += smoothedVelocity;

        // Rotação vertical da câmera
        currentLookingPosition.y = Mathf.Clamp(currentLookingPosition.y, -90f, 90f);

        // Aplica a rotação da câmera
        transform.localRotation = Quaternion.AngleAxis(-currentLookingPosition.y, Vector3.right);
        target.rotation = Quaternion.AngleAxis(currentLookingPosition.x, target.up);

        // Atualiza a posição da câmera em relação ao personagem
        transform.position = target.position + offset;
    }
}
