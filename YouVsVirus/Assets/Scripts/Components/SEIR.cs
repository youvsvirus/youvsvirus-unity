using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The SEIR model differential equations with social distancing factor rho.
/// with rho = 1 standard seir model (no distancing)
/// with rho = 0 quarantine
/// </summary>
public class SEIR : MonoBehaviour
{
	/// <summary>
	/// basic reproductive number of virus
    /// how many other people does one persion infect
	/// </summary>
    public float r_0;

	/// <summary>
	/// # of susceptibles
	/// </summary>
	public float S; //
	/// <summary>
	/// # of exposed
	/// </summary>
	public float E; //# of exposed
	/// <summary>
	/// # of infectious
	/// </summary>
	public float I; //# of infectious
	/// <summary>
	/// # of recovered (or dead)
	/// </summary>
	public float R; 

	/// <summary>
	/// Stuff set in the main menu.
	/// </summary>
	private LevelSettings levelSettings;
	//total population
	private float N ;
	/// <summary>
	/// Assumed incubation period
	/// </summary>
	public float t_incubation;
	/// <summary>
	/// Assumed infectious period
	/// </summary>
	public float t_infectious;

	/// <summary>
	/// opposite of social distancing rate 
	/// </summary>
	private float rho;

	// Start is called before the first frame update
	void Start()
	{
		levelSettings = LevelSettings.GetActiveLevelSettings();
		N = levelSettings.NumberOfNPCs;

		// Initial values for ODE
		// initial # of exposed
		E = levelSettings.NumberInitiallyExposed;
		// initial # of infectious
		I = levelSettings.NumberInitiallyInfectious; 
		// initial # of recovered
		R = 0;
		// initial # of susceptible / well
		S = N - E - I - R;
		// r_0 = 2.68f; for COVID-19
		r_0 = 2.68f;
		// in this model rho is the opposite of our social distancing factor (we make this 0.5 higher than usual)
		rho = 1 - levelSettings.SocialDistancingFactor / 100 + 0.5f;
		t_incubation = 5.2f;
		t_infectious = 2f;
	}

	// frame independent update for physics calculations
	void FixedUpdate()
	{
		// our time step
		float delta_t = Time.deltaTime;	// this returns the Time.fixedDeltaTime
		// alpha is the inverse of the incubation period (1/t_incubation)
		float alpha = 1.0f / t_incubation;
		// gamma is the mean recovery rate
		// = inverse of the mean infectious period
		float gamma = 1.0f / t_infectious;
		// contact rate beta
		// basic reproductive number r0 of virus times gamma 
		float beta = r_0 * gamma;
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

		// do some euler time stepping
		S = S + delta_t * dSdt;
		E = E + delta_t * dEdt;
		I = I + delta_t * dIdt;
		R = R + delta_t * dRdt;
		
		//if (Time.frameCount % 100 == 0)
		//{
		//	print(Time.fixedTime);
		//	print(E);
		//	print(I);
		//	print(R);
		//}
	}
}