using System.Collections.Generic;
using System.Linq;
using UCCollaborationLib;
using System.Runtime.InteropServices;

namespace OutlookPresenceProvider
{
    [ComVisible(true)]
    public class IMClientContactSettingDictionary : ContactSettingDictionary
    {
        private Dictionary<ContactSetting, object> contactSettingDictionary;

        public IMClientContactSettingDictionary()
        {
            contactSettingDictionary = new Dictionary<ContactSetting, object>();
        }
        public bool TryGetValue(ContactSetting _key, out object _value)
        {
            return contactSettingDictionary.TryGetValue(_key, out _value);
        }

        public ContactSetting GetKeyAt(int _index)
        {
            return contactSettingDictionary.ElementAt(_index).Key;
        }

        public dynamic GetValueAt(int _index)
        {
            return contactSettingDictionary.ElementAt(_index).Value;
        }

        public bool ContainsKey(ContactSetting _key)
        {
            return contactSettingDictionary.ContainsKey(_key);
        }

        public dynamic this[ContactSetting _key] => contactSettingDictionary[_key];

        public int Count => contactSettingDictionary.Count;

        public ContactSetting[] Keys => contactSettingDictionary.Keys.ToArray();

        public object[] Values => contactSettingDictionary.Values.ToArray();
    }

}
