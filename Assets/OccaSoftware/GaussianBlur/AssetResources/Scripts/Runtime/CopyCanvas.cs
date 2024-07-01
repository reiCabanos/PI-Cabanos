using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
public class CopyCanvas : MonoBehaviour
{
    [SerializeField]
    private Canvas sourceCanvas;
    private CanvasScaler sourceCanvasScaler;
    private CanvasScaler canvasScaler;

    void OnEnable()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas.renderMode != RenderMode.ScreenSpaceCamera)
        {
            Debug.LogWarning(
                "You must use the Screen Space - Camera render mode to render blurred materials",
                this
            );
        }
        canvasScaler = GetComponent<CanvasScaler>();

        if (sourceCanvas == null)
            return;

        if (sourceCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            Debug.LogWarning(
                "Any canvas using the Screen Space - Camera render mode will be blurred. You must use the Screen Space - Overlay render mode for a canvas to render on top of the blur.",
                sourceCanvas
            );
        }

        sourceCanvasScaler = sourceCanvas.GetComponent<CanvasScaler>();
    }

    private void LateUpdate()
    {
        if (canvasScaler == null || sourceCanvasScaler == null)
            return;

        canvasScaler.uiScaleMode = sourceCanvasScaler.uiScaleMode;
        canvasScaler.referenceResolution = sourceCanvasScaler.referenceResolution;
        canvasScaler.screenMatchMode = sourceCanvasScaler.screenMatchMode;
        canvasScaler.matchWidthOrHeight = sourceCanvasScaler.matchWidthOrHeight;
        canvasScaler.referencePixelsPerUnit = sourceCanvasScaler.referencePixelsPerUnit;
    }
}
