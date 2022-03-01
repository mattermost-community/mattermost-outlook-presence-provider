using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCCollaborationLib;

namespace OutlookPresenceProvider
{
    public class IMClientContactSettingDictionary : ContactSettingDictionary
    {
        private Dictionary<ContactSetting, object> ContactSettingDictionary;

        public IMClientContactSettingDictionary()
        {
            this.ContactSettingDictionary = new Dictionary<ContactSetting, object>();
        }
        public bool TryGetValue(ContactSetting _key, out object _value)
        {
            return this.ContactSettingDictionary.TryGetValue(_key, out _value);
        }

        public ContactSetting GetKeyAt(int _index)
        {
            return this.ContactSettingDictionary.ElementAt(_index).Key;
        }

        public dynamic GetValueAt(int _index)
        {
            return this.ContactSettingDictionary.ElementAt(_index).Value;
        }

        public bool ContainsKey(ContactSetting _key)
        {
            return this.ContactSettingDictionary.ContainsKey(_key);
        }

        public dynamic this[ContactSetting _key] => this.ContactSettingDictionary[_key];

        public int Count => this.ContactSettingDictionary.Count;

        public ContactSetting[] Keys => this.ContactSettingDictionary.Keys.ToArray();

        public object[] Values => this.ContactSettingDictionary.Values.ToArray();
    }

}
