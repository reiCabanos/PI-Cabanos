#ifndef OS_BLUR_COMMON_INCLUDE
#define OS_BLUR_COMMON_INCLUDE


static const float TAU =     6.28318530718;
static const float SqrtTAU = 2.50662827463;


// Basic Gaussian algorithm
float Gaussian(int distance, uint radius)
{
    float sigma = float(radius) * 0.5;
    float sigma2 = sigma * sigma;
    return (1.0 / (SqrtTAU * sigma)) * exp(-(distance * distance) / (2.0 * sigma2));
}

float OptimizedGaussian(float distance, float oneOver2Sigma2)
{
    return exp(-(distance * distance * oneOver2Sigma2));
}

float GetOneOver2Sigma2(float radius)
{
    // return 1.0 / (2.0 *  0.5f * radius *  0.5f * radius);
    // simplified as:
    //return 1.0 / (0.5 * radius * radius);
    return rcp(0.5 * radius * radius);
}

#define _KernelSize 3
static const float _Kernel3[_KernelSize] = {5.0, 6.0, 5.0};
static const float _Offset3[_KernelSize] = {-1.2, 0.0, 1.2};

static const float _Kernel[3][3] = {
    {1/16, 1/8, 1/16},
    {1/8, 1/4, 1/8},
    {1/16, 1/8, 1/16}
};
SamplerState my_linear_clamp_sampler;
void Blur_float(Texture2D tex, float2 UV, float radius, float2 texCoordSize, float2 direction, out float3 color)
{
    color = float3(0,0,0);
    
    float sum = 0;

    float2 pixelOffset = texCoordSize * radius;
    
    for(int i = -1; i < 1; i++)
    {
        for(int j = -1; j < 1; j++)
        {
            float2 sampleUV = UV + (pixelOffset * float2(i, j) * 1.2);
            float4 blurData = tex.SampleLevel(my_linear_clamp_sampler, sampleUV, 0);
            float weight = _Kernel[i + 1][j + 1];
            color += blurData.rgb * weight;
            sum += weight;
        }
    }
    
    sum += 1e-7; // Prevents divison by 0.

    color /= sum;


}

#endif