#region Copyright Notice
//
// COPYRIGHT 2020-2021, MILL5, LLC
//
// This software is owned by MILL5, LLC. You may use this software for your own use
// as you see fit.
//
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using static Pineapple.Common.Preconditions;

namespace LinkHide
{
    public class Link
    {
        public Link(Token token, LinkType linkType, string link)
        {
            CheckIsNotNull(nameof(token), token);
            CheckIsNotNullOrWhitespace(nameof(link), link);

            Token = token;
            LinkType = linkType;
            Value = link;
        }

        public Token Token { get; }

        public LinkType LinkType { get; }

        public string Value { get; }
    }
}
