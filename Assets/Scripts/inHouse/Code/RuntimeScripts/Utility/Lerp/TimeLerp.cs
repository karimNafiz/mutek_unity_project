using System;
using UnityEngine;

namespace Utility.Lerp
{
    public class TimeLerp : ILerpable
    {
        // the entire duration, the lerping will take place
        // this value will be in the denominator 
        // alphaFactor = time.DeltaTime / timeDuration
        private float timeDuration;
        private float accumulatedTime = 0f;
        
        public TimeLerp(float timeDuration)
        {
            this.timeDuration = timeDuration;
        }
        
        public override (bool, float) TickLerp()
        {
            accumulatedTime += Time.deltaTime;
            float t = accumulatedTime / timeDuration;
            lerpResult.isEndLerp = t >= 1f;
            lerpResult.lerpFactor = Mathf.Clamp01(t);

            return lerpResult;
        }
        
    }
}