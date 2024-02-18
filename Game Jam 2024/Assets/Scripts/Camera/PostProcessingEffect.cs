using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingEffect : MonoBehaviour
{

    [SerializeField] GameObject noPostCam;
    private float vignetteNormalVol = 0.2f;


    public static PostProcessingEffect instance { get; private set; }

    public float intensity = 0;
    Volume volume;

    Bloom bloom;
    Vignette vignette;
    ColorAdjustments colorAdjustments;
    DepthOfField depthOfField;
    ChromaticAberration chromatic;

    private void Awake()
    {

        if (instance != null && instance != this)
        {
            {
                Destroy(this);
            }
        }

        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        noPostCam.SetActive(false);

        volume = GetComponent<Volume>();



        //Check Post processing
        volume.profile.TryGet<Vignette>(out vignette);

        if (!vignette)
        {
            return;
        }

        else
        {
            vignette.intensity.Override(vignetteNormalVol);
            vignette.active = true;

        }


        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);

        if (!colorAdjustments)
        {
            return;
        }

        else
        {
            colorAdjustments.active = false;

        }

        volume.profile.TryGet<Bloom>(out bloom);

        if (!bloom)
        {
            return;
        }

        else
        {
            bloom.active = true;

        }


        volume.profile.TryGet<DepthOfField>(out depthOfField);

        if (!depthOfField)
        {
            return;
        }

        else
        {
           
            depthOfField.active = false;

        }


        volume.profile.TryGet<ChromaticAberration>(out chromatic);

        if (!chromatic)
        {
            return;
        }

        else
        {

            chromatic.active = false;

        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator HurtEffect()
    {
        intensity = 0.6f;
        // vignette.enabled.Override(true);
        vignette.active = true;
        vignette.intensity.Override(0.6f);
        yield return new WaitForSeconds(0.4f);

        while (intensity > 0)
        {
            intensity -= 0.01f;
            if (intensity < 0) intensity = 0;
            vignette.intensity.Override(intensity);
            yield return new WaitForSeconds(0.1f);

        }


        vignette.active = false;
        yield break;
    }


    private IEnumerator GetBoosted()
    {
        chromatic.active = true;
        intensity = 0.1f;
        // vignette.enabled.Override(true);
        chromatic.intensity.Override(0.1f);

        while (intensity < 1f)
        {

            intensity += 0.1f;
            if (intensity >= 1f) intensity = 1f;
            chromatic.intensity.Override(intensity);
            yield return new WaitForSeconds(0.2f);

        }


        //  vignette.active = false;
        yield break;
    }


    private IEnumerator GetNorm()
    {
        intensity = 1f;
        // vignette.enabled.Override(true);
        chromatic.intensity.Override(1f);

        while (intensity > 0f)
        {

            intensity -= 0.1f;
            if (intensity <=  0f) intensity = 0f;
            chromatic.intensity.Override(intensity);
            yield return new WaitForSeconds(0.1f);

        }
        chromatic.active = false;


        //  vignette.active = false;
        yield break;
    }


    public IEnumerator Boosting()
    {
        StartCoroutine(GetBoosted());
        yield return new WaitForSeconds(11f);
        StartCoroutine(GetNorm());
    }

    private IEnumerator EyesFocus()
    {
        intensity = 0.2f;
        // vignette.enabled.Override(true);
        vignette.intensity.Override(0.2f);

        while (intensity < 0.7f)
        {
            //Debug.Log("Bug here");
            intensity += 0.2f;
            if (intensity > 0.7f) intensity = 0.7f;
            vignette.intensity.Override(intensity);
            yield return new WaitForSeconds(0.1f);

        }


      //  vignette.active = false;
        yield break;
    }

    private IEnumerator EyesUnFocus()
    {
        intensity = 0.7f;
        // vignette.enabled.Override(true);
        vignette.intensity.Override(0.7f);

        while (intensity > 0.2f)
        {
            
            intensity -= 0.2f;
            if (intensity < 0.2f) intensity = 0.2f;
            vignette.intensity.Override(intensity);
            yield return new WaitForSeconds(0.1f);

        }


        //  vignette.active = false;
        yield break;
    }

    public void GetFocus()
    {
        StopCoroutine(EyesUnFocus());
        if (!noPostCam.activeSelf)
        {
            noPostCam.SetActive(true);
        }

        depthOfField.active = true;
        StartCoroutine(EyesFocus());

        //intensity = 0.6f;
        //// vignette.enabled.Override(true);
        //vignette.active = true;
        //vignette.intensity.Override(0.6f);
        //yield return new WaitForSeconds(0.4f);

        //while (intensity > 0)
        //{
        //    intensity -= 0.01f;
        //    if (intensity < 0) intensity = 0;
        //    vignette.intensity.Override(intensity);
        //    yield return new WaitForSeconds(0.1f);

        //}



        // yield break;
    }


    public void GetUnfocus()
    {
        StopCoroutine(EyesFocus());
        if (noPostCam.activeSelf)
        {
            noPostCam.SetActive(false);
        }
        
        depthOfField.active = false;

        StartCoroutine(EyesUnFocus());

    }



}
