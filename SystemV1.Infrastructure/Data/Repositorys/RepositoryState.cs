using Dapper;
using System;
using System.Collections.Generic;
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
                        select
                            id,
                            name
                        from state
                        where name like '%{name}%'
                            and isactive";
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
                                {"\""}IsActive{"\""}
                         FROM {"\""}State{"\""}
                         WHERE {"\""}id{"\""} = {id}
                            and {"\""}IsActive{"\""}";
            return await _sqlContext.Connection.QuerySingleAsync<State>(sql);
        }
    }
}