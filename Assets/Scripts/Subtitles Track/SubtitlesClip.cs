using UnityEngine;
using UnityEngine.Playables;

namespace PG.LocalizationManagement.SubtitlesCutscene
{
    public class SubtitlesClip : PlayableAsset
    {
        public Color color;
        public string subtitlesKey;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<SubtitlesBehaviour>.Create(graph);

            SubtitlesBehaviour subtitlesBehaviour = playable.GetBehaviour();

            subtitlesBehaviour.color = color;
            subtitlesBehaviour.subtitlesKey = subtitlesKey;

            return playable;
        }
    }
}