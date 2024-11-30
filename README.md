# CS 3500 – Software Practices: Snake Game & Spreadsheet Applications

## Overview

Welcome to our **CS 3500 Software Practices** repository! This project showcases two major applications:

1. **Spreadsheet Application**: A powerful tool for managing and manipulating data in an interactive spreadsheet interface.
2. **Snake Game**: A fun, multiplayer game with a modern twist on the classic Snake mechanics.

Both projects demonstrate thoughtful design, core functionality, and enhanced features to improve user experience and performance.

---

## 📊 **Spreadsheet Application**

### About the Spreadsheet Application

The **Spreadsheet Application** is a tool designed to facilitate data management and manipulation through an intuitive interface. Below is a guide to using the application.

### Core Functionality

- **Content Field Setting**: Input data into a content field corresponding to each cell in the spreadsheet, enabling dynamic cell value editing.
- **Cell Value and Name Management**: Assign a name (e.g., A1, B1) and value (e.g., 10, "Hello") to each cell, making referencing and retrieval easier.
- **Save and Load Functionality**: Save your spreadsheet to a file and reload it later, ensuring data is preserved between sessions.

### Additional Features

- **Enter Key Functionality**: Pressing the "Enter" key saves the cell value and moves focus to the next cell below for faster data entry.
- **Color Coding**: Cells are color-coded based on their content:
  - **Formula**: Yellow
  - **Double Values**: Green
  - **String Values**: Red
- **Dark Mode**: A dark mode feature for a comfortable viewing experience in low-light environments.
- **Clear Cell Button**: Clears the content of the currently selected cell.
- **Clear All Cells Button**: Resets the spreadsheet by clearing all cells.

---

## 🐍 **Snake Game**

### Welcome to Our Snake Game!

Dive into the **Snake Game**, where you control a snake, collect powerups, and compete for the highest score!

### Gameplay Overview

The game revolves around classic snake mechanics with several modern twists:
- **Control your snake** to slither around and collect powerups to grow longer.
- **Snakes cannot pass through walls**, resulting in game over if they try.
- **Edge-of-the-world behavior** is undefined unless surrounded by walls.
- **Powerups**: Collect powerups to increase your snake's length by a fixed amount.
- **Crashing**: Crashing into walls or other snakes results in respawning with a shorter snake after a delay.

### Game Design

- **Visual Design**:
  - Snakes are rendered with colorful, smooth lines (10px stroke width).
  - Powerups are bright red squares (16x16px) with yellow borders.
  - Walls are solid blocks (50x50px) that create obstacles.

- **Game World**:
  - The game utilizes two coordinate spaces:
    - **World-Space**: Centered at (0, 0) with coordinates extending from -worldSize / 2 to +worldSize / 2.
    - **Image-Space**: The screen's top-left corner is (0, 0), dynamically adjusted to keep the snake centered.

### Architecture

The game is structured using three main components:

1. **Model**: Tracks all in-game entities, including snakes, walls, powerups, and world size.
2. **Controller**: Handles server communication, player inputs, and updates to the model.
3. **View**: Renders the game world using Blazor.Extensions.Canvas for a smooth and engaging experience.

### Key Features

- **Name Customization**: Players can customize their snake's name.
- **Server Connection**: Connect to the game via IP address or hostname and select a connection port.
- **Dynamic Rendering**: Snakes, powerups, walls, and player scores are rendered in real-time.
- **Player Customization**: First 8 players have unique colors, with options for additional players to reuse colors.
- **Crash Animations**: Snakes flash red briefly when crashing.
- **Powerup Effects**: Powerups provide satisfying visual feedback when collected.
- **Zoom**: Use the 'm' and 'n' keys to zoom in and out of the game world.
- **Disconnect Button**: A dedicated button allows players to disconnect (reconnection is not yet implemented).

### Performance and Design Considerations

- **High Performance**: Optimized for smooth gameplay, even at 50+ FPS.
- **Polished Animations**: Including smooth score updates, crash effects, and zoom animations.
- **User-Friendly Design**: Thoughtful user experience with seamless interactions and feedback.

---


## 📄 **License**

This project is licensed under the MIT License – see the [LICENSE](LICENSE) file for details. Please do not copy code, it counts under Academic Misconduct

---

