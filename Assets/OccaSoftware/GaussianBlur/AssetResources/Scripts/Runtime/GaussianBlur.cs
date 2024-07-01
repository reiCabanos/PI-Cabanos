using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

namespace OccaSoftware.GaussianBlur.Runtime
{
    [
        Serializable,
        VolumeComponentMenuForRenderPipeline(
            "Post-processing/Gaussian Blur",
            typeof(UniversalRenderPipeline)
        )
    ]
    public sealed class GaussianBlur : VolumeComponent, IPostProcessComponent
    {
        public BoolParameter enabled = new BoolParameter(false);

        [Tooltip(
            "Set the blur radius. Combined additively with the blur radius set on individual materials in the scene or the UI."
        )]
        public MinIntParameter radius = new MinIntParameter(15, 0);

        public Texture2DParameter texture = new Texture2DParameter(null);

        public BoolParameter ignoreLessBlurredPixels = new BoolParameter(false);

        public bool IsActive()
        {
            return enabled.GetValue<bool>();
        }

        public bool IsTileCompatible() => false;
    }
}
