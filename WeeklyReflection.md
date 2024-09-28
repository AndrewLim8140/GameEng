
# Weekly Reflection

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
