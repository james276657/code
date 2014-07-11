using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Interface spec for TimeModel observers
namespace Instrument_Panel
{
    public interface ITimeObserver
    {
        void TimeUpdate(DateTime o);
    }

}
