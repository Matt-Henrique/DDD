using System.Text;
using Invoisys.Domain.Entity;
using Invoisys.Domain.Interface.Repository;
using Invoisys.Infrastructure.Data.Context;

namespace Invoisys.Infrastructure.Data.Repository
{
    public class ModelRepository : Repository<Model>, IModelRepository
    {
        public ModelRepository(InvoisysContext context) : base(context)
        {
            Context = context;
        }
        public void DeleteAll()
        {
            var strSql = new StringBuilder();
            strSql.Append("DELETE [dbo].[Model]");
            Context.Database.ExecuteSqlCommand(strSql.ToString());
        }
    }
}