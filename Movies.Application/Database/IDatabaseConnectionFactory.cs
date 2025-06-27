using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Database
{
    public interface IDatabaseConnectionFactory
    {
        Task<IDbConnection> CreateConnection();
    }
}
