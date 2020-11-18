using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace clearpixels.Helpers.database
{
    public class TransactionHelper
    {
        public static TransactionScope CreateTransactionScope(TransactionScopeOption option)
        {
            return new TransactionScope(option);
        }
    }
}
