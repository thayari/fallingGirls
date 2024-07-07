using UnityEngine;

namespace PG.PlotManagement
{
    public class PGPlotAnimationBehaviour : PGPlotBehaviour
    {
        public int animatorObjectIndex;
        private Animator _animator;
        public enum AnimatorType { Trigger, Bool, Int, Float }
        public AnimatorType animatorType;
        public string parameterName;
        public bool parameterBool;
        public int parameterInt;
        public float parameterFloat;
        public override void OnStartBehaviourState(PGPlotController plotController)
        {
            _animator = plotController.GetElement(animatorObjectIndex).GetComponent<Animator>();

            switch (animatorType)
            {
                case AnimatorType.Trigger:
                    _animator.SetTrigger(parameterName);
                    break;
                case AnimatorType.Bool:
                    _animator.SetBool(parameterName, parameterBool);
                    break;
                case AnimatorType.Int:
                    _animator.SetInteger(parameterName, parameterInt);
                    break;
                case AnimatorType.Float:
                    _animator.SetFloat(parameterName, parameterFloat);
                    break;
            }
        }
    }
}