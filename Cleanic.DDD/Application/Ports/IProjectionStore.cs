﻿using Cleanic.Core;
using System;
using System.Threading.Tasks;

namespace Cleanic.Application
{
    public interface IProjectionStore
    {
        Task<IProjection> Load(IIdentity id, Type type);
        Task Save(IProjection projection);
        Task Clear();
    }
}