# Project Soul

Current Client Version 0.21

Works in Unity 2019.2.9 and newer versions.

Log Format: Name - Log Number - Push Date - Branch - Notable Changes

Stratos #1 18/10/19 Master 
1) Added several folders to the project.
2) Set the default preferences->external tools-> external script editor to Windows Visual Studio 2019
3) Added AI and Props tag.
4) Added Player, Game and Stats Managers and worked a bit on functionality.

Nikos #1 18/10/19 Master 
1) Added several scripts, folders, materials to the project.
2) Added Enemy tag.
3) Created a prototype of Attack mechanic in TestNick Scene.

Jim #1 26/10/19 Master
1) Added Cinemachine package to the project.
2) Added alot of Animation related things to the project.
3) Changed the Input settings at the Project Settings from Input (for controllers).
4) Added Movement. Animation. Attacking using controller buttons (Rb, Rt, Lb, Lt, Left Click). 
 -Not finished but good enough for prototyping.
 -Can focus on objects by pressing (F and/or Y analog button) and back again.
 -Based on good tutorials it's good stand-alone needs work to fit in game, although tutorial has more videos that will help.
 -Animations from tutorial and Mixamo.
 -Needs a ton more work.
 
Stratos #2 05/11/19 Master
1) Added IState,StateMachine for general use throughout the game.
2) Implemented FSM Logic on Game Manager and added 3 new state scripts along minor changes.
3) Populated stats manager.

Jim #2 20/11/19 Master
1) Added level prototype.
2) Added a temporary run button (left shift) and a jump button (space) for debugging purposes.

Nikos #2 26/11/19 Master 
1) Added Weapon Hook (at right shoulder wrist)
2) Changed Attack scripts
3) Updated Nikos Attack demo scene

Stratos #3 02/12/19 Master
This changelog entry includes commits pushed by Stratos on 06/11, 19/11, 21/11 and 02/12.
1) AI_Manager added but not implemented yet. Will provide hierarchical orders in certain scenarios.
2) AI_Goap CORE system implemented (includes STACK_FSM/GOAP_Planner/GOAP_Agent/IGOAP/GOAPAction).
3) Added CombatStats as required component to provide essential stats to the agent.
4) Added Archetype classes for our agents (Ranger/Warrior/Labourer(FriendlyNPCs)). Also, Knight - extension of Warrior.
5) Added archetype-based backpack components.
6) Added MeleeAttackAction and FinisherMeleeAction.
7) Final work needed to have prototype-ready agent is setting up animator and creating 1-2 animations.
Sub-note: Made changes on dummy agent in Envir scene and added AttackController script made by Nick to the scene.

Stratos #4 09/12/19 Master
1) Added player health on previous update.
2) Added Buff & Debuff classes along with StatusEffectDatabase class that has all the information for the buff/debuff ops.
3) Added Player Stats class that handles all the stat modifications, including between inventory-buffs-debuffs-health.
4) Finally, added IModifiable interface for Stats class to achieve communication with managers/other object. 

Jim #3 11/12/19 Master
1) Revamped some movement logic.
2) Added animations for attacking, attacks can be executed consecutively in a combo fashion. Each weapon has each own attacks/ animations,
	if there is no weapon, animations default to a punch.
3) Logic for attacking added to weapon animations, colliders on/ off are controlled in the animation clips by the equivalent animation name.

Nikos #3 13/12/19 Master 
1) Added ItemNikos class(Item name was taken) and ItemAttribute
2) Added ItemDatabase and ItemAttributeDatabase as ScriptableObjects in Resources
3) Updated Inventory Manager
4) Added Temporary pick up script with OnTriggerEnter
5) Created a demo Scene for Inventory and UI(future update)

Jim #4 14/12/19 Master
1) Added changelog for <Jim #3>.
2) Fixed a bug with oh_attack 3 animation event which let the player unarmed strike with weapon.
3) Fixed a bug with debug code for running which didn't properly reset the movement speed.
4) Removed some left over junk from previous projects - scripts folders not changed other than mine.
5) Soft removed Nikos' WeaponHook.cs and replaced with mine. The logic that it had is now controlled by the combination of WeaponHolderHook, WeaponHolderManager and WeaponItem.
6) Added some temporary code to AnimatorHook.cs so that animation event errors don't show up in the console.
7) Added the tag Enemy to the AI Dummy.
8) This is more of a reminder if someone needs a variable from the player it should in theory (hopefully) already exist in either CharacterStateManager or PlayerStateManager
	either way if you need something from them, reference them properly and get what you need. Also since that structure is StateManager> CharacterStateManager> PlayerStateManager
	you can just reference the PlayerStateManager and have access to whatever exists in the parent classes - unless they are protected variables in which case there should be a 
	get/ set method for it. For example if we need to see if the player is acting which currently he just attacks the variable isInteracting will be flipped to true and we can just reference
	that if we need to know if player is attacking.

Stratos #5 14/12/19 Master
1) Added developer console csgo-style in the project files. Enable in every scene is advised -> requires Event System & DeveloperConsoleObjectNew 
from prefabs folder to work. In unity play or exe press ` to activate and hooks in Debug.Log/Warning/Error messages. Only available command in v1.0 
is quit which exits play mode or closes the application. Needs evaluation of behaviour with other systems (player control).
2) Added AI_Showcase scene for future use.
3) In yesterday update added and edited a lot of AI scripts and animator.

Stratos #6 16/12/19 Master
1) Added AudioManager static class that can be called to apply positional and non-positional sound to the scene.
2) Edited a bit GameManager.
3) Added GameAssets script which supports our game with reference to every game asset needed. (Can apply item/buff database here)
4) Added a few test sounds.

Stratos #7 23/12/19 Master
1) Re-created GameManager based on Project needs (Requires further development in project).
2) Edited a bit Stats & AI manager.

Nikos #4 11/01/20 Master 
1) Created a simple inventory UI and connected it with picked up items

Jim #5 30/1/20 Master
1) Revamped Player Controller. The player as an entity exists from the scripts InputControl, CameraManager and Controller. Proper sample scene is JimScenebed for demo.
	- Movement is about the same WASD movement, Left Shift dodges, Left/Right click attacks with appropriate weapon and R locks on to first target in hierarchy that has or inherits ILockable.
2) Added sample AI animation logic that is consisted of AIController. More animations and behaviours to come in next patch.
3) Need to properly adapt AIController logic to GOAP agent logic. Currently hacked up - edited Warrior.cs to introduce some movement but it is not entirely correct.
4) AIController currently can spawn, chase the enemy using parameter based actions and use attacks appropriately to the parameters. Also can die and become ragdoll for fun.
5) Stopped using Cinemachine for player because it didn't have full control of it. Might introduce later for cinematics.
6) Bugs: plently will fix in future patches, ex.AIController gravity doesn't work, agent component "jerky" sometimes when near player, left to right combo doesn't transition to correct hand.
7) If lost in scripts try finding way around by searching by reference in Visual Studio until I add comments.
8) Replaced old CharacterController in scenes that used it. InventoryUIScene works better now that it doesn't have input problems, still need to when the UI is open disable by logic the InputControl. 
9) Cleaned up project abit.

Jim #6 9/2/20 Master
1) Added Parry & Backstabbing functionality left mouse triggers a parry depending on the frame of enemy animation you can time your left attack and stun the enemy then follow with a right attack to
	attack with a lunge and throw enemy backwards. If you go around the back of enemy you can backstab with right attack. Animations are wonky I know :C also damage doesn't get applied from those attacks
	I will fix later probably.
2) Added Sprint hold the dodge button down to sprint, tap to dodge and tap while stationary to jump back.
3) Added red dragon as a test for non-humanoids. It is hard to get good results from mecanim on non-humanoids. Also backstabbin and parry don't work against it.
4) Added sample quickslot ui, no functionality yet.

Stratos #8 29/2/20 Master
1) Added test scenes.
2) Edited ALL Goap scripts (working speciment is on StratosTestBed scene).
3) Added 4 new actions & edited Enemy Animation Controller to support them.
4) GOAP state is working. Requires Combat Stats and damage I/O which is the next top priority. Further actions must be deployed soon to showcase the depth of the system.

Stratos #9 1/3/20 Master
1) Edited AnimatorHook damageCollider logic. Player is unaffected by this change.
2) Added damage type on ActionContainer.
3) Edited Player Controller. Added health and damage input on player. Need to hook in UI/Combat stats and GameOver scene.
4) Major changes in GOAP Core and GOAP Action. Now supports full damage input and output (no combat stats yet).
5) Edited a bit GOAP actions to further support combat with colliders/damage.

Stratos #10 2/3/20 Master
1) Edited GOAP Core to use combat stats on both defensive and offensive operations.
2) Major changes in combat stats script for Agents to use. Will be used on player as well in future version.
3) GOAP Agent is ready for final release. Need a simple way to rotate when changing actions and is in range of player to attack.
   Other than that only design choices and required from now on in regards of AI.

Jim #7 3/3/20 Master
1) Added object pools for VFX objects currently only works for blood on hits.
2) Added various vfx: sandstorm, rain, waterfall and hooked up a fixed up sound system.
3) Various fixes.

Jim #8 3/3/20 Master
1) Added more sounds for the sound manager.
2) Changes on controller functionality, it handles better and arrow keys work.
3) More fixes all over the place.

Stratos #11 8/3/20 Master
1) Fixed rotation issues with AI. Now checks if enemy is in front before performing an action.
2) Fixed backstab/parry on AI side to properly stop AI current actions and mitigate extra damage. Then it re-plans.
3) Added backwards_left dodge on AI animation and hooked it with an action.
4) Edited GOAP_Core & all actions scripts.
5) Created AI prefab for use in main scene.

Stratos #12 10/3/20 Master
1) Added elevator in StratosScenebed & as a prefab for use in main scene.

Stratos #13 11/3/20 Master
1) Added 6 new actions with new animation & logic for a total of 10 actions available for GOAP.
2) Added new action animations in EnemyAnimationController
3) Patched AnimatorHook

Jim #9 11/3/20 Master
1) Changed Main Menu scene to adjust with screen size.
2) Changed how from main menu you go to game and About page, no need to load second scene for about page, also when starting level
	it loads it asynchronous and makes sure game is ready for player with a loading screen.
3) UI fixes all over the place, 
 - Added Health, Mana and Stamina for player, they change values accordingly.
 - Also using Nicks' previous HPController, re-added it to GOAP agents, it gets enabled when the player is targeting their specific target.
 - Quickslots icons change according to currently held item.
 - Picking up an Item checks if player already has it and if not adds it to his list of items. This is janky but it works.
4) Added more sounds.
5) Added more icons.
6) Changed/ Fixed various player related things.
7) Added helper class FastStats for super easy access(dirty) to stats, this is a buff.
8) Added FastStats to GoapCore for another super easy access, this is probably a nerf.
9) Unarmed and kick works again.
10) Changed player and canvas in StratosScenebed to test out things.
11) Added 2 new enemy models and rigged them to be operational by AI.
12) Including the 2 new models all current available AIs are in the Assets/ Prefabs/ AI. 3 with GOAP 2 with SimpleAI.

Stratos #14 11/3/20 Master
1) Fixed a performance bug in GOAP_Core.
2) Added AI & Elevator in mainScene.
