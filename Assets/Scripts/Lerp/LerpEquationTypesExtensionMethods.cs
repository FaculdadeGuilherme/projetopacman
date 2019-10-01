using UnityEngine;
using System.Collections;



public static class LerpEquationTypesExtensionMethods {
	public static float GetEquationTransformedFactor(
		this LerpEquationTypes lerpEquation, float crudeFactor
	) {
		crudeFactor = Mathf.Clamp(crudeFactor, 0.0f, 1.0f);
		
		switch(lerpEquation) {
		case LerpEquationTypes.Quadratic:
			crudeFactor = crudeFactor * crudeFactor;
			break;
			
		case LerpEquationTypes.SmoothCos:
			crudeFactor = (Mathf.Cos(crudeFactor * Mathf.PI) * -0.5f) + 0.5f;
			break;

		case LerpEquationTypes.InverseQuadratic:
			crudeFactor = 1 - ((1 - crudeFactor) * (1 - crudeFactor));
			break;

        case LerpEquationTypes.Smootherstep:
            crudeFactor = crudeFactor * crudeFactor * crudeFactor * (crudeFactor * (6f * crudeFactor - 15f) + 10f);
            break;
        }
		
		return(crudeFactor);
	}



	public static Vector3 Lerp(
		this LerpEquationTypes lerpEquation, Vector3 start, Vector3 end, float crudeFactor
	) {
		return(Vector3.Lerp(start, end, lerpEquation.GetEquationTransformedFactor(crudeFactor)));
	}

    public static Vector3 Slerp(
        this LerpEquationTypes lerpEquation, Vector3 start, Vector3 end, float crudeFactor
    )
    {
        return (Vector3.Slerp(start, end, lerpEquation.GetEquationTransformedFactor(crudeFactor)));
    }

    public static Color Lerp(
        this LerpEquationTypes lerpEquation, Color start, Color end, float crudeFactor
    )
    {
        return (Color.Lerp(start, end, lerpEquation.GetEquationTransformedFactor(crudeFactor)));
    }

    public static float Lerp(
		this LerpEquationTypes lerpEquation, float start, float end, float crudeFactor
	) {
		return(Mathf.Lerp(start, end, lerpEquation.GetEquationTransformedFactor(crudeFactor)));
	}

    public static Quaternion Lerp(
        this LerpEquationTypes lerpEquation, Quaternion start, Quaternion end, float crudeFactor
    )
    {
        return (Quaternion.Lerp(start, end, lerpEquation.GetEquationTransformedFactor(crudeFactor)));
    }

    public static Quaternion Slerp(
       this LerpEquationTypes lerpEquation, Quaternion start, Quaternion end, float crudeFactor
   )
    {
        return (Quaternion.Slerp(start, end, lerpEquation.GetEquationTransformedFactor(crudeFactor)));
    }
}
