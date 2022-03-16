using System.Collections;
using System.Collections.Generic;
using UCCollaborationLib;
using System.Runtime.InteropServices;

namespace OutlookPresenceProvider
{
    [ComVisible(true)]
    public class IMClientGroupCollection : GroupCollection
    {
        // private CollectionBase<>
        private List<Group> _groups;
        public IMClientGroupCollection()
        {
            _groups = new List<Group>();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var group in _groups)
            {
                yield return group;
            }
        }

        public bool TryGetGroup(string _groupName, out Group _value)
        {
            foreach (Group _group in _groups)
            {
                if (_group.Name == _groupName)
                {
                    _value = _group;
                    return true;
                }
            }

            _value = null;
            return false;
        }

        public GroupCollection GetGroupsByType(GroupType _groupType)
        {
            foreach(Group _group in _groups)
            {
                if(_group.Type == _groupType)
                {
                    return (GroupCollection)this;
                }
            }
            return null;
        }

        public int IndexOf(Group _group)
        {
            return _groups.IndexOf(_group as IMClientGroup);
        }

        public int Count => _groups.Count;

        public Group this[int _index] => _groups[_index];

        public void AddGroup(Group _group)
        {
            _groups.Add(_group);
        }
    }

}
