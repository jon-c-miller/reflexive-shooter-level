### Reflexive Shooter Level

An example of a endless level first person shooter with a modular infrastructure and reactive aiming elements.

<details>
  <summary>Project Architecture</summary>
  <ul><br>
	  <b>Core Direction</b>
	  <br>
	  The goal for this project was to implement a module-based architecture alongside a notify system in order to enable and encourage scalability and compartmentalization. As the focus is heavily on code and project infrastructure, design elements are intentionally kept to a minimum. 
	  <br><br>
	  <b>Modules</b>
	  <br>
	  Each module, or aspect, of the project is designed to be as independent as possible, with some relying only on dependency injection made from other modules through the notify system during Start().
	  <br><br>
	  A simple implementation of Model-View-Controller was used to group each module into data adjustable in the editor (model), representation of the module onscreen (view), and a coordinator (controller) to manage the two and receive messages from other modules.
	  <br><br>
	  <b>Notifies Pipeline</b>
	  <br>
	  In order to plan for project scaling where there may be a higher risk of race condition causing unwanted behavior, a synchronous notification system was used. This lightens the per-frame load, allowing logic to be spread over multiple frames; and also provides a built-in way to log notify calls for inspection in the editor.
	  <br><br>
	  In addition to the notify itself, the notify queue can accept an indefinite amount of additional parameters as an object array, allowing for effective one-way communication between modules. Notifies make use of xml tags to make it easy to see at-a-glance (when cursoring over the notify name) what parameters are intended to be passed in, if any.
	</ul>
</details>

<details>
  <summary>Features</summary>
  <ul><br>
	  <b>AI Controller</b>
	  <br>
	  This module manages AI unit spawning, and keeps a reference that allows individual AI units to detect and attack the player. Notifies upon AI units being destroyed are also filtered in this module, and level completion is triggered when all units have been destroyed. A cached reference to an interface (obtained from the player during level entry detection via the notifies pipeline) providing access to the player hit detection transform's position allows the module to carry out requests from AI units to Raycast to and fire at the player's current position.
	  <br><br>
	  <b>AI Unit</b>
	  <br>
	  An AI Unit represents an individual enemy unit in the level. These enemies are very simple, and do not move. While the player is in the combat area, they direct the AI controller Raycast to the player's position at intervals to check that it is visible and that a shot to it is expected to hit. If the Raycast hits an object with the player's tag, they then direct the AI controller to launch a projectile in that direction, and continue doing so at intervals. Requesting a Raycast to the player's position once again before requesting a launch provides a way to update the launch direction and halt the attack loop if the player is no longer in sight. AI Units are pooled to keep effects from garbage collection at a minimum. 
	  <br><br>
	  <b>Game Controller</b>
	  <br>
	  This module manages the overall flow of the game, filtering notifies that handle major changes in game state (such as level entry and completion), and generally orchestrating the operation of other modules. A simple screen fader is used to briefly delay some calls that cause abrupt changes in view to not occur until the screen is faded to opaque. This ensures that the player isn't alarmed and the experience is kept smooth.
	  <br><br>
	  <b>HUD Controller</b>
	  <br>
	  This module manages the heads up display for the player, and listens for updates to the current score, enemy count in the level, current player health, and the current level itself. Instead of a single canvas containing all of these elements, a separate canvas for each element was used to avoid superfluous canvas updates to those elements that haven't changed.
	  <br><br>
	  <b>Player Camera</b>
	  <br>
	  This module provides a first person view with 360° rotation on the y-axis and configurable rotation on the x-axis. On Start(), it passes via the notifies pipeline the transform of the camera object for any modules that need access to the camera's position or facing. The camera logic itself was more or less written from scratch, and is intended to be as simple and performant as possible while providing a smooth experience. A cached reference to an interface (obtained during Start() via the notifies pipeline) providing access to the player movement transform's position allows the camera to constantly stay with the player.
	  <br><br>
	  <b>Player Firing</b>
	  <br>
	  This module manages the aiming reticle and projectile launching for the player. Object pooling is used for projectiles to keep effects from garbage collection at a minimum. A cached reference to the player camera module's camera transform (obtained during Start() via the notifies pipeline) allows the launch point's position to be updated based on the camera's position and a configurable offset. Lerping is used to smooth the fire point's position, creating a reflexive experience for the player, with the launch point lagging slightly behind movement and camera view changes. The reticle itself is designed to be indicative of where a shot will hit, regardless of distance.
	  <br><br>
	  <b>Player Hit Detection</b>
	  <br>
	  This module provides a way for incoming AI projectiles to damage the player, as well as trigger level entry and exit notifies through level entry detection. Interaction with the level entry collision system provides a way to enable and disable some modules that are intended to be available only in the combat area. A cached reference to an interface (obtained during Start() via the notifies pipeline) providing access to the player movement transform's position allows hit detection to constantly stay with the player.
	  <br><br>
	  <b>Player Movement</b>
	  <br>
	  This module manages player directional movement and jumping. On Start(), it passes via the notifies pipeline an interface hosted in the view for any modules that need access to the player's position. During movement input, force is used to continually add velocity to the module's rigidbody, which is curbed against infinite acceleration by a physic material on the floor. A cached reference to the camera's transform (obtained during Start() via the notifies pipeline) allows the movement's facing to constantly copy the camera's y rotation.
	  <br><br>
	  <b>Sound Controller</b>
	  <br>
	  This module listens for any calls to play a sound, and filters the call based on an enum of SoundIDs. Sounds that are expected to overlap make use of PlayOneShot(), while normal sounds such as a jingle on level start use Play(). Requests to play enemy firing and player hit sounds are limited based on a frame count delay due to overlapping voice constraints and the potential for them to be exceeded during higher levels where there are many enemies.
	</ul>
</details>

<details>
  <summary>Challenges and Learning</summary>
  <ul><br>
	  <b>Solving for Minimal Dependency</b>
	  <br>
	  As an FPS, the camera plays a central role in multiple aspects of gameplay, such as movement direction, fire direction, and character facing. One solution to integrate these is to simply attach the camera to the player movement object, and add a transform as a child of the camera to function as a firing offset point. However, this ran counter to the aim of keeping each module in a self-contained prefab, so it was necessary to carefully plan out how the elements would interact.
	  <br><br>
	   It was decided that the player camera module would announce its camera transform through the notify system on Start(), and any module needing the camera facing, etc. would be listening for it and cache it. A similar but more indirect solution was devised for the modules needing to follow the player's movement by copying its current position, in which case the player position is accessible via an ICanBeTargeted interface cached by listeners on Start().
	  <br><br>
	  <b>Aiming to Conquer Firing Offsets</b>
	  <br>
	  The next main difficulty was in creating offsets for firing based on camera facing. Keeping independent and separate modules meant that a bit of vector math was needed to correctly apply offsets to the camera's position to accurately move the launch point each frame. Getting these offsets right and designing consistent behavior was challenging, and provided a great opportunity to strengthen valuable vector manipulation skills in a practical context.
	  <br><br>	  
	  <b>Constructing a Reliable Reticle</b>
	  <br>
	  A fairly labor-intensive aspect was the design of the reflexive aiming system. In many (if not most) FPS aiming implementations, the target reticle is simply a screen overlay, and is thus not a true estimate of where the shot will land (especially at close ranges). If the firing vector is calculated based on the reticle's projected position onscreen as a solution to this, the sudden changes in shot direction can be alarming and cause the user experience to suffer. In order to avoid this type of inconsistency and have a reticle that reliably predicted a launched projectile's hit point, a 3D sphere object serving as the reticle along with a Raycast system was used.
	  <br><br>
	  In order to obtain an end point beyond a given aimed at object, a light amount of vector math using an arbitrary distant point (default of 750 units away) extended from the camera's forward facing is used. A directional vector is then created using this point and the camera's current position. This allows a Raycast to attempt to extend a ray to that distant point every frame, and let an interception point from a hit object serve as the reticle's intended position.
	  <br><br>
	  In order to ensure that the reticle appears the same size regardless of distance, the reticle object's scale is updated based on a multiplier along with the square distance between the hit point and the camera's current position. As a sky is generally not intended to play a part in aiming, it was decided that the reticle would simply be hidden when not aiming at an object in the level. In this case, the reticle scale is kept at 0 and its position is set to directly in front of the camera. Because the reticle's position is lerped, it appears to quickly snap back to the hit point, creating a fluid experience when the player again aims at a valid object.
	  <br><br>
	  <b>Economizing on Target Logistics</b>
	  <br>
	  As this project is intended to be an endless level game with the number of AI units increasing infinitely as long as the player can continue completing levels, there was the potential for an undesirably high amount of references if the targeting system was not designed carefully. In order to avoid this, it was decided that a single entity would be responsible for keeping the reference to the player: an AI controller.
	  <br><br>
	  After some trial and iteration, this was taken a step further, with player visible checking and firing logic also being moved into the AI controller, making it responsible for handling all of the calculation for AI Units. The AI controller was given a reference to the player hit interface, as well as check visible and launch methods that take a Vector3 for origin position. Access to these was hidden behind an interface which is injected to AI units when spawning them. AI units then use that cached interface reference to request a check to see if the player is visible from their position, and to request a launch if so.
	  <br><br>
	  <b>Managing Vocals</b>
	  <br>
	  One of the more interesting challenges in this project was in ensuring that the sound controller could properly handle requests to play overlapping sounds. At high levels where there are many AI units and there are tens of shots being made at the player every second, pushing all sound requests through results in the sound core quickly being overwhelmed and ceasing to play sound correctly due to simultaneous voice count constraints.
	  <br><br>
	  Designing to account for this required an approach from two angles. First, integer fields for each high frequency sound to be managed were added in the model to track the current 'queue' size for each sound. This allows incoming requests for these sounds to be rejected if the current queue size is greater than a predetermined max queue count per sound variable. Second, rather than play one sound per frame, sounds with a queue count over 0 are played once every x frames during Update(). This solution resulted in smooth audio performance that capped repeated playback at a reasonable frequency without overloading the audio core. 
  </ul>
</details>

<details>
  <summary>Ideas for Future Additions</summary>
	  <ul><br>
		  · A level layout controller which randomizes the position of obstacles in the map using an array of Obstacle class objects that define their own constraints for randomization (+ or -10 units along z-plane, etc.)
		  <br><br>
		  · Lerping UI text elements upon changes, or replacing them entirely with bars or other image-related elements
		  <br><br>
		  · An actual model to represent the player's firing apparatus
		  <br><br>
		  · Powerups, such as those that grant increased movement speed or invulnerability
		  <br><br>
		  · An ammo system that forces the player to fire conservatively
		  <br><br>
		  · A mechanic that encourages the player to try to complete levels faster
	  </ul>
</details>

<details open>
  <summary>How to Use the Project</summary>
	  <ul><br>
		  <b>1.</b> Open project in Unity3D as usual
		  <br>
		  <b>2.</b> Enable gizmos in Play Mode
		  <br>
		  <b>3.</b> Run MainLevel scene
		  <br><br>
		  <i>Created using Unity version 2022.3.9f1</i>
	  </ul>
</details>