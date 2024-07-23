using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyePatch : MonoBehaviour
{

    public Camera leftEye;
    public Camera rightEye;

    public MeshRenderer eyePatch;

    public LayerMask eyePatchVisibility;
    public LayerMask normalVisibility;

    public float fadeSpeed = 2f;

    private bool transitioning = false;

    public enum Eye{
        Neither = 0,
        LeftEye = 1,
        RightEye = 2,
        Both = 3
    }

    #if UNITY_EDITOR
    void Update(){
        if(Input.GetKeyDown(KeyCode.L)){
            SetEyePatch(Eye.LeftEye);
        }else if(Input.GetKeyDown(KeyCode.R)){
            SetEyePatch(Eye.RightEye);
        }else if(Input.GetKeyDown(KeyCode.B)){
            SetEyePatch(Eye.Both);
        }else if(Input.GetKeyDown(KeyCode.N)){
            SetEyePatch(Eye.Neither);
        }
    }
    #endif

    public void SetEyePatch(Eye switchToEye){
        if(transitioning){
            StopCoroutine("Transition");
        }

        StartCoroutine(Transition(switchToEye));
    }

    private IEnumerator Transition(Eye switchToEye){
        transitioning = true;

        Color currentColour = eyePatch.material.color;

        while(currentColour.a > 0){
            yield return new WaitForFixedUpdate();
            
            currentColour.a = Mathf.Clamp01(currentColour.a - (fadeSpeed * Time.fixedDeltaTime));
            eyePatch.material.color = currentColour;
        }

        leftEye.cullingMask = (switchToEye == Eye.LeftEye || switchToEye == Eye.Neither) && switchToEye != Eye.Both ? normalVisibility : eyePatchVisibility;
        rightEye.cullingMask = (switchToEye == Eye.RightEye || switchToEye == Eye.Neither) && switchToEye != Eye.Both ? normalVisibility : eyePatchVisibility;

        while(currentColour.a < 1){
            yield return new WaitForFixedUpdate();
            
            currentColour.a = Mathf.Clamp01(currentColour.a + (fadeSpeed * Time.fixedDeltaTime));
            eyePatch.material.color = currentColour;
        }

        transitioning = false;
    }
}
