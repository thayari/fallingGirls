using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace PG.LocalizationManagement.SubtitlesCutscene
{
    [TrackBindingType(typeof(TMP_Text))]
    [TrackClipType(typeof(SubtitlesClip))]
    public class SubtitlesTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<SubtitlesTrackMixer>.Create(graph, inputCount);
        }
    }
}