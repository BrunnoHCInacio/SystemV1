using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Repositorys
{
    public class RepositoryState : Repository<State>, IRepositoryState
    {
        private readonly SqlContext _sqlContext;

        public RepositoryState(SqlContext sqlContext) : base(sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<IEnumerable<State>> GetByNameAsync(string name)
        {
            var sql = $@"
                        SELECT
                            {"\""}Id{"\""},
                            {"\""}Name{"\""}
                        FROM {"\""}State{"\""}
                        WHERE name LIKE '%{name}%'
                            AND {"\""}IsActive{"\""}";
            return await _sqlContext.Connection.QueryAsync<State>(sql);
        }

        public async Task<IEnumerable<State>> GetAllStatesAsync(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            var sql = @$"SELECT {"\""}Id{"\""},
                                {"\""}Name{"\""}
                         FROM {"\""}State{"\""}
                         WHERE {"\""}IsActive{"\""}
                         ORDER BY {"\""}Id{"\""}
                         LIMIT {pageSize}
                         OFFSET {skip}"
                         ;

            return await _sqlContext.Connection.QueryAsync<State>(sql);
        }

        public async Task<State> GetStateByIdAsync(Guid id)
        {
            var sql = $@"SELECT {"\""}Id{"\""},
                                {"\""}Name{"\""},
                                {"\""}DateRegister{"\""},
                                {"\""}DateChange{"\""},
                                {"\""}IdUserRegister{"\""},
                                {"\""}IdUserChange{"\""},
                                {"\""}CountryId{"\""},
                                {"\""}IsActive{"\""}
                         FROM {"\""}State{"\""}
                         WHERE {"\""}Id{"\""} = '{id}'
                            and {"\""}IsActive{"\""}";

            var r = await _sqlContext.Connection.QuerySingleAsync<State>(sql);
            return r;
        }

        public async Task<State> GetStateCountryByIdAsync(Guid id)
        {
            var sql = $@"SELECT s.{"\""}Id{"\""},
                                s.{"\""}Name{"\""},
                                s.{"\""}DateRegister{"\""},
                                s.{"\""}DateChange{"\""},
                                s.{"\""}IdUserRegister{"\""},
                                s.{"\""}IdUserChange{"\""},
                                s.{"\""}CountryId{"\""},
                                s.{"\""}IsActive{"\""},
                                c.{"\""}Id{"\""},
                                c.{"\""}Name{"\""}
                         FROM {"\""}State{"\""} AS s
                            LEFT JOIN {"\""}Country{"\""} AS c
                                ON s.{"\""}CountryId{"\""} = c.{"\""}Id{"\""}
                         WHERE s.{"\""}Id{"\""} = '{id}'
                            and s.{"\""}IsActive{"\""}";

            var state = await _sqlContext.Connection.QueryAsync<State, Country, State>(sql,
                (state, country) =>
                {
                    state.SetCountry(country);
                    return state;
                },
                splitOn: "Id"
                );

            return state.FirstOrDefault();
        }
    }
}