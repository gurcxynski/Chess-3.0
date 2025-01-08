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
    git clone https://github.com/your-repo/chess-game.git

2. Open the solution in Visual Studio 2022.

3. Restore the NuGet packages:
    dotnet restore

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
