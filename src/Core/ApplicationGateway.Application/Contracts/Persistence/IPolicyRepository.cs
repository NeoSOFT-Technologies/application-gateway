﻿using ApplicationGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Contracts.Persistence
{
    public interface IPolicyRepository:IAsyncRepository<Policy>
    {
        Task<IReadOnlyList<Policy>> GetSearchedResponseAsync(int page, int size, string col, string value);
    }
}
