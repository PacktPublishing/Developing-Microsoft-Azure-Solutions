using System;
using static Pineapple.Common.Preconditions;

namespace LinkHide
{
    public class Token
    {
        private readonly string _token;
        private readonly int _hashcode;

        public Token(string token)
        {
            CheckIsNotNullOrWhitespace(nameof(token), token);

            _token = token;
            _hashcode = GetHashCode(_token);
        }

        private static int GetHashCode(string text)
        {
            // Consistent hashing that survives process lifetime
            unchecked
            {
                int hash = 23;
                foreach (char c in text)
                {
                    hash = hash * 31 + c;
                }
                return hash;
            }
        }

        public string Value { get { return _token; } }

        public override bool Equals(object obj)
        {
            if (!(obj is Token t))
                return false;

            var result = _token.Equals(t._token);
            return result;
        }

        public override int GetHashCode()
        {
            return _hashcode;
        }
    }
}
