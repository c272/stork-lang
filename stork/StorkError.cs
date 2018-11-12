using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stork
{
    class StorkError
    {
        public enum Error
        {
            invalid_args,
            invalid_file,
            expected_statement,
            expected_statement_close,
            invalid_statement,
            syntax_error_identifier,
            check_outside_comparison,
            invalid_argument_syntax,
            invalid_variable_name,
            cannot_assign_variable_value,
            invalid_function_name,
            invalid_parameter_type,
            function_proto_nodefine,
            function_missing_parameter_separator,
            function_missing_parameter,
            no_type_found,
            preprocess_no_identifier,
            preprocess_no_value,
            preprocess_invalid_identifier,
            expected_endline,
            for_statement_overflow,
            for_statement_underflow
        }

        public static void printError(Error err, bool fatal = true, string act=null)
        {
            switch (err)
            {
                case Error.invalid_args:
                    Console.WriteLine("STKI FATAL E001: No command line arguments given, use --help for more information.");
                    break;
                case Error.invalid_file:
                    Console.WriteLine("STKI FATAL E002: Invalid file path given, or error reading the file.");
                    break;
                case Error.expected_statement:
                    Console.WriteLine("STKI FATAL E003: Expected statement, but none was given.");
                    break;
                case Error.expected_statement_close:
                    Console.WriteLine("STKI FATAL E004: Expected statement close, but none was given.");
                    break;
                case Error.invalid_statement:
                    Console.WriteLine("STKI FATAL E005: Statement in block cannot be evaluated, did you miss a bracket?");
                    break;
                case Error.syntax_error_identifier:
                    Console.WriteLine("STKI FATAL E006: Syntax error, unknown identifier found.");
                    break;
                case Error.check_outside_comparison:
                    Console.WriteLine("STKI FATAL E007: Check statement given, but outside a comparison.");
                    break;
                case Error.invalid_argument_syntax:
                    Console.WriteLine("STKI FATAL E008: Invalid func_argument syntax given, syntax error.");
                    break;
                case Error.invalid_variable_name:
                    Console.WriteLine("STKI FATAL E009: Variable identifier given, but the name is invalid or nonexistant.");
                    break;
                case Error.cannot_assign_variable_value:
                    Console.WriteLine("STKI FATAL E010: Invalid syntax or variable value, cannot set variable to value:");
                    break;
                case Error.invalid_function_name:
                    Console.WriteLine("STKI FATAL E011: Function identifier given, but the name is invalid or nonexistant.");
                    break;
                case Error.invalid_parameter_type:
                    Console.WriteLine("STKI FATAL E012: You cannot give this function the type of parameter specified.");
                    break;
                case Error.function_proto_nodefine:
                    Console.WriteLine("STKI FATAL E013: Function prototype declared, but is not closed by an endline character or a definition.");
                    break;
                case Error.function_missing_parameter_separator:
                    Console.WriteLine("STKI FATAL E014: Another function parameter is given, but there is no separator.");
                    break;
                case Error.function_missing_parameter:
                    Console.WriteLine("STKI FATAL E015: A parameter separator was given, but no parameter follows it.");
                    break;
                case Error.no_type_found:
                    Console.WriteLine("STKI FATAL E016: No valid type was found in check where one was expected.");
                    break;
                case Error.preprocess_no_identifier:
                    Console.WriteLine("STKI FATAL E017: Preprocess symbol detected, but no identifier followed.");
                    break;
                case Error.preprocess_no_value:
                    Console.WriteLine("STKI FATAL E018: Preprocess identifier detected, but no value was given.");
                    break;
                case Error.preprocess_invalid_identifier:
                    Console.WriteLine("STKI FATAL E019: Invalid preprocess identifier given.");
                    break;
                case Error.expected_endline:
                    Console.WriteLine("STKI FATAL E020: No endline character detected where one was expected.");
                    break;
                case Error.for_statement_overflow:
                    Console.WriteLine("STKI FATAL E021: Too many statements in a single for loop.");
                    break;
                default:
                    Console.WriteLine("STKI FATAL: Invalid error code given.");
                    break;
            }

            if (act!=null)
            {
                Console.WriteLine("ERROR AT: \""+act+"\".");
            }

            if (fatal)
            {
                Environment.Exit(-1);
            }
        }
    }
}
