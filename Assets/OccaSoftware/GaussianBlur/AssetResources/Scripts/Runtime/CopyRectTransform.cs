using UnityEngine;

namespace OccaSoftware.GaussianBlur.Runtime
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class CopyRectTransform : MonoBehaviour
    {
        [SerializeField]
        private RectTransform target;
        private RectTransform self;

        public void SetRectTransformTarget(RectTransform target)
        {
            this.target = target;
        }

        private void OnEnable()
        {
            self = GetComponent<RectTransform>();
        }

        private void LateUpdate()
        {
            if (target == null)
                return;

            self.localScale = target.localScale;
            self.anchoredPosition3D = target.anchoredPosition3D;
            self.anchorMin = target.anchorMin;
            self.anchorMax = target.anchorMax;
            self.anchoredPosition = target.anchoredPosition;
            self.sizeDelta = target.sizeDelta;
            self.pivot = target.pivot;
        }
    }
}
