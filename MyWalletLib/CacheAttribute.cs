using System;

namespace MyWalletLib
{
    public class CacheAttribute : Attribute
    {
        public int Duration { get; set; }
    }
}