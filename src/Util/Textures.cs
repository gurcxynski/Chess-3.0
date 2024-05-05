namespace Chess.Util;

using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

static class Textures
{
    private static readonly Dictionary<string, Texture2D> textures = new();
    public static void Load(Game game)
    {
        var contentManager = game.Content;
        var textureFiles = Directory.EnumerateFiles(contentManager.RootDirectory + "/Textures", "*.xnb");
    
        foreach (var textureFile in textureFiles)
        {
            var textureName = Path.GetFileNameWithoutExtension(textureFile);
            var texture = contentManager.Load<Texture2D>(textureName);
            textures.Add(textureName, texture);
        }
    }
    public static Texture2D Get(string name)
    {
        return textures[name];
    }
}