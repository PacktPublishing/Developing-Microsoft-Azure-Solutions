using System;
using System.Collections.Generic;
using System.Text;

namespace LinkHide.Cache
{
    public class RowVersionLinkCache : SQLServerLinkCache
    {
        public override Link GetLink(Token token)
        {
            throw new NotImplementedException();
        }

        public override Link GetLink(string token)
        {
            throw new NotImplementedException();
        }
    }
}
