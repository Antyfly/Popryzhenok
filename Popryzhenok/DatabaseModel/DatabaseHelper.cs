using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popryzhenok.DatabaseModel
{
    public static class DatabaseHelper
    {
        private static PopryzhenokEntities _Entities = new PopryzhenokEntities();

        public static List<Agent> GetAgents()
        {
            return _Entities.Agent.ToList();
        }

        public static List<AgentType> GetAgentsType()
        {
            return _Entities.AgentType.ToList();
        }
        public static List<Product> GetProduct()
        {
            return _Entities.Product.ToList();
        }

        public static void SaveChange()
        {
            _Entities.SaveChanges();
        }

    }
}
