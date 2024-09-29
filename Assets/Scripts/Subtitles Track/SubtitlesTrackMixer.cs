using PG.LocalizationManagement;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace PG.LocalizationManagement.SubtitlesCutscene
{
    public class SubtitlesTrackMixer : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            TMP_Text tMP_Text = playerData as TMP_Text;
            string text = "";
            float currentAlpha = 0f;
            Color color = Color.white;

            if (!tMP_Text)
            {
                return;
            }

            int inputCount = playable.GetInputCount();

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                if (inputWeight > 0)
                {
                    ScriptPlayable<SubtitlesBehaviour> scriptPlayable = (ScriptPlayable<SubtitlesBehaviour>)playable.GetInput(i);
                    SubtitlesBehaviour subtitlesBehaviour = scriptPlayable.GetBehaviour();
                    if (Application.isPlaying)
                    {
                        text = LocalizationManager.Instance.GetLocalizedValue(subtitlesBehaviour.subtitlesKey);
                    }
                    else
                    {
                        text = subtitlesBehaviour.subtitlesKey;
                    }
                    color = subtitlesBehaviour.color;
                    currentAlpha = inputWeight;
                }
            }

            tMP_Text.text = text;
            tMP_Text.color = new Color(color.r, color.g, color.b, currentAlpha);
        }
    }
}
