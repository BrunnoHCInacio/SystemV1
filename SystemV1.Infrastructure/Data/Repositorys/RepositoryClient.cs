﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Repositorys
{
    public class RepositoryClient : Repository<Client>, IRepositoryClient
    {
        private readonly SqlContext _sqlContext;

        public RepositoryClient(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<IEnumerable<Client>> GetByNameAsync(string name)
        {
            var sql = $@"
                        SELECT
                            {"\""}Id{"\""},
                            {"\""}Name{"\""},
                            {"\""}Document{"\""}
                        FROM {"\""}client{"\""}
                        WHERE name LIKE '%{name}%' and {"\""}IsActive{"\""}
                        ";
            return await _sqlContext.Connection.QueryAsync<Client>(sql);
        }
    }
}