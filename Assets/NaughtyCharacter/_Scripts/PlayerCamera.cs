using UnityEngine;

// Importa a engine Unity
using UnityEngine;

// Define um namespace para organizar o c�digo
namespace NaughtyCharacter
{
    // Define uma classe p�blica que herda de MonoBehaviour
    public class PlayerCamera : MonoBehaviour
    {
        // Define uma vari�vel p�blica com um valor padr�o de 0.0f
        [Tooltip("Velocidade de rota��o da c�mera em torno do piv�. Valores <= 0 s�o interpretados como rota��o instant�nea")]
        public float RotationSpeed = 0.0f;

        // Define outra vari�vel p�blica com um valor padr�o de 0.0f
        public float PositionSmoothDamp = 0.0f;

        // Define uma vari�vel p�blica do tipo Transform
        public Transform Rig; // The root transform of the camera rig

        // Define outra vari�vel p�blica do tipo Transform
        public Transform Pivot; // The point at which the camera pivots around

        // Define uma vari�vel p�blica do tipo Camera
        public Camera Camera;

        // Define uma vari�vel privada do tipo Vector3
        private Vector3 _cameraVelocity;

        // Define um m�todo p�blico que recebe um Vector3 como par�metro
        public void SetPosition(Vector3 position)
        {
            // Atualiza a posi��o do Rig usando Vector3.SmoothDamp para uma transi��o gradual
            Rig.position = Vector3.SmoothDamp(Rig.position, position, ref _cameraVelocity, PositionSmoothDamp);
        }

        // Define um m�todo p�blico que recebe um Vector2 como par�metro
        public void SetControlRotation(Vector2 controlRotation)
        {
            // Calcula as rota��es alvo para o Rig e o piv�
            Quaternion rigTargetLocalRotation = Quaternion.Euler(0.0f, controlRotation.y, 0.0f); // Controla a rota��o Yaw (horizontal)
            Quaternion pivotTargetLocalRotation = Quaternion.Euler(controlRotation.x, 0.0f, 0.0f); // Controla a rota��o Pitch (vertical)

            // Aplica rota��es suavemente se RotationSpeed for positivo, caso contr�rio instantaneamente
            if (RotationSpeed > 0.0f)
            {
                // Usa Quaternion.Slerp para interpola��o suave entre as rota��es atual e alvo
                Rig.localRotation = Quaternion.Slerp(Rig.localRotation, rigTargetLocalRotation, RotationSpeed * Time.deltaTime);
                Pivot.localRotation = Quaternion.Slerp(Pivot.localRotation, pivotTargetLocalRotation, RotationSpeed * Time.deltaTime);
            }
            else
            {
                // Aplica rota��es diretamente
                Rig.localRotation = rigTargetLocalRotation;
                Pivot.localRotation = pivotTargetLocalRotation;
            }
        }
    }
}
