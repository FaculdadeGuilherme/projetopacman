// Utility class for interpolating values. Usage
//
//    LerpEquationTypes ival;
//    totalLerpDuration = 5;
//    countDownToTurnOff = 5;
//    No Update:
//    countDownToTurnOff -= Time.deltaTime;
//    lerpFactor = countDownToTurnOff / totalLerpDuration;
//    ival.Lerp(0, 10,  1 - lerpFactor); // interpolate from 0 to 10 using t
//
// Ideas for more curve types: http://sol.gfxile.net/interpolation/ 

public enum LerpEquationTypes {
    Smootherstep,
    Linear,
	SmoothCos,
	Quadratic,
	InverseQuadratic    
}