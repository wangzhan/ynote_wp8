using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YNote.Common
{
    public class CollectionUtil
    {
        public static void CloneDict<T1, T2>(IDictionary<T1, T2> src, IDictionary<T1, T2> dest)
        {
            foreach (var s in src)
            {
                dest.Add(s);
            }
        }
    }
}
