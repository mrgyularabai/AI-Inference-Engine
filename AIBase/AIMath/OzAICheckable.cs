using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ozeki
{
    public abstract class OzAICheckable
    {
        public abstract bool IsPossible(out string error);

        public static bool CheckIfNull(object obj, string name, out string error)
        {
            if (name == null)
            {
                error = "No names provided for the obj to check if null";
                return false;
            }
            if (obj == null)
            {
                var capitalized = Char.ToUpper(name[0]);
                var capName = capitalized + name.Substring(1);
                error = $"{capName} provided was null.";
                return false;
            }
            error = null;
            return true;
        }

        public static bool CheckIfNull(List<object> objs, List<string> names, out string error)
        {
            if (objs == null)
            {
                error = "No objs provided to check if null";
                return false;
            }
            if (names == null)
            {
                error = "No names provided for the objs to check if null";
                return false;
            }
            if (names.Count != objs.Count)
            {
                error = "Not the same number of names provided as objs for when checking if objs are null.";
                return false;
            }
            for (int i = 0; i < names.Count; i++)
            {
                var obj = objs[i];
                var name = names[i];
                if (!CheckIfNull(obj, name, out error))
                    return false;
            }
            error = null;
            return true;
        }
    }
}
