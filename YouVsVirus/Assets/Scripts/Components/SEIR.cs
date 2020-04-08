using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEIR : MonoBehaviour
{

	public float S; //# of susceptibles
	public float E; //# of exposed
	public float I; //# of infectious
	public float R; //# of recovered

	public float S_old; //# of susceptibles
	public float E_old; //# of exposed
	public float I_old; //# of infectious
	public float R_old; //# of recovered

	public float D; //# of dead
	public float N = 100; //total population

	public float S0; //initial # of susceptible
	public float E0 = 1; //initial # of exposed
	public float I0 = 0; //initial # of infectious
	public float R0 = 0; //initial # of recovered
    // Assume an incubation period of 5.2 days
	public float t_incubation = 5.2f;
	// Assume infectious period of 2 days
	public float t_infectious = 2f;
	// social distancing rate
	public float rho = 1.0f;

	public float deltaT; //time step
	private float count = 0f;
	

	// Start is called before the first frame update
	void Start()
    {
		E = E0;
		I = I0;
		R = R0;
		S = N - E0 - I0 - R0;
		count = 0;
	}

   	void FixedUpdate()
	{
		
		float delta_t = Time.deltaTime;
		count+=delta_t;
		// alpha is the inverse of the incubation period (1/t_incubation)
		float alpha = 1.0f / t_incubation;
		// gamma is the mean recovery rate
		// inverse of the mean infectious period (1/t_infectious)
		float gamma = 1.0f / t_infectious;
		// contact rate beta
		// basic reproductive number of virus times inverse of mean infectious rate r_0 = 2.68 * gamma = 0.5
		float beta = 2.68f * gamma;
		// change in people susceptible to the disease
		// moderated by the number of infectious people and their contact with the infectious.
		float dSdt = -rho * beta * S * I / N;
		// people who have been exposed to the disease
		// grows based on the contact rate and decreases based on the incubation period
		// whereby people then become infectious
		float dEdt = rho * beta * S * I / N - alpha * E;
		// change in infectious people based on the exposed population and the incubation period
		// decreases based on the infectious period: the higher gamma is, the more quickly people die/recover
		float dIdt = alpha * E - gamma * I;
		// no longer infected: immune or diseased
		float dRdt = gamma * I;

		S = S + delta_t * dSdt;
		E = E + delta_t * dEdt;
		I = I + delta_t * dIdt;
		R = R + delta_t * dRdt;
		if(Time.frameCount%100 == 0)
        {
			print(count);
			print(Time.timeScale);
			print(Time.fixedTime);
			print(delta_t);
			print(Time.fixedDeltaTime);
			print(Time.time);
			print(S);
			print(E);
			print(I);
			print(R);
		}
	}
}
