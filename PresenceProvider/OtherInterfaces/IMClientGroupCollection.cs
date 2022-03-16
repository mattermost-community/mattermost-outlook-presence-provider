using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCCollaborationLib;

namespace OutlookPresenceProvider
{
    public class IMClientGroupCollection : IGroupCollection
    {
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool TryGetGroup(string _groupName, out Group _value)
        {
            throw new NotImplementedException();
        }

        public GroupCollection GetGroupsByType(GroupType _groupType)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(Group _group)
        {
            throw new NotImplementedException();
        }

        public int Count => throw new NotImplementedException();

        public Group this[int _index] => throw new NotImplementedException();
    }

}
