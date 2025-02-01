# Chess Game

This is a chess game developed using .NET 9 and MonoGame framework. The game features a graphical user interface built with GeonBit.UI and supports different game modes including Hotseat, Bot, and Online.

## Features

- **Graphical User Interface**: Built using GeonBit.UI.
- **Game Modes**: Supports Hotseat, Bot, and Online modes.
- **Chess Mechanics**: Implements standard chess rules including piece movements, captures, castling, and check/checkmate detection.
- **Extensible Architecture**: Designed to be easily extendable for additional features and improvements.

## Requirements

- .NET 9
- Visual Studio 2022
- MonoGame Framework
- GeonBit.UI

## Getting Started

### Prerequisites

Ensure you have the following installed:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

### Installation
          
1. Clone the repository:
    ```console
    git clone https://github.com/your-repo/chess-game.git
    ```
2. Open the solution in Visual Studio 2022.

3. Restore the NuGet packages:
    ```console
    dotnet restore
    ```
    
### Running the Game

1. Set the `Chess` project as the startup project.
2. Build and run the project using Visual Studio.

## Project Structure

- **Chess.csproj**: Project file containing dependencies and build configurations.
- **Core**: Contains the core game logic and engine.
  - **Chess.cs**: Main game class handling initialization, content loading, updating, and drawing.
  - **ChessGame.cs**: Abstract class representing a chess game with different game modes.
  - **Engine**: Contains the game engine components such as the board and pieces.
  - **Util**: Utility classes for handling textures, moves, and other helper functions.
- **ChessEngines**: Contains the integration with various chess engines.

## Adding a Specific Chess Engine

To add a specific chess engine to the game, follow these steps:

1. Create a new folder under the `ChessEngines` directory for the specific engine.
2. Implement the necessary classes and methods to interface with the engine. This typically involves creating a class that can communicate with the engine's API or executable.
3. Update the game logic to utilize the new engine. This may involve modifying the `ChessGame` class or creating a new subclass that uses the engine for move generation and evaluation.
4. Ensure that any dependencies required by the engine are included in the project and properly configured.

## Dependencies

- [MonoGame.Framework.WindowsDX](https://www.nuget.org/packages/MonoGame.Framework.WindowsDX/)
- [MonoGame.Extended](https://www.nuget.org/packages/MonoGame.Extended/)
- [GeonBit.UI](https://www.nuget.org/packages/GeonBit.UI/)

## Contributing

Contributions are welcome! Please fork the repository and submit pull requests for any improvements or bug fixes.

## License

This project is licensed under the MIT License.

## Acknowledgements

- [MonoGame](https://www.monogame.net/)
- [GeonBit.UI](https://github.com/RonenNess/GeonBit.UI)
