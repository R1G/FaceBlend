using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(FaceRig))]
[TrackClipType(typeof(LipSyncClip))]
public class LipSyncTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject owner, int inputCount) {
        foreach(var clip in GetClips()) {
            var lipSyncClip = (LipSyncClip)clip.asset;
            if(lipSyncClip) {
                lipSyncClip.clipDuration = (float)clip.duration;
            }
        }

        return ScriptPlayable<LipSyncTrackMixer>.Create(graph, inputCount);
    }
}
