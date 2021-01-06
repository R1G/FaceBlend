using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LipSyncClip : PlayableAsset
{
    public string dialogueText;
    public float clipDuration;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
        var playable = ScriptPlayable<LipSyncBehavior>.Create(graph);

        LipSyncBehavior lipSyncBehavior = playable.GetBehaviour();
        lipSyncBehavior.dialogueText = dialogueText;
        lipSyncBehavior.clipDuration = clipDuration;

        lipSyncBehavior.hasSpoken = false;

        return playable;
    }
}
