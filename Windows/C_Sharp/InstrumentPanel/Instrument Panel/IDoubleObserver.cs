using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Interface spec for Double Model observers
namespace Instrument_Panel
{
    public interface IDoubleObserver
    {
        void DoubleUpdate(double o);
    }
    public interface IRPMDoubleObserver
    {
        void RPMUpdate(double o);
    }

    public interface IMPHDoubleObserver
    {
        void MPHUpdate(double o);
    }
    public interface IOILDoubleObserver
    {
        void OILUpdate(double o);
    }
    public interface ITEMPDoubleObserver
    {
        void TEMPUpdate(double o);
    }
    public interface IBATDoubleObserver
    {
        void BATUpdate(double o);
    }
    public interface IGEARDoubleObserver
    {
        void GEARUpdate(double o);
    }
    public interface IFUELDoubleObserver
    {
        void FUELUpdate(double o);
    }
    public interface IODODoubleObserver
    {
        void ODOUpdate(double o);
    }
    public interface ITRIPDoubleObserver
    {
        void TRIPUpdate(double o);
    }
    public interface ISTARTDoubleObserver
    {
        void STARTUpdate(double o);
    }
}
