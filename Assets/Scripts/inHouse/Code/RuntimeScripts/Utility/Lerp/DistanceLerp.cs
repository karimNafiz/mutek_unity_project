using UnityEngine;

namespace Utility.Lerp
{
    public class DistanceLerp : ILerpable
    {
        private Transform trackTransform;
        private Transform targetTransform;
        private float startDistance;
        private const float epsilon = 0.01f;
        public DistanceLerp(Transform trackTransform, Transform targetTransform)
        {
            this.trackTransform = trackTransform;
            this.targetTransform = targetTransform;
            startDistance = Vector3.Distance(trackTransform.position, targetTransform.position);

        }
        public override (bool, float) TickLerp()
        {
            float currentDistance = Vector3.Distance(trackTransform.position, targetTransform.position);
            //Debug.Log("current distance " + currentDistance);
            float t = 1 - currentDistance / startDistance;
            lerpResult.isEndLerp = t >= (1f - epsilon);
            lerpResult.lerpFactor = Mathf.Clamp01(t);
            //Debug.Log("lerp result "+ lerpResult);
            return lerpResult;
        }
    }
}