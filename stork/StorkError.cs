﻿using System;
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
            invalid_statement
        }

        public static void printError(Error err, StorkActionTree.ActionItem act=null)
        {
            switch (err)
            {
                case Error.invalid_args:
                    Console.WriteLine("STKI FATAL E001: No command line arguments given, use--help for more information.");
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
                default:
                    Console.WriteLine("STKI FATAL: Invalid error code given.");
                    break;
            }

            if (act!=null)
            {
                Console.WriteLine("ERROR AT ACTION: " + act.act.ToString() + " , CONTENT " + act.item);
            }
        }
    }
}
