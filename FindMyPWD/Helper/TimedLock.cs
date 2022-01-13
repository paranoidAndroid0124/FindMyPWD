using System;
using System.Threading;

namespace FindMyPWD.Helper
{
    public class TimedLock
    {
        private readonly object toLock;

        public TimedLock(object toLock)
        {
            this.toLock = toLock;
        }

        public LockReleaser Lock(TimeSpan timeout)
        {
            if (Monitor.TryEnter(toLock, timeout))
            {
                return new LockReleaser(toLock);
            }
            throw new TimeoutException();
        }

        public struct LockReleaser : IDisposable
        {
            private readonly object toRelease;

            public LockReleaser(object toRelease)
            {
                this.toRelease = toRelease;
            }
            public void Dispose()
            {
                Monitor.Exit(toRelease);
            }
        }
    }
}
