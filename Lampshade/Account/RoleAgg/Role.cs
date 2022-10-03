using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountManagement.Domain.AccountAgg;

namespace AccountManagement.Domain.RoleAgg
{
    public class Role
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public List<Account> Accounts { get; private set; }

        public Role(string name)
        {
            Name = name;
        }

        public void Edit(string name)
        {
            Name = name;
        }
    }
}
