using System;
using System.Collections.Generic;
using System.Linq;

namespace stork
{
    //Utility class for a collection of scopes in Stork.
    public class StorkScopeCollection
    {
        //A list of all the current scopes.
        private List<Dictionary<string, StorkValue>> Scopes = new List<Dictionary<string, StorkValue>>();
        public StorkScopeCollection()
        {
            //Add the global scope.
            Scopes.Add(new Dictionary<string, StorkValue>());
        }

        //Create a new scope and return its index.
        public int CreateScope()
        {
            //Add a new scope to the list.
            Scopes.Add(new Dictionary<string, StorkValue>());

            //Return the index of this new scope.
            return Scopes.Count - 1;
        }

        //Get the current deepest scope.
        public Dictionary<string, StorkValue> GetCurrentScope()
        {
            if (Scopes.Count == 0) { return null; }
            return Scopes.Last();
        }

        //Get a scope with a given index.
        public Dictionary<string, StorkValue> GetScope(int index)
        {
            if (index < 0) { return null; }
            return Scopes[index];
        }

        //Returns all variables.
        public Dictionary<string, StorkValue> GetVariables()
        {
            //Concatenate all scopes together.
            return Scopes.SelectMany(dict => dict)
                         .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        //Gets a variable of a given name from the deepest index.
        public StorkValue GetVariable(string name)
        {
            //Start at the deepest local scope!
            for (int i=Scopes.Count-1; i>=0; i--)
            {
                if (Scopes[i].ContainsKey(name))
                {
                    return Scopes[i][name];
                }
            }
            
            //Not found, return null.
            return default(StorkValue);
        }

        //Sets a variable of a given name at the deepest found index.
        public void SetVariable(string name, object value)
        {
            //Start at the deepest local scope!
            for (int i = Scopes.Count - 1; i >= 0; i--)
            {
                if (Scopes[i].ContainsKey(name))
                {
                    //Set the value.
                    Scopes[i][name] = new StorkValue() { Type = Scopes[i][name].Type, Value = value };
                    return;
                }
            }
        }

        //Craetes a variable at the current deepest scope. Returns success as bool.
        public bool CreateVariable(string name, object value, StorkType type)
        {
            //Checking if the variable already exists.
            if (!GetVariable(name).Equals(default(StorkValue)))
            {
                //Value already exists.
                return false;
            }

            //Adding to scope.
            Scopes.Last().Add(name, new StorkValue()
            {
                Type = type,
                Value = value
            });

            //Returning success.
            return true;
        }
    }
}
