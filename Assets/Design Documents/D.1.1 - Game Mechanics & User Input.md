# D1.1 - Game Mechanics & User Input

| Game Mechanic              | Player Action                  | User Input                    | Feedback / Result                                       |
| -------------------------- | ------------------------------ | ----------------------------- | ------------------------------------------------------- |
| **Navigation**       | Move around the classroom      | `WASD` or Arrow keys        | The player moves smoothly within the environment.       |
| **Camera Control**   | Look around                    | Mouse movement                | First-person view rotation.                             |
| **Light Switch**     | Turn lights on or off          | `E` key near the switch     | Lights fade ON/OFF and a click sound plays.             |
| **Laptop**           | Turn laptop screen on or off   | Left mouse click              | Screen material changes and a typing sound plays.       |
| **Notebook**         | Read a short note              | `E` key near the notebook   | A small UI panel opens with text and a page-flip sound. |
| **Headphones**       | Start or stop background music | `E` key near the headphones | Music fades in or out smoothly.                         |
| **Window**           | Approach or click the window   | `E` key or automatic        | Soft outside ambient sound (birds or wind) plays.       |
| **Lighting Control** | Adjust overall light intensity | Indirect (via switch)         | Light intensity changes gradually.                      |
| **Narration**        | Intro voice line               | On start                      | A short welcome message is played once.                 |

### Notes

- All interactions are activated through a short-distance raycast.
- Every interaction includes sound or light feedback to improve immersion.
