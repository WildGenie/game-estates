﻿using Gamer.Core;
using Gamer.Proxy;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gamer.Estate.Tes
{
    public static class TesExtensions
    {
        public static TesGame ToTesGame(this Uri uri, Func<object> func, out ProxySink proxySink, out string[] filePaths) => ProxySink.ToGame<TesGame>(uri, func, out proxySink, out filePaths, "Tes", (path, game) => TesFileManager.GetFilePaths(Path.GetExtension(path) == ".bsa" || Path.GetExtension(path) == ".ba2", path, game));

        public static Task<IDataPack> GetTesDataPackAsync(this Uri uri, Func<object> func = null)
        {
            var game = uri.ToTesGame(func, out var client, out var filePaths);
            return Task.FromResult((IDataPack)new TesDataPack(client, filePaths.Single(), game));
        }
    }
}