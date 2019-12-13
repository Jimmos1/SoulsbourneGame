# Project Soul

Current Client Version 0.01

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
2) Added 

Nikos #3 13/12/19 Master 
1) Added ItemNikos class(Item name was taken) and ItemAttribute
2) Added ItemDatabase and ItemAttributeDatabase as ScriptableObjects in Resources
3) Updated Inventory Manager
4) Added Temporary pick up script with OnTriggerEnter
5) Created a demo Scene for Inventory and UI(future update)