using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Viseme {
    public string shape;    /* name of the viseme, eg. 'ah', 'oh', 'f' */
    public int index;       /* index of the corresponding blendshape */
} 

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class FaceRig : MonoBehaviour
{
    public float blinkRate;
    public Viseme[] visemeMap;  /* Maps user-defined set of visemes to blendshapes */
    
    private SkinnedMeshRenderer mesh;  /* Must contain blendshapes used in animation*/
    private float talkingSpeed;
    private bool isTalking;


    private void Start() {
        mesh = GetComponent<SkinnedMeshRenderer>();
        StartCoroutine(BlinkRoutine());
        isTalking = false;
    }

    /* Call this method via timeline signals 
       Begins procedurally generated lip-sync animation */
    public void Speak(string dialogue, float _talkingSpeed) {
        if(!isTalking) {
            StopCoroutine("PlayShapes");
            talkingSpeed = _talkingSpeed;
            List<string> shapes = GetShapes(dialogue);  /* convert dialogue to visemes */
            StartCoroutine("PlayShapes", shapes);
            isTalking = true;
        }
    }

    /* Called by the custom emotion track to set emotion visemes */
    public void SetEmotion(string emotion, float intensity) {
        //search for correct index
        for(int i=0; i<visemeMap.Length; i++) {
            if(visemeMap[i].shape == emotion) {
                //lerp it to desired intensity
                StartCoroutine(LerpShape(visemeMap[i].index, intensity, 0.1f));
            }
        }
    }

    /* Used as a coroutine to blink at roughly the specified blinkRate */
    private IEnumerator BlinkRoutine() {
        int blinkIndex = -1;
        for(int i=0; i<visemeMap.Length; i++) {
            if(visemeMap[i].shape == "Blink") 
                blinkIndex = visemeMap[i].index;
        }

        while(true) {
            if(blinkIndex<0)
                break;
            StartCoroutine(Blink(blinkIndex));
            yield return new WaitForSeconds(blinkRate * Random.Range(2/3f, 4/3f));
        }
        yield return null;
    }

    /* Procedurally makes a single 'blink' animation */
    /* Humans usually blink within 1/10th of a second */ 
    private IEnumerator Blink(int blinkIndex) {
        StartCoroutine(LerpShape(blinkIndex, 100f, 0.05f));
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(LerpShape(blinkIndex, 0f, 0.05f));
        yield return null;
    }

    /* updates blendshape for each viseme on an interval */
    private IEnumerator PlayShapes(List<string> shapes) {
        for(int i=0; i<shapes.Count; i++) {
            ResetAllVisemes();
            SetShape(shapes[i]);
            yield return new WaitForSeconds(talkingSpeed);
        }
        ResetAllVisemes();
        isTalking = false;

        yield return null;
    }

    /* Converts a string of dialogue into a list of visemes */
    private List<string> GetShapes(string dialogue) {
        List<string> shapes = new List<string>();
        bool prevWasClosed = false; 
        char[] chars = dialogue.ToLower().ToCharArray();    /* Case-Insensitive */
        for(int i=0; i<chars.Length; i++) {
            string shape = CharToShape(chars[i]);
            bool currentIsClosed = (shape == " ");
            if(!prevWasClosed || !currentIsClosed)
                shapes.Add(shape);
            prevWasClosed = currentIsClosed;
        }
        return shapes;
    }

    /* Naive implementation of phonetic analysis */
    private string CharToShape(char c) {
        switch(c) {
            case 'a':
                return "Ah";
            case 'y':
                return "Ee";
            case 'e':
                return "Ee";
            case 'i':
                return "Uh";
            case 'o':
                return "Oh";
            case 'u':
                return "Uh";
            case 'w':
                return "W_Oo";
            case 'f':
                return "F";
            case 'v':
                return "F";
            case ',':
                return "PAUSE";
            case '.':
                return "PAUSE";
            default:
                return " ";
        }
    }
    
    /* Sets all blendshape values to 0 */
    private void ResetAllVisemes() {
        foreach (Viseme viseme in visemeMap) {
            StartCoroutine(LerpShape(viseme.index, 0f, talkingSpeed));
        }
    }

    /* Sets the specified blendshape to 100, if it exists */
    private void SetShape(string shape) {
        foreach(Viseme viseme in visemeMap) {
            if(viseme.shape == shape) {
                StartCoroutine(LerpShape(viseme.index, 100f, talkingSpeed));
                return;
            }
        }
    }

    /* Smoothly interpolates the blendshape to the desired value */
    private IEnumerator LerpShape(int index, float desired_value, float speed) {
        if(!mesh) { yield return null; }
        float current_value = mesh.GetBlendShapeWeight(index);
        for(float lerp_value=0f; lerp_value<=1.0f; lerp_value+=Time.deltaTime/speed) {
            current_value = Mathf.Lerp(current_value, desired_value, lerp_value);
            mesh.SetBlendShapeWeight(index, current_value);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}




