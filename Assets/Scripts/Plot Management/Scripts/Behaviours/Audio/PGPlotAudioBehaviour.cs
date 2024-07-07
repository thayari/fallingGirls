using UnityEngine;
namespace PG.PlotManagement
{
    public class PGPlotAudioBehaviour : PGPlotBehaviour
    {
        public int audioObjectIndex;
        private AudioSource _audioSource;
        public bool fromClip;
        public AudioClip clip;

        public override void OnStartBehaviourState(PGPlotController plotController)
        {
            _audioSource = plotController.GetElement(audioObjectIndex).GetComponent<AudioSource>();
            if (fromClip)
            {
                _audioSource.PlayOneShot(clip);
            }
            else
            {
                _audioSource.Play();
            }
        }
    }
}