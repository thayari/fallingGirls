using System;
using UnityEngine;

namespace PG.PlotManagement
{
    [Serializable]
    public class PGPlotRangeBetweenObjects : PGPlotCondition
    {

        public int objectIndex;
        private Transform _transform;
        public int objectTriggerIndex;
        private Transform _trigger;
        public enum ConditionType { Less, Greater };
        public ConditionType conditionType;
        public float radius;


        public override void OnStartCondition(PGPlotController plotController)
        {
            if (plotController.plotAsset.Objects[objectIndex] == "Player")
            {
                _transform = GameObject.FindGameObjectWithTag("Player").transform;
            }
            else
            {
                _transform = plotController.GetElement(objectIndex).gameObject.transform;
            }
            _trigger = plotController.GetElement(objectTriggerIndex).gameObject.transform;
        }
        public override void OnUpdateCondition(PGPlotController plotController)
        {
            switch (conditionType)
            {
                case ConditionType.Less:
                    if (Vector3.Distance(_transform.position, _trigger.position) < radius)
                    {
                        plotController.NextPlot(this);
                    }
                    break;
                case ConditionType.Greater:
                    if (Vector3.Distance(_transform.position, _trigger.position) > radius)
                    {
                        plotController.NextPlot(this);
                    }
                    break;
            }
        }
    }
}