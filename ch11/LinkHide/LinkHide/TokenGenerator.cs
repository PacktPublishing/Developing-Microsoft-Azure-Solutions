using System;
using System.Collections.Generic;
using System.Text;
using static Pineapple.Common.Preconditions;

namespace LinkHide
{
    public interface ITokenGenerator
    {
        Token Generate(int length = 5);
    }

    public class DefaultTokenGenerator : ITokenGenerator
    {
        private const string _c = "ABCDEFGHIJKLMNOPQRSTUVWXYZ012345789abcdefghijklmnopqrstuvwxyz0123456789";

        private static readonly Random _r = new Random();
        private static readonly int _l = 0;
        private static readonly int _u = _c.Length - 1;

        public DefaultTokenGenerator()
        {
        }

        public Token Generate(int length = 5)
        {
            CheckIsNotNull(nameof(length), length);

            var sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                var index = _r.Next(_l, _u);
                sb.Append(_c[index]);
            }

            return new Token(sb.ToString());
        }
    }
}
