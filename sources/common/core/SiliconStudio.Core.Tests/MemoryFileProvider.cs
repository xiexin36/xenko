﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SiliconStudio.Core.IO
{
    public class MemoryFileProvider : VirtualFileProviderBase
    {
        private Dictionary<string, FileInfo> files = new Dictionary<string, FileInfo>();

        public MemoryFileProvider(string rootPath) : base(rootPath)
        {
        }

        public override Stream OpenStream(string url, VirtualFileMode mode, VirtualFileAccess access, VirtualFileShare share = VirtualFileShare.Read, StreamFlags streamFlags = StreamFlags.None)
        {
            if (share != VirtualFileShare.Read)
                throw new NotImplementedException();

            lock (files)
            {
                FileInfo fileInfo;
                bool exists = files.TryGetValue(url, out fileInfo);
                bool write = access != VirtualFileAccess.Read;

                switch (mode)
                {
                    case VirtualFileMode.CreateNew:
                        if (exists)
                            throw new IOException("File already exists.");
                        files.Add(url, fileInfo = new FileInfo());
                        return new MemoryFileStream(this, fileInfo, write);
                    case VirtualFileMode.Create:
                        files.Remove(url);
                        files.Add(url, fileInfo = new FileInfo());
                        return new MemoryFileStream(this, fileInfo, write);
                    case VirtualFileMode.Truncate:
                        if (!exists)
                            throw new IOException("File doesn't exists.");
                        files.Remove(url);
                        return new MemoryStream();
                    case VirtualFileMode.Open:
                        if (!exists)
                            throw new FileNotFoundException();
                        if (write)
                            throw new NotImplementedException();
                        return new MemoryFileStream(this, fileInfo, false, fileInfo.Data);
                    case VirtualFileMode.OpenOrCreate:
                        throw new NotImplementedException();
                }
            }

            return null;
        }

        private class FileInfo
        {
            public byte[] Data;
            public int Streams;
        }

        private class MemoryFileStream : MemoryStream
        {
            private readonly MemoryFileProvider provider;
            private readonly FileInfo fileInfo;

            public MemoryFileStream(MemoryFileProvider provider, FileInfo fileInfo, bool write)
            {
                this.provider = provider;
                this.fileInfo = fileInfo;
                Initialize(fileInfo, write);
            }

            public MemoryFileStream(MemoryFileProvider provider, FileInfo fileInfo, bool write, byte[] data)
                : base(data)
            {
                this.provider = provider;
                this.fileInfo = fileInfo;
                Initialize(fileInfo, write);
            }

            private static void Initialize(FileInfo fileInfo, bool write)
            {
                if (Interlocked.Increment(ref fileInfo.Streams) > 1 && write)
                    throw new InvalidOperationException();
            }

            protected override void Dispose(bool disposing)
            {
                lock (provider.files)
                {
                    fileInfo.Data = ToArray();
                    Interlocked.Decrement(ref fileInfo.Streams);
                }
                base.Dispose(disposing);
            }
        }
    }
}