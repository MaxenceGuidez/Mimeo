# Methods and Choices

## Menus
I wanted to create a user-friendly main menu with large buttons, a [cool image](https://fr.freepik.com/photos-gratuite/concept-architecture-copyspace-gauche_2369069.htm#query=reconstruction%20a%20resolution%208k&position=35&from_view=keyword&track=ais_user&uuid=99378784-87cc-48b6-8b22-fe8c988e216a) found on the internet, and minimal information at the beginning. I also wanted to add a settings menu because, to me, it is essential in any project; the user should be able to change the basic settings. Additionally, I included a pause menu to avoid having to quit with Alt+F4 and to allow the user to return to the main menu.

## Map
Initially, I envisioned it as a structure in the middle of an empty map, similar to Bimeo's rendering, and in the end, it turned out exactly as I imagined. I found a [free simple construction](https://assetstore.unity.com/packages/3d/environments/urban/modular-abandoned-slaughterhouse-lite-58082) on the Unity Asset Store and placed it in the middle of an empty terrain. After that, I added some textures to the ground (also using [texture assets](https://assetstore.unity.com/packages/2d/textures-materials/world-materials-free-150182) found on the asset store) and mountains, just to avoid having an unattractive map (even though it still isn't perfect...).

## Movements

I created a free-flying camera to navigate the map effectively, especially within the model. I added a small sphere collider to prevent passing through walls. I also used Unity's new input system.

## Selection system
To interact with the elements, I chose a very user-friendly selection system. With a crosshair, you just need to hover over the elements and click on them to select them. This displays the information panel, which shows all four specifications. When we're not selecting but just hovering, the information panel is still slightly visible to allow reading the name of the currently highlighted element. To change materials available in the dropdowns, there are two variables allowing you to modify them, and everything will be automatically taken care of.

## More
I added music and SFX, using code that I had already used in an [old project](https://github.com/MaxenceGuidezCollege/AEON/).

## To upgrade
These are points that could be improved in the future:
- Implement a save system using Unity's UserPrefs or encrypt the data in another file.
- Enhance the selection system to allow selecting multiple elements at the same time, enabling modifications to all of them at once.
- Add more specifications to modify. Also, include a preview in the information panel to show the new material before applying it to the element.
- Create more beautiful dropdowns, adding the color of each option in the background, and the same for the textures. Also, add a final option in each dropdown, like a "+" option, that allows users to use their own colors and textures.

## Bugs
These are issues I haven't had time to fix:
- The original brick texture remains visible even if we change the texture; it just stays in the background.
- If we select an element that has already been modified, it resets to its original color and texture.
- Some of the textures I found on the internet are rotated 90 degrees.
- 