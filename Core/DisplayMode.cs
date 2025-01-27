using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Chess.Core;
internal class DisplayMode(int width, int height, bool fullscreen, bool borderless)
{
    internal static int maxWidth = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.Last().Width;
    internal static int maxHeight = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.Last().Height;
    internal int Width { get; private set; } = width;
    internal int Height { get; private set; } = height;
    internal bool Fullscreen { get; private set; } = fullscreen;
    internal bool Borderless { get; private set; } = borderless;
    internal void SetFullscreen()
    {
        Fullscreen = true;
        Borderless = true;
    }
    internal void SetWindowed()
    {
        Fullscreen = false;
        Borderless = false;
    }
    internal void SetBorderless()
    {
        Fullscreen = false;
        Borderless = true;
        Width = maxWidth;
        Height = maxHeight;
    }
    internal void SetResolution(int width, int height)
    {
        Width = width;
        Height = height;
    }
}