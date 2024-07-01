::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
 This asset was shared by https://unityassetcollection.com

 Contact us:
 - Email: unityassetcollection@gmail.com
 - Telegram: @assetcollection or https://t.me/assetcollection
								
 If you find this package helpful and want to support us. 	
 Please go to https://tinyurl.com/d0nat10n			
 We really appreciate your help.				
 Thank you.	
::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::





README
================================

Contents
--------------------------------
About
Installation Instructions
Usage Instructions
Requirements
Support


About
--------------------------------
Blurring is a useful technique in design that can give the impression of movement or depth in a scene. 
Layer blur can be used for a variety of purposes, including masking information, softening or shifting focus from the background, generating abstract backgrounds, and replicating photographic effects such as camera depth or bokeh. 
By default, Gaussian Blur will apply a full-screen layer blur to the camera view.


LEARN MORE ABOUT OCCASOFTWARE
Gaussian Blur is provided by OccaSoftware.
Our products are designed to last and built with integrity.
Our brand is built on our relationship with our customers.

* Click here to find other great assets: https://www.occasoftware.com/



Installation Instructions
--------------------------------
1. Import the asset into your project.


Usage Instructions
--------------------------------
1. Add the Gaussian Blur render feature to Universal Renderer Data asset.
2. Add the Gaussian Blur override to a volume in your scene. Enable it. Adjust the Blur Radius (px).
!!! -> The Gaussian Blur override MUST be enabled and active for ANY blur, including mask-based blurs.


Read our FAQs for additional how-to's with screenshots: https://www.occasoftware.com/assets/gaussian-blur

You can also assign the Gaussian Blur effect to Local Box and Sphere Volumes in the same way you would with any other built-in URP post-processing effect.

You can reference the URP documentation on configuring post-processing for additional help:
https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@12.1/manual/integration-with-post-processing.html


FAQs
--------------------------------
Can I blur the entire screen?
-> Yes. To blur the entire screen:
1. Create a new Global Volume component.
2. Add the Guassian Blur override.
3. Enable it.
4. Set the radius to some non-zero value.
Note: Full screen blur is combined additively with mask-based blurs.


Can I blur text and UI elements?
-> Yes. To blur text:
1. Create a Screen Space - Camera canvas. 
2. Create a new Raw Image object in this canvas.
3. Apply a Blur Mask material to this object.
4. Create and add text and images to the same canvas.



Can I have unblurred text and UI elements?
-> Yes. To prevent text from being blurred, use a Screen Space - Overlay canvas.
1. Create a Screen Space - Camera canvas. 
2. Create a new Raw Image object in this canvas.
3. Apply a Blur Mask material to this object.
4. Create a Screen Space - Overlay canvas.
5. Create and add text and images to the Screen Space Overlay canvas.




Troubleshooting
--------------------------------
Ensure that you have added the Gaussian Blur render feature to your Universal Renderer Data.
Ensure that you have added the Gaussian Blur override to a global or local volume.
Ensure that the Gaussian Blur override is enabled.


Compatibility
--------------------------------
This asset is compatible with any platform with Compute Shader support.
This asset conducts straight Gaussian Blur blending without any up- or down-scaling. 
This gives you the finest control over progressive blur levels, blur pixel radius, and more.
However, this also requires more GPU resources when computing the blur.
Try to avoid very high blur radius values to limit the performance impact.


Support
--------------------------------
If you're not happy, we're not happy.
Please contact us at hello@occasoftware.com or join our Discord at https://www.occasoftware.com/discord for any support.

