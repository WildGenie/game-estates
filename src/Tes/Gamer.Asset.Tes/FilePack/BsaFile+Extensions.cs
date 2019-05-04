﻿using System;
using System.Linq;

namespace Gamer.Asset.Tes.FilePack
{
    partial class BsaFile
    {
        void TestContainsFile()
        {
            foreach (var file in _files)
            {
                Console.WriteLine($"{file.Path} {file.PathHash}");
                if (!ContainsFile(file.Path))
                    throw new FormatException("Hash Invalid");
                else if (!_filesByHash[HashFilePath(file.Path)].Any(x => x.Path == file.Path))
                    throw new FormatException("Hash Invalid");
            }
        }

        void TestLoadFileData()
        {
            foreach (var file in _files)
            {
                Console.WriteLine(file.Path);
                LoadFileData(file);
            }
        }
    }
}