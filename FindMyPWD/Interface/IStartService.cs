using System;
using System.Collections.Generic;
using System.Text;

namespace FindMyPWD.Interface
{
    public interface IStartService
    {
        void StartForegroundServiceCompat(); //TODO: probably here to fix the scanning issue
    }
}
