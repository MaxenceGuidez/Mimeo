# Scripts
This file show you recap of all scripts of the Mimeo app.


## Main scripts
### FPSController
The FPSController class is the core component that manages first-person movement and camera control within the Mimeo application. It provides smooth and responsive controls for navigating the 3D house model, including movement, sprinting, and looking around. The class handles user input to control the player's movement speed and direction, as well as the camera's pitch and yaw, ensuring an immersive first-person experience. By locking the cursor and managing its visibility, FPSController creates a focused and uninterrupted interaction environment. This class is essential for providing an intuitive and engaging way for users to explore and interact with the house model, making it a key part of the Mimeo renovation application.

### Selector
<warning>TODO</warning>

### BuildElement
The BuildElement class represents a customizable part of the build model within the Mimeo application. Each instance of BuildElement can be individually selected and modified by the user, allowing for dynamic changes to its color and texture. The class manages the visual state of the element, including highlighting and selecting states, which helps users identify and interact with different parts of the model. By providing methods to change the appearance of the elements, BuildElement enables a high level of customization, allowing users to experiment with different design choices. This class is fundamental to the renovation system in Mimeo, offering an interactive and engaging way for users to personalize the house model.


## Manager scripts
### InputsManager
The InputsManager class in Mimeo is designed to handle all user input actions. It integrates with New Unity's Input System to manage inputs from various devices, ensuring smooth and responsive control for the user. This class enables and disables different input actions based on the current state of the application, such as during gameplay or while navigating menus. It also listens for specific input events like sprinting, selecting, pausing, and opening settings, and triggers the corresponding actions within the application. By providing a centralized management system for user inputs, InputsManager ensures that user interactions are intuitive and consistent throughout the application.

### AudioManager
The AudioManager class is a crucial component of the Mimeo renovation application, responsible for managing all audio-related functionality. It serves as the central hub for playing background music, handling sound effects for user interactions, and maintaining a seamless audio experience throughout the application. The class manages a playlist of audio clips, enabling continuous background music playback and automatically transitioning to the next track when one ends. It also provides functions to play specific sound effects, such as button hover and click sounds, enhancing the user's interactive experience. By using the singleton pattern, AudioManager ensures that there is only one instance of the class active at any time, preserving audio settings and states across different scenes.


## Menu scripts
### MainMenu
The MainMenu class represents the main entry point for users into the Mimeo application. It provides the interface for navigating to different parts of the application, including starting the renovation simulation, accessing settings, and quitting the application. The class includes methods that handle button click events, ensuring that user interactions trigger the appropriate responses, such as transitioning to the next scene or toggling the settings menu. The integration with AudioManager ensures that audio feedback is provided for button interactions, enhancing the user experience. The MainMenu class is essential for guiding users through the initial steps of their interaction with Mimeo, providing a smooth and engaging start to their experience.

### SettingsMenu
The SettingsMenu class provides a comprehensive interface for users to customize their Mimeo application settings. It includes options for adjusting music and sound effects volume, toggling fullscreen mode, and selecting screen resolutions. The class initializes these settings based on the current configuration and updates them according to user input. It interacts with the AudioManager to apply volume changes in real-time and with Unity's Screen class to adjust display settings. The SettingsMenu ensures that users can tailor their experience to their preferences, offering flexibility and control over the application's audio and visual settings. By providing these customization options, the SettingsMenu enhances the overall usability and satisfaction of the Mimeo application.

### PanelInfos
<warning>TODO</warning>

### PauseMenu
The PauseMenu class in Mimeo is designed to manage the in-game pause functionality. It provides users with the ability to pause the game, access settings, and navigate back to the main menu or quit the application. When the game is paused, the class ensures that the game state is properly handled, freezing the game time and unlocking the cursor for menu navigation. It integrates with InputsManager to disable in-game controls while the pause menu is active and re-enable them upon resuming. The PauseMenu also interacts with AudioManager to provide audio feedback for button interactions, maintaining a consistent user experience. This class is vital for offering users control over their game session, allowing them to take breaks, adjust settings, and manage their gameplay experience effectively.
