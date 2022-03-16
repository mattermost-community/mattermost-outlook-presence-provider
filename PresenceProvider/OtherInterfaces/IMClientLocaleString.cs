using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCCollaborationLib;
using System.Runtime.InteropServices;

namespace OutlookPresenceProvider
{
    [ComVisible(true)]
    public class IMClientLocaleString : ILocaleString
    {
        private int _localeId;
        public int LocaleId
        {
            get { return _localeId; }
            set { _localeId = value; }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
