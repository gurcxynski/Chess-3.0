using System.Collections.Generic;
using System.IO;
using Chess.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess.Util;
internal static class Textures
{
    private static readonly Dictionary<string, Texture2D> textures = new();
    internal static void Load(Game game)
    {
        var contentManager = game.Content;
        var textureFiles = Directory.EnumerateFiles(contentManager.RootDirectory, "*.xnb");
    
        foreach (var textureFile in textureFiles)
        {
            var textureName = Path.GetFileNameWithoutExtension(textureFile);
            var texture = contentManager.Load<Texture2D>(textureName);
            textures.Add(textureName, texture);
        }
    }
    internal static Texture2D Get(string name)
    {
        return textures[name];
    }
    internal static Texture2D Get(Piece piece)
    {
        return Get((piece.IsWhite ? "w" : "b") + piece.GetType().ToString().Split('.')[^1]);
    }
}