using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Xoroshiro128Plus
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Xoroshiro128PlusRandom : IEnumerable<long>
    {
        private readonly ulong _state0, _state1;

        public long Value => (long)(this._state0 + this._state1);

        public bool Initialized => this._state0 != default || this._state1 != default;

        public Xoroshiro128PlusRandom(long seed) : this((ulong)seed, (ulong)seed ^ ulong.MaxValue)
        { }

        public Xoroshiro128PlusRandom(ulong state0, ulong state1)
        {
            this._state0 = state0;
            this._state1 = state1;
        }

        public Xoroshiro128PlusRandom Next()
        {
            var s0 = this._state0;
            var s1 = this._state1 ^ s0;

            return new Xoroshiro128PlusRandom(
                state0: (s0 << 55 | s0 >> 64 - 55) ^ s1 ^ s1 << 14,
                state1: (s1 << 36 | s1 >> 64 - 36));
        }

        public IEnumerator<long> GetEnumerator()
        {
            for (var random = this.Next(); true; random = random.Next())
                yield return random.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();
    }
}
