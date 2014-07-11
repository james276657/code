using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Instrument_Panel
{
   
        public interface IDoubleSubject
        {
            void RegisterObserver(IDoubleObserver o);
            void RemoveObserver(IDoubleObserver o);
            void NotifyObserver(double o);
        }

        public interface IRPMDoubleSubject
        {
            void RegisterObserver(IRPMDoubleObserver o);
            void RemoveObserver(IRPMDoubleObserver o);
            void NotifyObserver(double o);
        }

        public interface IMPHDoubleSubject
        {
            void RegisterObserver(IMPHDoubleObserver o);
            void RemoveObserver(IMPHDoubleObserver o);
            void NotifyObserver(double o);
        }

        public interface IOILDoubleSubject
        {
            void RegisterObserver(IOILDoubleObserver o);
            void RemoveObserver(IOILDoubleObserver o);
            void NotifyObserver(double o);
        }
        public interface ITEMPDoubleSubject
        {
            void RegisterObserver(ITEMPDoubleObserver o);
            void RemoveObserver(ITEMPDoubleObserver o);
            void NotifyObserver(double o);
        }
        public interface IBATDoubleSubject
        {
            void RegisterObserver(IBATDoubleObserver o);
            void RemoveObserver(IBATDoubleObserver o);
            void NotifyObserver(double o);
        }

        public interface IGEARDoubleSubject
        {
            void RegisterObserver(IGEARDoubleObserver o);
            void RemoveObserver(IGEARDoubleObserver o);
            void NotifyObserver(double o);
        }

        public interface IFUELDoubleSubject
        {
            void RegisterObserver(IFUELDoubleObserver o);
            void RemoveObserver(IFUELDoubleObserver o);
            void NotifyObserver(double o);
        }

        public interface IODODoubleSubject
        {
            void RegisterObserver(IODODoubleObserver o);
            void RemoveObserver(IODODoubleObserver o);
            void NotifyObserver(double o);
        }

        public interface ITRIPDoubleSubject
        {
            void RegisterObserver(ITRIPDoubleObserver o);
            void RemoveObserver(ITRIPDoubleObserver o);
            void NotifyObserver(double o);
        }
        public interface ISTARTDoubleSubject
        {
            void RegisterObserver(ISTARTDoubleObserver o);
            void RemoveObserver(ISTARTDoubleObserver o);
            void NotifyObserver(double o);
        }
    }
