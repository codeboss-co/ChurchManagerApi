using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ChurchManager.Infrastructure.Persistence.Extensions
{
    public class CustomTypeSqlQuery<T> where T : class
    {
        private readonly IMapper _mapper;

        // Properties
        public DatabaseFacade DatabaseFacade { get; set; }
        public string SqlQuery { get; set; }
        public DbParameter[] SqlParameters { get; set; }

        public CustomTypeSqlQuery()
        {
            _mapper = new MapperConfiguration(cfg => {
                cfg.AddDataReaderMapping();
                cfg.CreateMap<IDataRecord, T>();
            }).CreateMapper();
        }

        #region Methods

        public async Task<IList<T>> ToListAsync(CancellationToken ct = default)
        {
            IList<T> results = new List<T>();
            var conn = DatabaseFacade.GetDbConnection();
            try
            {
                await conn.OpenAsync(ct);
                using(var command = conn.CreateCommand())
                {
                    // Add Parameters
                    foreach(var param in SqlParameters)
                    {
                        var p = command.CreateParameter();
                        p.ParameterName = param.ParameterName;
                        p.Value = param.Value;
                        command.Parameters.Add(p);
                    }

                    command.CommandText = SqlQuery;
                    DbDataReader reader = await command.ExecuteReaderAsync(ct);

                    if(reader.HasRows)
                        results = _mapper.Map<IDataReader, IEnumerable<T>>(reader)
                            .ToList();
                    await reader.DisposeAsync();
                }
            }
            finally
            {
                await conn.CloseAsync();
            }
            return results;
        }

        public async Task<T> FirstOrDefaultAsync(CancellationToken ct = default)
        {
            T result = null;
            var conn = DatabaseFacade.GetDbConnection();
            try
            {
                await conn.OpenAsync(ct);
                using(var command = conn.CreateCommand())
                {
                    // Add Parameters
                    foreach(var param in SqlParameters)
                    {
                        var p = command.CreateParameter();
                        p.ParameterName = param.ParameterName;
                        p.Value = param.Value;
                        command.Parameters.Add(p);
                    }

                    command.CommandText = SqlQuery;
                    DbDataReader reader = await command.ExecuteReaderAsync(ct);

                    if(reader.HasRows)
                    {
                        var results = _mapper.Map<IDataReader, IEnumerable<T>>(reader)
                            .ToList();
                        result = results.FirstOrDefault();
                    }
                    await reader.DisposeAsync();
                }
            }
            finally
            {
                await conn.CloseAsync();
            }
            return result;
        } 

        #endregion
    }
}
