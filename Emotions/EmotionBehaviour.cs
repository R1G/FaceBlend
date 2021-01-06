using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class EmotionBehaviour : PlayableBehaviour
{
    public string emotion;
    public float intensity;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        FaceRig faceRig = (FaceRig)playerData;
        faceRig.SetEmotion(emotion, intensity*info.weight);
    }
}
