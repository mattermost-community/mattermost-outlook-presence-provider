using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCCollaborationLib;

namespace OutlookPresenceProvider
{
    public class IMClientContactInformationDictionary : ContactInformationDictionary
    {
        private Dictionary<ContactInformationType, object> ContactInformationDictionary;

        public IMClientContactInformationDictionary()
        {
            this.ContactInformationDictionary = new Dictionary<ContactInformationType, object>();
        }

        public void Add(ContactInformationType _key, object _value)
        {
            this.ContactInformationDictionary.Add(_key, _value);
        }
        public bool TryGetValue(ContactInformationType _key, out object _value)
        {
            return this.ContactInformationDictionary.TryGetValue(_key, out _value);
        }

        public ContactInformationType GetKeyAt(int _index)
        {
            return this.ContactInformationDictionary.ElementAt(_index).Key;
        }

        public dynamic GetValueAt(int _index)
        {
            return this.ContactInformationDictionary.ElementAt(_index).Value;
        }

        public bool ContainsKey(ContactInformationType _key)
        {
            return this.ContactInformationDictionary.ContainsKey(_key);
        }

        public object[] Values => this.ContactInformationDictionary.Values.ToArray();

        public int Count => this.ContactInformationDictionary.Count;

        public dynamic this[ContactInformationType _key] => this.ContactInformationDictionary[_key];

        public ContactInformationType[] Keys => this.ContactInformationDictionary.Keys.ToArray();
    }
}
