﻿using Gamer.Core;
using Gamer.Core.Format;
using Gamer.Estate.Nif.Format;
using System;
using System.IO;
using System.Threading.Tasks;
using static Gamer.Core.Debug;

namespace Gamer.Estate.Tes.FilePack
{
    partial class BsaMultiFile
    {
        public Task<Texture2DInfo> LoadTextureInfoAsync(string texturePath)
        {
            var filePath = FindTexture(texturePath);
            if (filePath != null)
            {
                var fileData = LoadFileData(filePath);
                return Task.Run(() =>
                {
                    Log(filePath);
                    var fileExtension = Path.GetExtension(filePath);
                    if (fileExtension.ToLowerInvariant() == ".dds") return DdsReader.LoadDDSTexture(new MemoryStream(fileData));
                    else throw new NotSupportedException($"Unsupported texture type: {fileExtension}");
                });
            }
            else
            {
                Log($"Could not find file \"{texturePath}\" in a BSA file.");
                return Task.FromResult<Texture2DInfo>(null);
            }
        }

        public Task<object> LoadObjectInfoAsync(string filePath)
        {
            var fileData = LoadFileData(filePath);
            return Task.Run(() =>
            {
                var file = new NiFile(Path.GetFileNameWithoutExtension(filePath));
                file.Deserialize(new BinaryFileReader(new MemoryStream(fileData)));
                return (object)file;
            });
        }

        /// <summary>
        /// Finds the actual path of a texture.
        /// </summary>
        string FindTexture(string texturePath)
        {
            var textureName = Path.GetFileNameWithoutExtension(texturePath);
            var textureNameInTexturesDir = $"textures/{textureName}";
            var filePath = $"{textureNameInTexturesDir}.dds";
            if (ContainsFile(filePath))
                return filePath;
            //filePath = textureNameInTexturesDir + ".tga";
            //if (ContainsFile(filePath))
            //    return filePath;
            var texturePathWithoutExtension = Path.GetDirectoryName(texturePath) + '/' + textureName;
            filePath = texturePathWithoutExtension + ".dds";
            if (ContainsFile(filePath))
                return filePath;
            //filePath = texturePathWithoutExtension + ".tga";
            //if (ContainsFile(filePath))
            //    return filePath;
            // Could not find the file.
            return null;
        }
    }
}