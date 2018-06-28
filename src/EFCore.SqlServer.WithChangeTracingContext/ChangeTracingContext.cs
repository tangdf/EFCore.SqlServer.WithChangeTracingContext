using System;
using System.Threading;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    internal class ChangeTracingContext:IDisposable
    {

        private static readonly AsyncLocal<Item> _asyncLocal=new AsyncLocal<Item>();

        public ChangeTracingContext(string contextName)
        {
            _asyncLocal.Value = new Item(_asyncLocal.Value,contextName);
        }

        public static string CurrentContextName
        {
            get
            {
                var item = _asyncLocal.Value;
                return item?.ContextName;
            }
        }

        public void Dispose()
        {
            var item = _asyncLocal.Value;
            _asyncLocal.Value = item?.OuterItem;

        }

        private class Item
        {
            public Item(Item outerItem, string contextName)
            {
                OuterItem = outerItem;
                ContextName = contextName;
            }

            public Item OuterItem { get; set; }
            public string ContextName { get; set; }
        }
    }
}