using UnityEngine;
using UnityEngine.UI;

namespace OccaSoftware.GaussianBlur.Runtime
{
    [ExecuteAlways]
    [RequireComponent(typeof(Image))]
    public class CopyImage : MonoBehaviour
    {
        [SerializeField]
        private Image target;
        private Image self;

        public void SetImageTarget(Image target)
        {
            this.target = target;
        }

        private void OnEnable()
        {
            self = GetComponent<Image>();
        }

        private void LateUpdate()
        {
            if (target == null)
                return;

            self.sprite = target.sprite;
            self.raycastTarget = false;
            self.raycastPadding = target.raycastPadding;
        }
    }
}
