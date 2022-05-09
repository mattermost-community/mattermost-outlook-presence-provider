using System.Collections.Generic;
using System.Linq;
using UCCollaborationLib;
using System.Runtime.InteropServices;

namespace MMPresenceProviderImpl
{
    [ComVisible(true)]
    public class IMClientContactInformationDictionary : ContactInformationDictionary
    {
        private Dictionary<ContactInformationType, object> ContactInformationDictionary;

        public IMClientContactInformationDictionary()
        {
            ContactInformationDictionary = new Dictionary<ContactInformationType, object>();
        }

        public void Add(ContactInformationType _key, object _value)
        {
            ContactInformationDictionary.Add(_key, _value);
        }
        public bool TryGetValue(ContactInformationType _key, out object _value)
        {
            return ContactInformationDictionary.TryGetValue(_key, out _value);
        }

        public ContactInformationType GetKeyAt(int _index)
        {
            return ContactInformationDictionary.ElementAt(_index).Key;
        }

        public dynamic GetValueAt(int _index)
        {
            return ContactInformationDictionary.ElementAt(_index).Value;
        }

        public bool ContainsKey(ContactInformationType _key)
        {
            return ContactInformationDictionary.ContainsKey(_key);
        }

        public object[] Values => ContactInformationDictionary.Values.ToArray();

        public int Count => ContactInformationDictionary.Count;

        public dynamic this[ContactInformationType _key] => ContactInformationDictionary[_key];

        public ContactInformationType[] Keys => ContactInformationDictionary.Keys.ToArray();
    }
}
