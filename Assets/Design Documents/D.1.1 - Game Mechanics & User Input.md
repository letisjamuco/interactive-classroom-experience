# D1.1 - Game Mechanics & User Input

| Game Mechanic              | Player Action                  | User Input                    | Feedback / Result                                       |
| -------------------------- | ------------------------------ | ----------------------------- | ------------------------------------------------------- |
| **Welcome Screen**         | View game instructions         | On start (auto-displayed)     | UI panel with controls; close with SPACE/ENTER.         |
| **Navigation**             | Move around the classroom      | `WASD` or Arrow keys          | The player moves smoothly within the environment.       |
| **Camera Control**         | Look around                    | Mouse movement                | First-person view rotation.                             |
| **Crouch**                 | Lower camera height            | `C` key                       | Player crouches smoothly with adjusted camera position. |
| **Light Switch**           | Turn lights on or off          | `E` key near the switch       | Lights toggle ON/OFF, intensity changes, click sound.   |
| **Laptop**                 | Turn laptop screen on or off   | Left mouse click              | Screen material changes and a typing sound plays.       |
| **Notebook**               | Read a short note              | `E` key near the notebook     | A small UI panel opens with text and a page-flip sound. |
| **Headphones**             | Start or stop background music | `E` key near the headphones   | Music fades in or out smoothly.                         |
| **Window**                 | Approach or click the window   | `E` key or automatic          | Soft outside ambient sound (birds or wind) plays.       |
| **Day/Night Cycle**        | Toggle lighting environment    | `N` key                       | Skybox and lighting switch between day and night.       |
| **Narration**              | Intro voice line               | On start                      | A short welcome message is played.						|

## Notes
- A **welcome screen** is displayed at game start with full control instructions. Player dismisses it with SPACE or ENTER to begin gameplay.
- All interactions are activated through a short-distance **raycast** or **trigger zones**.
- Every interaction includes **sound or light feedback** to improve immersion.
- **Crouch** (`C` key) allows the player to lower their viewpoint for added realism.
- The **notebook UI** temporarily disables WASD controls (to avoid conflicts while typing), but arrow keys and mouse look remain functional.
- The **laptop** uses a trigger zone for proximity detection and a raycast for click interaction.