using System;
using System.Xml.Linq;

namespace DoubleMa.INI {

    internal interface IINILoader {

        XElement GetSectionOrCreate(XSectionWithComment x);

        string GetValueOrCreate<T>(XKeyWithComment<T> x) where T : IComparable, IConvertible;

        void Save();
    }
}