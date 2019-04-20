using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace stork
{
    public enum TokenType {
        Invalid, //not a valid token
        OpenBracket, // (
        CloseBracket, // )
        OpenCBracket, // {
        CloseCBracket, // }
        StringLiteral, // "sometext"
        IntLiteral, // 92183
        FunctionDefinition, // func
        Identifier, // name of a var, func, etc
        Separator, // ,
        PreprocessorStatement, // ~
        EndLine // ;
    }

    public class Tokenizer
    {
        //A static list of the current token pairs of type to Regex.
        private static List<Tuple<TokenType, string, int>> TokenPairs = new List<Tuple<TokenType, string, int>>();

        /// <summary>
        /// Takes the given script and turns it into a list of tokens, used for parsing.
        /// </summary>
        /// <param name="script">The script to process.</param>
        /// <returns>A list of token structs.</returns>
        public static List<Token> Process(string script)
        {
            //Create a list of matches for the script.
            var allMatches = new List<Tuple<TokenType, Match>>();

            //For every type of added token, find regex matches.
            foreach (var tokenRegex in TokenPairs)
            {
                Regex tokenRgx = new Regex(tokenRegex.Item2);
                var matches = tokenRgx.Matches(script);

                //Place the matches into the global match list.
                foreach (Match match in matches)
                {
                    allMatches.Add(new Tuple<TokenType, Match>(tokenRegex.Item1, match));
                }
            }

            //Sorting list by index in the match.
            allMatches.Sort((x, y) => x.Item2.Index.CompareTo(y.Item2.Index));

            //Removing duplicate tokens, based on precedence.
            //CURRENTLY INEFFICIENT! REWORK LATER!
            for (int i = 0; i < allMatches.Count; i++)
            {
                for (int j = 0; j < allMatches.Count; j++)
                {
                    //Don't compare to self.
                    if (i == j) { continue; }

                    //Checking for substrings.
                    string sub1 = allMatches[i].Item2.Value, sub2 = allMatches[j].Item2.Value;

                    if (sub1.Contains(sub2) || sub2.Contains(sub1))
                    {
                        //One contains the other, use precedence.
                        //Getting precedence of both.
                        int iPrecedence = TokenPairs.Find(x => x.Item1 == allMatches[i].Item1).Item3;
                        int jPrecedence = TokenPairs.Find(x => x.Item1 == allMatches[j].Item1).Item3;

                        //Removing the one with the highest precedence number. (No. 1 is the best)
                        if (iPrecedence > jPrecedence)
                        {
                            allMatches.RemoveAt(i);
                        } else
                        {
                            allMatches.RemoveAt(j);
                        }
                    }
                }
            }

            //Stripping the script of all matched tokens, to check for invalid tokens.
            foreach (var match in allMatches)
            {
                script = script.Replace(match.Item2.Value, "");
            }

            //Replacing useless \n and \r characters before error.
            string[] invalids = script.Replace("\n", "").Replace("\r", "").Split(' ');
            foreach (var invalid in invalids)
            {
                if (invalid!="")
                {
                    Console.WriteLine("STKI Error 001: Invalid token '" + invalid + "' in script.");
                }
            }
            

            //Adding all matches in order to the return list.
            var output = new List<Token>();
            foreach (var matchTuple in allMatches)
            {
                output.Add(new Token()
                {
                    Type = matchTuple.Item1,
                    Value = matchTuple.Item2.Value
                });
            }

            //Return the list.
            return output;
        }

        /// <summary>
        /// Adds a token pattern to the global list to be checked.
        /// </summary>
        /// <param name="regex">The regex for the desired token.</param>
        /// <param name="token">The token to add.</param>
        public static void AddToken(string regex, TokenType token, int prec)
        {
            TokenPairs.Add(new Tuple<TokenType, string, int>(token, regex, prec));
        }
    }

    /// <summary>
    /// Struct that describes the structure of a single token identifier.
    /// </summary>
    public struct Token
    {
        public TokenType Type;
        public string Value;
    }
}
