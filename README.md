# Balloony (Formerly BathBomb)

Balloony is a multiplayer PvP party game inspired by classic Bomberman gameplay. Players battle in restroom-themed arenas using bath bombs, collecting power-ups and avoiding explosions to become the last player standing.

---

# Features

- Multiplayer local PvP gameplay
- Grid-based Bomberman-inspired mechanics
- Bath bomb placement and explosion system
- Power-ups (speed, bomb range, max bombs)
- AI opponents
- Match win detection
- Cartoon restroom-themed arenas
- Player stun/elimination mechanics

---

# Technologies Used

- Unity 6 (6000.3.7f1)
- C#
- GitHub
- Unity Tilemap System

---

# GitHub Repository

Repository Link:

https://github.com/catlopezruiz/Balloony.git

---

# How To Play

## Objective

Eliminate all other players using bath bombs and survive until the end of the match.

## Gameplay

- Move around the arena
- Place bath bombs
- Avoid explosions
- Break destructible blocks
- Collect power-ups
- Trap opponents

The last remaining player wins.

---

# Controls

## Player 1

| Action | Key |
|---|---|
| Move Up | W |
| Move Down | S |
| Move Left | A |
| Move Right | D |
| Place Bath Bomb | Space |

## Player 2

| Action | Key |
|---|---|
| Move Up | Up Arrow |
| Move Down | Down Arrow |
| Move Left | Left Arrow |
| Move Right | Right Arrow |
| Place Bath Bomb | Right Shift |

---

# How To Run The Game

## Option 1 — Play Using Build (Recommended)

1. Download `BalloonyBuild.zip`
2. Extract the ZIP folder
3. Open the extracted folder
4. Double-click `Balloony.exe`

No Unity installation is required.

---

## Option 2 — Open In Unity

### Requirements

- Unity Hub
- Unity Editor 6000.3.7f1

### Steps

1. Clone the repository:
2. Open Unity Hub
3. Click: Add Project From Disk
4. Select the cloned Balloony folder
5. Open the project using Unity 6000.3.7f1
6. Open: assets/Scenes/MainGameScene.unity
7. Press the Play button in Unity


#Build Instructions

To create a playable executable:

1. Open Unity
2. Go to: File → Build Profiles
3. Select: Windows
4. Add the main scene to the build
5. Click: Build
6. Choose an output folder
Unity will generate:

Balloony.exe
Balloony_Data
runtime files


# Project Structure
Assets/
├── Scripts/
├── Prefabs/
├── Scenes/
├── Art/
├── Animations/
├── Audio/
└── Tilemaps/


# Known Issues
- Multiplayer networking systems are still experimental
- Some AI behaviors may occasionally become stuck
- Minor balancing adjustments may still be needed


# Future Improvements
- Online multiplayer support
- Additional maps
- More power-ups
- Cosmetic customization
- Ranked matchmaking
- Expanded AI behaviors


Team Members

Catherine Lopez-Ruiz

Zion Hsieh

Christopher Mendoza


```bash
git clone https://github.com/catlopezruiz/Balloony.git
