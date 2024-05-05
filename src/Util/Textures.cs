using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chess.Util;
internal class Textures
{
    private readonly Dictionary<string, Texture2D> textures = new();
    internal void Load(Game game)
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
    internal Texture2D Get(string name)
    {
        return textures[name];
    }
}