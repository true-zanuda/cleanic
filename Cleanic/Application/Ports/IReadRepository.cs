﻿using Cleanic.Core;
using System;
using System.Threading.Tasks;

namespace Cleanic.Application
{
    public interface IReadRepository
    {
        Task<Projection> Load(Type type, String id);
        Task Save(Projection projection);
    }
}