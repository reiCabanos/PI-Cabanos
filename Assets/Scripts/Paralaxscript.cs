using UnityEngine;

public class Paralaxscript : MonoBehaviour
{
    private float parallaxSpeed;
    private Transform cameraTransform;
    private float starpositionX;
    private float spriteSizeX;


    private void Start()
    {
        cameraTransform = Camera.main.transform;
        starpositionX = transform.position.x;
        spriteSizeX = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    private void Update()
    {
        float relativeDist = cameraTransform.position.x * parallaxSpeed;
        transform.position = new Vector3 (starpositionX + relativeDist, transform.position.y , transform.position.z);

        // loop parralx effet 
        float relativeCamaeraDist = cameraTransform.position.x * (1 - parallaxSpeed);
        if (relativeCamaeraDist > starpositionX)
        {
            starpositionX += relativeDist;
        }
        else if (relativeCamaeraDist < starpositionX - spriteSizeX)
        {
            starpositionX -= spriteSizeX;
        }
    }
      



}
