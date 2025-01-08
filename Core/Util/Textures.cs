using Chess.Core.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace Chess.Core.Util;
internal static class Textures
{
    private static readonly Dictionary<string, Texture2D> textures = [];
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
    internal static Texture2D Get(string name) => textures[name];
    internal static Texture2D Get(Piece piece) => Get((piece.IsWhite ? "w" : "b") + piece.GetType().ToString().Split('.')[^1]);
}