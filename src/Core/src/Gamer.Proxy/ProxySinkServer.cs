﻿using Gamer.Proxy.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gamer.Proxy
{
    public class ProxySinkServer : ProxySink
    {
        readonly Func<object> _func;

        public ProxySinkServer(Func<object> func) => _func = func;

        // ASSET
        public override HashSet<string> GetContainsSet(Func<HashSet<string>> action)
        {
            var res = (HttpResponse)_func();
            var r = action(); res.ContentBytes = ProxyUtils.ToBytes(true, r); return r;
        }
        public override bool ContainsFile(string filePath, Func<bool> action) => throw new NotSupportedException();
        public async override Task<byte[]> LoadFileDataAsync(string filePath, Func<Task<byte[]>> action)
        {
            var res = (HttpResponse)_func();
            var r = await action(); res.ContentBytes = ProxyUtils.ToBytes(true, r); return r;
        }

        // DATA
        public override byte[] GetDataContains(Func<byte[]> action)
        {
            var res = (HttpResponse)_func();
            var r = action(); res.ContentBytes = ProxyUtils.ToBytes(true, r); return r;
        }
        public async override Task<byte[]> LoadDataLabelAsync(string filePath, Func<Task<byte[]>> action)
        {
            var res = (HttpResponse)_func();
            var r = await action(); res.ContentBytes = ProxyUtils.ToBytes(true, r); return r;
        }
    }
}
