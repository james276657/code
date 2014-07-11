using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//Interface spec for TimeModel subject
namespace Instrument_Panel
{
    public interface ITimeSubject
        {
            void RegisterObserver(ITimeObserver o);
            void RemoveObserver(ITimeObserver o);
            void NotifyObserver(DateTime o);
        }

}
