using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Xoroshiro128Plus
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Xoroshiro128PlusMutable : IEnumerable<long>
    {
        private ulong state0, state1;

        public bool Initialized => this.state0 != default || this.state1 != default;

        public Xoroshiro128PlusMutable(long seed) : this((ulong)seed, (ulong)seed ^ ulong.MaxValue)
        { }

        public Xoroshiro128PlusMutable(ulong state0, ulong state1)
        {
            this.state0 = state0;
            this.state1 = state1;
        }

        public long Next()
        {
            var s0 = this.state0;
            var s1 = this.state1 ^ s0;

            this.state0 = (s0 << 55 | s0 >> 64 - 55) ^ s1 ^ s1 << 14;
            this.state1 = (s1 << 36 | s1 >> 64 - 36);

            return (long)(this.state0 + this.state1);
        }

        public int NextInt()
        {
            var value = (ulong)this.Next();
            return (int)(value ^ value >> 32);
        }

        public IEnumerator<long> GetEnumerator()
        {
            var random = this; // local copy
            while (true)
                yield return random.Next();
        }

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}
