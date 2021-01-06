using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EmotionClip : PlayableAsset
{
    public string emotion;
    public float intensity;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<EmotionBehaviour>.Create(graph);
        EmotionBehaviour emotionBehaviour = playable.GetBehaviour();

        emotionBehaviour.emotion = emotion;
        emotionBehaviour.intensity = intensity;

        return playable;
    }

}
