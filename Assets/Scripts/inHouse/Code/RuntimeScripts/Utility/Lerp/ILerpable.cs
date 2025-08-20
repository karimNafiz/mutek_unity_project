namespace Utility.Lerp
{
    public abstract class ILerpable
    {
        // lerp result tuple definition
        protected (bool isEndLerp, float lerpFactor) lerpResult;
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// bool - isEndLerp
        /// float - lerpFactor
        /// </returns>
        public abstract (bool, float) TickLerp();
        
    }
}