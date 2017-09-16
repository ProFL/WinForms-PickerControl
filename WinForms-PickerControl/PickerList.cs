using System;
using System.Collections.Generic;

namespace PickerControl
{
    public abstract partial class Picker<T>
    {
        public class PickerList : List<T>
        {
            public event EventHandler ItemAdded;
            public event EventHandler ItemRemoved;

            public PickerList() : base() { }

            public new void Add(T item)
            {
                base.Add(item);
                ItemAdded?.Invoke(this, null);
            }

            public new void AddRange(IEnumerable<T> collection)
            {
                try
                {
                    base.AddRange(collection);
                }
                catch (Exception)
                {
                    throw;
                }

                foreach (var item in collection)
                {
                    ItemAdded?.Invoke(this, null);
                }
            }

            public new void Remove(T item)
            {
                try
                {
                    base.Remove(item);
                }
                catch (Exception)
                {
                    throw;
                }

                ItemRemoved?.Invoke(this, null);
            }

            public new int RemoveAll(Predicate<T> match)
            {
                int prevItemCount = Count;
                int retVal = 0;

                try
                {
                    retVal = base.RemoveAll(match);
                }
                catch (Exception)
                {
                    throw;
                }

                for (int i = 0; i < prevItemCount; i++)
                {
                    ItemRemoved?.Invoke(this, null);
                }

                return retVal;
            }

            public new void RemoveAt(int index)
            {
                try
                {
                    base.RemoveAt(index);
                }
                catch (Exception)
                {
                    throw;
                }

                ItemRemoved?.Invoke(this, null);
            }

            public new void RemoveRange(int index, int count)
            {
                try
                {
                    base.RemoveRange(index, count);
                }
                catch (Exception)
                {
                    throw;
                }

                for (int i = 0; i < count; i++)
                {
                    ItemRemoved?.Invoke(this, null);
                }
            }
        }
    }
}