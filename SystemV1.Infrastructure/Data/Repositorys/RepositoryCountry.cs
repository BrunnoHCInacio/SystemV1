using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Repositorys
{
    public class RepositoryCountry : Repository<Country>, IRepositoryCountry
    {
        private readonly SqlContext _sqlContext;

        public RepositoryCountry(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<IEnumerable<Country>> GetByNameAsync(string name)
        {
            var sql = $@"
                        SELECT id, name
                        FROM country
                        WHERE name LIKE '%{name}%'
                            AND isactive";
            return await _sqlContext.Connection.QueryAsync<Country>(sql);
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            var sql = @$"SELECT {"\""}Id{"\""},
                                {"\""}Name{"\""}
                         FROM {"\""}Country{"\""}
                         WHERE {"\""}IsActive{"\""}
                         ORDER BY {"\""}Id{"\""}
                         LIMIT {pageSize}
                         OFFSET {skip}";

            return await _sqlContext.Connection.QueryAsync<Country>(sql);
        }

        public async Task<Country> GetCountryByIdAsync(Guid id)
        {
            var sql = $@"SELECT {"\""}Id{"\""},
                                {"\""}Name{"\""},
                                {"\""}DateRegister{"\""},
                                {"\""}DateChange{"\""},
                                {"\""}IdUserRegister{"\""},
                                {"\""}IdUserChange{"\""},
                                {"\""}IsActive{"\""}
                         FROM {"\""}Country{"\""}
                         WHERE {"\""}Id{"\""} = '{id}'
                            and {"\""}IsActive{"\""}";
            return await _sqlContext.Connection.QuerySingleOrDefaultAsync<Country>(sql);
        }
    }
}