using System;

namespace Wicked.Favorites
{
    public static class Instance
    {
        static Instance()
        {
            ID = Guid.NewGuid();
        }

        public static Guid ID { get; }
    }
}
