using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Ninesky.Core
{
    public class NineskyContext : DbContext
    {
        public NineskyContext() : base("DefaultConnection")
        {
            Database.SetInitializer<NineskyContext>(new CreateDatabaseIfNotExists<NineskyContext>());
        }
    }
}
