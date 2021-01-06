using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class LipSyncBehavior : PlayableBehaviour
{
    public string dialogueText;
    public float clipDuration; 

    public bool hasSpoken = false;

    /* The speak method is called at the beginning of each lip sync clip in timeline */
    public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
        FaceRig rig = (FaceRig)playerData;
        if(!rig) { return; }
        if(!hasSpoken) {
            rig.Speak(dialogueText, ComputeTalkingSpeed());
            hasSpoken = true;
        }
    }

    /* Shorter timeline clips lead to faster talking speed */
    private float ComputeTalkingSpeed() {
        return clipDuration / dialogueText.Length;
    }

}
