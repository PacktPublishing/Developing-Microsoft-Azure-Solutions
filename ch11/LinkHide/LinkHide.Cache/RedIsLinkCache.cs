using System;
using System.Collections.Generic;
using System.Text;

namespace LinkHide.Cache
{
    public class RedisLinkCache : ILinkCache
    {
        public Link GetLink(Token token)
        {
            throw new NotImplementedException();
        }

        public Link GetLink(string token)
        {
            throw new NotImplementedException();
        }
    }
}
