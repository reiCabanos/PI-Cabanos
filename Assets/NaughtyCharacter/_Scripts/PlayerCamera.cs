using UnityEngine;

// Importa a engine Unity
using UnityEngine;

// Define um namespace para organizar o código
namespace NaughtyCharacter
{
    // Define uma classe pública que herda de MonoBehaviour
    public class PlayerCamera : MonoBehaviour
    {
        // Define uma variável pública com um valor padrão de 0.0f
        [Tooltip("Velocidade de rotação da câmera em torno do pivô. Valores <= 0 são interpretados como rotação instantânea")]
        public float RotationSpeed = 0.0f;

        // Define outra variável pública com um valor padrão de 0.0f
        public float PositionSmoothDamp = 0.0f;

        // Define uma variável pública do tipo Transform
        public Transform Rig; // The root transform of the camera rig

        // Define outra variável pública do tipo Transform
        public Transform Pivot; // The point at which the camera pivots around

        // Define uma variável pública do tipo Camera
        public Camera Camera;

        // Define uma variável privada do tipo Vector3
        private Vector3 _cameraVelocity;

        // Define um método público que recebe um Vector3 como parâmetro
        public void SetPosition(Vector3 position)
        {
            // Atualiza a posição do Rig usando Vector3.SmoothDamp para uma transição gradual
            Rig.position = Vector3.SmoothDamp(Rig.position, position, ref _cameraVelocity, PositionSmoothDamp);
        }

        // Define um método público que recebe um Vector2 como parâmetro
        public void SetControlRotation(Vector2 controlRotation)
        {
            // Calcula as rotações alvo para o Rig e o pivô
            Quaternion rigTargetLocalRotation = Quaternion.Euler(0.0f, controlRotation.y, 0.0f); // Controla a rotação Yaw (horizontal)
            Quaternion pivotTargetLocalRotation = Quaternion.Euler(controlRotation.x, 0.0f, 0.0f); // Controla a rotação Pitch (vertical)

            // Aplica rotações suavemente se RotationSpeed for positivo, caso contrário instantaneamente
            if (RotationSpeed > 0.0f)
            {
                // Usa Quaternion.Slerp para interpolação suave entre as rotações atual e alvo
                Rig.localRotation = Quaternion.Slerp(Rig.localRotation, rigTargetLocalRotation, RotationSpeed * Time.deltaTime);
                Pivot.localRotation = Quaternion.Slerp(Pivot.localRotation, pivotTargetLocalRotation, RotationSpeed * Time.deltaTime);
            }
            else
            {
                // Aplica rotações diretamente
                Rig.localRotation = rigTargetLocalRotation;
                Pivot.localRotation = pivotTargetLocalRotation;
            }
        }
    }
}
