using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stork
{
    public class StorkScope
    {
        //List of all current scopes.
        public List<Dictionary<string, StorkClassInstance>> Scopes = new List<Dictionary<string, StorkClassInstance>>();

        //Constructor, adds the default highest scope.
        public StorkScope()
        {
            Scopes.Add(new Dictionary<string, StorkClassInstance>());
        }

        //Attempts to find a given variable, starting at the deepest scope.
        public StorkClassInstance GetVariable(string name)
        {
            for (int i = Scopes.Count - 1; i >= 0; i--)
            {
                if (Scopes[i].ContainsKey(name))
                {
                    return Scopes[i][name];
                }
            }

            //Not found, return null.
            return null;
        }

        //Adds a new variable to the deepest scope. Returns tuple of success/error message.
        public Tuple<bool, string> AddVariable(string name, StorkClassInstance value)
        {
            //If the variable already exists, error and return.
            if (GetVariable(name)!=null)
            {
                return new Tuple<bool, string>(false, "This variable already exists in scope.");
            }

            //Add to deepest scope.
            Scopes.Last().Add(name, value);
            return new Tuple<bool, string>(true, "");
        }

        //Adds a new scope at the deepest level.
        public void AddScope()
        {
            Scopes.Add(new Dictionary<string, StorkClassInstance>());
        }

        //Removes the lowest scope.
        public bool RemoveScope()
        {
            //Can't remove the highest scope, otherwise environment is closed.
            if (Scopes.Count<=1)
            {
                return false;
            }

            //Remove last.
            Scopes.RemoveAt(Scopes.Count - 1);
            return true;
        }
    }
}
