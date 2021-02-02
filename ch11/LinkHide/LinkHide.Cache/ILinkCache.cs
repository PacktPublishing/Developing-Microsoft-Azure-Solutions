using System;

namespace LinkHide.Cache
{
    public interface ILinkCache
    {
        Link GetLink(Token token);
        Link GetLink(string token);
    }
}
