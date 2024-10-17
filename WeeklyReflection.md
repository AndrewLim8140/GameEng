
# Weekly Reflection

## Week 4
Learnt about fuzzy logic in lecture, which allows the ingame entities to have human-like behavior. Despite the term fuzzy logic being new to me, i had experienced its
effects in games like alient isolation , which uses neural network , fuzzy logic for its enemies behavior which gives the impression as though the enemy adapts to the players
action, increasing the player's immersion. Whilst not to that extent, i am interested in learning about the ins and out of fuzzy logic as i personally believe that
enemies with ambigious resoning will improve immersion overall and players would not be able to easily guess the programming behind it.

**Practical**
> Learnt that adding vectors to an component does not nessasarily cause it to move to the intended location, this is caused as the vector being added to the component is relative to
> world axis and not the actual distance between the 2 objects.

> Dividing the vector into smaller "steps" allows for simulation of motion instead of instantly teleporting to the target.

> The usage of Time.deltaTime with normalization allows two components to travel at the exact same speed despite the difference in distance. Time.deltaTime is also useful in tracking
> the length of time since the last execution

> Unity has a built-in method to acquire vectors between objects , which can be used to pathfind to target.

> A projectile can be rotated mid-flight to simulate dropoff

**Self-Learning**
Did not spend much time this week on developing the personal project. However i did manage to find a material library `Poligon` that could be used in the final project. 
Its a material library for blender but it is also compatiable with unity. 

## Week 3
Learnt how to attach various effects onto in-game items : items , equips and drops, how to create a class and spawn in items onto the ingame world. Various functions
like inheritance, tags which is new to me , allows us to share codes within similar family.

**Ran the C# for beginner**
> Created a item spawner that spews out items onto the ingame world that uses a while loop to ensure that each items are of equidistance.

> Objects could have a tag that allows the engine to recognise what group it is part of. Allows us to declare multiple common code snippets that is shared
> amoungst multiple classes / objects.

> Noticed that there are multiple ways of codeing despite having the same outcome.

> Learnt about inheritance

**Self-Learning**

Noticed that my player character will experience a reproduceable bug that continously increases gravity acceleration if certain actions is conducted :

- Character repeatly falls off the platform without any jump input.
- Acceleration is repeately compounded , causing the player model down-ward moment to increase exponentially.

After scanning through the codes , realised that this was caused by the  `PlayerMovement.cs` which only resets `Acceleration = Gravity * Time.deltaTime` 
after a `jump`. This is remedied by introducing a flag that checks when a player is grounded via `characterController.isGrounded` and resetting the `ymovement`
direction. A cap to the `Acceleration` is also introduced to cap speed if the fall is too high up.

Created a trigger `capsule` that will be used to detect for an enemies's hitbox which scales accordingly to the duration that the left mouse button `Input.GetMouseButton(0)` 
is held down. The duration held down is calculated using `if (Time.deltaTime % 1 == 0)` which increments the `ChargeLvl` every seconds , capping at 5 secs.

**To-Do**
- Learn how to transform player's position to the enemies in the attack range, represented by the trigger capsule . The code `PlayerEntity.transform.position =
  teleportDestination.position` used by the out-of-bound trigger could be used, however i am unable to detect who is being hit. (i.e: multiple enemies , who to
  teleport to).
- Tried using `Nav Mesh Agent` for enemies movement, need to learn more about it.

**Issues**
Got a bit better at using GitBash, but as the new prefab has a large file`100mb` i had to use lfs .Tried calling the file name directly via `git lfs track [name]` but
it was unable to find the file. Thus i have to use`git lfs track --filename [name]` ,but the prefab name contains space it was unable to track the file. So in order
to upload to git, i had to rename and remove the spaces contained within the filename, only then i was able to track the files , commit and push to github.

## Week 2
Learnt that the vectors of an objects could be presented as a matrix , and how collision detection is conducted(Separating axis theorem and GJK algorithm) 
and its issues (i.e: tunneling). 

**Ran the FPS prefab and learnt various things :**
> Saving created components as presets for ease of usage

> Level creation could be easily created via a combination of pre-created area and a script to allow for easy chaining of levels

> Scene could be used as a different levels within the same game (i.e: level 1 , 2 etc)

> How to enable and use gizmo

> Various scripts used in the prefab could be used in the final project (mainly the endstage script , projectile collider and controller ( mainly for weapon
> selection and firing ) and how to conduct animations within unity.

Continued self-laerning and figured out sky-boxes , creation and application of materials , triggers , teleporting players and detection of mouseclicks.
Also learnt how unity render in objects via render queue and its effects.

However, i experienced some issues when attempting to upload my project into github via gitbash :
> Forget to change branch before pushing , when attempting to undo commit and push i accidentally removed all project files (except my self-learning files
> as its in another directory). Had to get LFS (large file system) to retrack all files before i can continue.

## Week 1

Installed unity engine onto my laptop and followed through with the weekly practical lessons, completing the nessasary tasks. Apart from that , 
i also played around with the components of the GameObject trying to figure out on what each additional components does.  

**Tinkered around and learnt a bunch of stuff from various tutorials as i am deeply interested in the unity engine :**  

> Managed to successfully allow player's input to affect the camera's behavior, created a 3d capsule with mesh that serves as the player's body and allows 
for interactions, a flat plane for the players and objects to stand on, added **mesh** and **mesh collider** to allow for collision. 

> Learnt the differences between the available character controller(rigitbody,builtin char controller and kinematic), each of their weakness and benefits i.e:
> interactions with physics.  

> Local's vs World's axis , which is crutial when a player-controlled camera is needed for the end product/game. As if the local and world's axis aren't aligned,
> the character would not move in the direction that the player intended and will instead move with reference to the world axis. And preventing the player from
> 'levitating' or 'clipping' when the camera is facing directly upwards or downwards.

**Experienced some issues when uploading the unity files to github :**  

> as both Prefab and Scratch files are too large to be directly uploaded, i have searched the web and had chanced upon LFS (Large File Storage)
that could be used to upload large files like Library and Assets and referenced by git when nessasary.

> Had various issues occured however (i.e : forbidden files (.gitattribute referencing another .gitattribute), unclear and vague documentation for LFS) but at the end of the day , i manageed to successfully upload
to git hub.
