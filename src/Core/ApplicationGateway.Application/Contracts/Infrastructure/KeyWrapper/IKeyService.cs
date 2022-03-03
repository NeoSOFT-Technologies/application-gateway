﻿using ApplicationGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Contracts.Infrastructure.KeyWrapper
{
    public interface IKeyService
    {
        Task<List<string>> GetAllKeysAsync();
        Task<Key> GetKeyAsync(string keyId);
        Task<Key> CreateKeyAsync(Key key);
        Task<Key> UpdateKeyAsync(Key key);
        Task DeleteKeyAsync(string keyId);
    }
}
