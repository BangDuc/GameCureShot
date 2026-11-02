# GameCureShot

> A 2D point-and-click survival and puzzle game developed with Unity and C#.

**Status:** Work in Progress  
**Team Size:** 3  
**Engine:** Unity  
**Language:** C#  

---

## 🎮 About the Game

CureShot is a 2D point-and-click survival/puzzle game set inside a mysterious
house affected by supernatural anomalies.

The player explores different rooms, interacts with the environment, and uses
a camera to discover and photograph unusual phenomena while trying to survive
each run.

The game combines exploration, puzzle-solving, survival mechanics, resource
management, and progression between runs.

---

## 🔁 Core Gameplay Loop

```text
Explore the house
        ↓
Investigate objects and anomalies
        ↓
Use the camera to collect evidence
        ↓
Complete objectives and survive the run
        ↓
Receive resources and rewards
        ↓
Purchase items / upgrades
        ↓
Prepare for the next run
```

Runs take place within a limited amount of time.

Taking damage can cause the current run to fail, while repeated failures affect
the player's permanent progression.

---

## 📷 Core Features

### Camera Investigation

The camera is one of the main interaction tools in the game.

Players can:

- Enter camera inspection mode
- Focus on evidence targets
- Navigate between available targets
- Photograph supernatural anomalies
- Use collected evidence as part of objectives and progression

### Survival

Each run has its own survival state.

Dangerous interactions and hostile entities can reduce the player's health.
Failing a run may also affect permanent health and long-term progression.

### Point-and-Click Interaction

The game uses mouse-based interaction for exploring rooms, interacting with
objects, inspecting the environment, and solving puzzles.

### Inventory & Items

Players can collect and use items that affect gameplay and interactions.

The inventory system is designed to support both purchasable items and items
discovered during runs.

### Shop & Progression

Resources earned during gameplay can be used between runs to purchase items
and improve the player's ability to survive and investigate the environment.

### Day / Run Progression

The game is structured around repeated runs and progression between them.

Run results are summarized before the player proceeds to the next stage of
progression.

---

## 🧩 Game Flow

```text
Main Menu
   ↓
New Game / Continue
   ↓
Shop / Preparation
   ↓
Survival Run
   ↓
Camera Investigation
   ↓
Objectives / Evidence
   ↓
Run End
   ↓
Day Summary
   ↓
Progression
```

---

## 👨‍💻 My Contributions

My work on the project includes both technical implementation and system design.

### Unity Development

- Implemented an interactive camera-focus and target-navigation system
- Built and integrated UI flows across multiple scenes
- Worked on inventory, shop, save/load, and progression integration
- Debugged scene, UI, and system integration issues
- Worked with Unity scenes, prefabs, serialized references, and runtime state

### System & Architecture Design

- Contributed to the overall system use cases
- Designed game data structures
- Contributed to class and activity modeling
- Designed module architecture and detailed module structure
- Reviewed consistency between different system diagrams and modules

### Team Collaboration

- Collaborated in a three-person development team
- Used Git and GitHub with feature branches
- Integrated Unity scenes, scripts, assets, and team changes
- Resolved merge and integration issues during development

---

## 🏗️ Architecture

The project separates persistent game state, application-level systems,
gameplay logic, and presentation responsibilities.

A simplified structure is:

```text
Game Root
│
├── Game Session / Game State
│
├── Save System
│
├── Game State Machine
│
├── Managers / Services
│   ├── Shop
│   ├── Inventory
│   ├── Items
│   ├── Run Flow
│   └── Progression
│
├── Presenters / Views
│   ├── Shop UI
│   ├── Inventory UI
│   ├── HUD
│   └── Summary UI
│
└── Gameplay Components
    ├── Camera Interaction
    ├── World Objects
    └── Scene-specific Behaviour
```

The project uses Unity's component-based workflow for runtime gameplay while
keeping persistent data and larger game systems separated from individual
GameObjects.

---

## 🛠️ Technologies

- Unity
- C#
- Git & GitHub
- Visual Studio
- JSON / Unity Resources
- Unity UI
- TextMesh Pro

---

## 📸 Screenshots

![Demo App](SourceReadme/Anh_1.png)

![Demo App2](SourceReadme/Anh_2.png)

Gameplay Demo: <https://youtu.be/2QlNGfo-G3U>

---

## 🚧 Development Status

CureShot is currently under active development.

Some gameplay systems and content are still being implemented or integrated,
and the current repository represents a work-in-progress university game
project.

---

## 👥 Team

Developed by a team of three Information Technology students at the
University of Transport Technology.

---

## 📄 Academic Project

This project was developed as part of a university project focused on studying
Unity and applying it to the development of a complete 2D game.

---

## 📌 Future Improvements

- Complete the full survival-run gameplay loop
- Expand anomaly and puzzle content
- Improve game balancing and progression
- Complete mission and enemy systems
- Add additional visual and audio feedback
- Improve performance and polish
- Add gameplay video and playable build
