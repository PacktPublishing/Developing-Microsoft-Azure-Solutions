using System;
using System.Collections.Generic;
using System.Text;

namespace LinkHide.Cache
{
    public class SQLServerLinkCache : ILinkCache
    {
        public virtual Link GetLink(Token token)
        {
            throw new NotImplementedException();
        }

        public virtual Link GetLink(string token)
        {
            throw new NotImplementedException();
        }
    }
}
