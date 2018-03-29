using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTestingInput : MonoBehaviour {
	
	#region Variables
	[Header("Test Inputs - Music")]
	public KeyCode MusicStop = KeyCode.Q;

	[Header("Test Inputs - Player")]
	public KeyCode Footstep = KeyCode.F;
	public KeyCode PlayerHit = KeyCode.H;
	public KeyCode PlayerHitByWolf = KeyCode.J;
	[Header("Test Inputs - Sword")]
	public KeyCode PlayerDrawSword = KeyCode.T;
	public KeyCode PlayerSwordSwing = KeyCode.S;
	public KeyCode PlayerSwordHit = KeyCode.A;
	[Header("Test Inputs - Luger")]
	public KeyCode PlayerDrawLuger = KeyCode.D;
	public KeyCode PlayerLugerShot = KeyCode.L;

	[Header("Test Inputs - Mob")]
	public KeyCode BatAggro = KeyCode.U;
	public KeyCode BatWingLoop = KeyCode.I;
	public KeyCode WolfAggro = KeyCode.E;
	public KeyCode WolfAttack = KeyCode.W;
	public KeyCode WolfLunge = KeyCode.R;

	/* Private */
	bool ExpoMusicPlaying = true;
	bool BatWingLoopPlaying = false;
	
	#endregion
	
	void Update () {

		/* Music Events */
		if(Input.GetKeyDown(MusicStop)){
			if(ExpoMusicPlaying == false){
				CombattoExpoMusic();
				ExpoMusicPlaying = true;
			}
			else {
				ExpotoCombatMusic();
				ExpoMusicPlaying = false;
			}
		}
		
		/* Player Events */
		if(Input.GetKeyDown(Footstep)){
			PlayFootsteps();
		}
		if(Input.GetKeyDown(PlayerHit)){
			PlayPlayerHit();
		}
		if(Input.GetKeyDown(PlayerHitByWolf)){
			PlayPlayerHitByWolf();
		}

		/* Sword Events */
		if(Input.GetKeyDown(PlayerDrawSword)){
			PlayPlayerDrawSword();
		}
		if(Input.GetKeyDown(PlayerSwordSwing)){
			PlayPlayerSwordSwing();
		}
		if(Input.GetKeyDown(PlayerSwordHit)){
			PlayPlayerSwordHit();
		}
		if(Input.GetKeyDown(PlayerSwordHit) && Input.GetKeyDown(KeyCode.LeftShift)){
			PlayPlayerSwordSwing();
			PlayPlayerSwordHit();
		}

		/* Ranged Weapon Events */
		if(Input.GetKeyDown(PlayerDrawLuger)){
			PlayDrawLuger();
		} 
		if(Input.GetKeyDown(PlayerLugerShot)){
			PlayLugerShot();
		}

		/* Mob Events */

		/* Bat Events */
		if(Input.GetKeyDown(BatAggro)){
			PlayBatAggro();
		}
		if(Input.GetKeyDown(BatWingLoop)){
			if(BatWingLoopPlaying == false){
				StartBatLoop();
				BatWingLoopPlaying = true;
			}
			else {
				StopBatLoop();
				BatWingLoopPlaying = false;
			}
		}

		/* Wolf Events */
		if(Input.GetKeyDown(WolfAggro)){
			PlayWolfAggro();
		}
		if(Input.GetKeyDown(WolfAttack)){
			PlayWolfAttack();
		}
		if(Input.GetKeyDown(WolfLunge)){
			PlayWolfLungeAttack();
		}
	}
	
/* All SFX should be posted with a reference to their parent object,
	So that Fabric can track their 3D position and instances.
	If this isn't possible, just remove the ", gameObject" */

#region Player-SFX-Events
/***** Player SFX Events *****/

/*** Player Character Events ***/ 
	
	// Player footsteps
		void PlayFootsteps()
		{
			Fabric.EventManager.Instance.PostEvent("Footstep", gameObject);
		}
		
	// For when the player gets hit
	// Later we will have different versions for the amount of damage the player takes
		void PlayPlayerHit()
		{
			Fabric.EventManager.Instance.PostEvent("Player Hit", gameObject);
		}
		
	// For when the player is hit by a Wolf
		void PlayPlayerHitByWolf()
		{
			Fabric.EventManager.Instance.PostEvent("Player Hit By Wolf", gameObject);
		}

/*** Sword Events ***/
	
	// For when the player draws their sword
	void PlayPlayerDrawSword()
	{
		Fabric.EventManager.Instance.PostEvent("Player Draw Sword", gameObject);
	}

	// For *every time* the player swings their sword
		void PlayPlayerSwordSwing()
		{
			Fabric.EventManager.Instance.PostEvent("Player Sword Swing", gameObject);
		}

	// When the player swings and *hits* something and does damage
		void PlayPlayerSwordHit()
		{
			Fabric.EventManager.Instance.PostEvent("Player Sword Hit", gameObject);
		}

/*** Ranged Weapon Events ***/
	
	// For when the player switches to the Luger weapon
		void PlayDrawLuger()
		{
			Fabric.EventManager.Instance.PostEvent("Player Draw Luger", gameObject);
		}

	// Luger Shot (Triggers 3 different audio sources at once)
		void PlayLugerShot()
		{
			// Currently 1 shot pitch modulated, will fix later to be random container
			Fabric.EventManager.Instance.PostEvent("Player Luger Shot", gameObject);
		}

#endregion

#region Mob-SFX-Events
/***** Mob SFX Events *****/
	// For the associated monsters

/*** Bat Events ***/

	// Bat screech when it aggros the player
		void PlayBatAggro()
		{
			Fabric.EventManager.Instance.PostEvent("Bat Aggro", gameObject);
		}

	// When the bat starts moving towards the player begin the loop of its wings flapping
		void StartBatLoop()
		{
			Fabric.EventManager.Instance.PostEvent("Bat Start Wing Loop", gameObject);
		}

	// When the bat dies, stop the looping bat wing sound
	// Note to self: this event is set to override on the audiocomponent level, *not* event level
		void StopBatLoop()
		{
			Fabric.EventManager.Instance.PostEvent("Bat Stop Wing Loop", gameObject);
		}

/*** Wolf Events ***/

	// Whe the player is detected by a Wolf
		void PlayWolfAggro()
		{
			Fabric.EventManager.Instance.PostEvent("Wolf Aggro", gameObject);
		}
	
	// For when the wolf attacks
		void PlayWolfAttack()
		{
			Fabric.EventManager.Instance.PostEvent("Wolf Attack", gameObject);
		}
	
	// For the wolf's lunge attack
		void PlayWolfLungeAttack()
		{
			Fabric.EventManager.Instance.PostEvent("Wolf Lunge Attack", gameObject);
		}

#endregion

#region Music-Events
/***** Music Events *****/
	// Music Events don't need to pass a gameObject because they are entirely 2D.

	// Transitions music from Exploration to Combat
		void ExpotoCombatMusic()
		{
			Fabric.EventManager.Instance.PostEvent("Expo to Combat Music");
		}
	
	//Transitions music from Combat to Exploration 
		void CombattoExpoMusic()
		{
			Fabric.EventManager.Instance.PostEvent("Combat to Expo Music");
		}
}
#endregion