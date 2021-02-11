using System;
using System.Collections.Generic;

namespace BracketsValidationDemo

{
	/// <summary>
	/// Simple validation result model.
	/// Not in a separate .cs file due to the app's simplicity.
	/// </summary>
	class ValidationResult
	{
		public bool Valid { get; set; }
		public string ErrorMessage { get; set; }
	}

	class Program
	{
		/// <summary>
		/// Holds possible opening brackets values (3 types).
		/// </summary>
		static List<char> _openingBrackets = new() { '(', '[', '{' };

		/// <summary>
		/// Holds possible brackets values (3 types).
		/// </summary>
		static List<char> _brackets = new() { '(', ')', '[', ']', '{', '}' };

		/// <summary>
		/// Checks if the provided input contains brackets of any of 3 types - ()[]{}, - and that they are closed correctly.
		/// </summary>
		/// <param name="input">The input string to check.</param>
		static ValidationResult ValidateBrackets(string input)
		{
			// Isolate brackets from the input string for easier processing later.
			var bracketsIsolated = "";

			foreach (var c in input)
			{
				if (_brackets.Contains(c))
				{
					bracketsIsolated += c;
				}
			}

			if (bracketsIsolated.Length == 0)
			{
				return new ValidationResult
				{
					Valid = false,
					ErrorMessage = "ERR_NO_BRACKETS"
				};
			}
			else if (bracketsIsolated.Length % 2 != 0)
			{
				return new ValidationResult
				{
					Valid = false,
					ErrorMessage = "ERR_NOT_EVEN_TOTAL_BRACKET_AMOUNT"
				};
			}

			// Stores encountered opening brackets.			
			string openingBracketsStore = "";

			// When a closing bracket of the same type as the last stored opening bracket ([ & ], for example)
			// is encountered, that last stored opening bracket is removed from the store.
			//
			// Thus, after looping through all characters in isolated brackets string,
			// an empty opening brackets store means all opening brackets were closed correctly.

			foreach (var currentBracket in bracketsIsolated)
			{
				if (_openingBrackets.Contains(currentBracket))
				{
					// Encountered an opening bracket.

					openingBracketsStore += currentBracket;
				}
				else
				{
					// Encountered a closing bracket.

					if (openingBracketsStore.Length == 0)
					{
						return new ValidationResult
						{
							Valid = false,
							ErrorMessage = "ERR_CLOSING_BRACKET_BEFORE_OPENING"
						};
					}

					var lastStoredOpeningBracketPosition = openingBracketsStore.Length - 1;

					if (currentBracket == GetClosingBracket(openingBracketsStore[lastStoredOpeningBracketPosition]))
					{
						openingBracketsStore = openingBracketsStore.Remove(lastStoredOpeningBracketPosition);
					}
					else
					{
						return new ValidationResult
						{
							Valid = false,
							ErrorMessage = "ERR_WRONG_CLOSING_BRACKET"
						};
					}
				}
			}

			if (openingBracketsStore.Length == 0)
			{
				return new ValidationResult
				{
					Valid = true
				};
			}
			else
			{
				return new ValidationResult
				{
					Valid = false,
					ErrorMessage = "ERR_BRACKET_AMOUNT_OPENING_GREATER_THAN_CLOSING"
				};
			}
		}

		/// <summary>
		/// Returns a closing bracket of the same type as the provided opening bracket.
		/// </summary>
		/// <param name="openingBracket">The opening bracket to get a closing bracket for.</param>
		static char GetClosingBracket(char openingBracket)
		{
			return openingBracket switch
			{
				'(' => ')',
				'[' => ']',
				'{' => '}',
				_ => throw new ArgumentException("Unsupported opening bracket", nameof(openingBracket))
			};
		}

		static void Main()
		{
			while (true)
			{
				Console.WriteLine("Enter the string to check for correct braces positioning:");

				var input = Console.ReadLine();

				var result = ValidateBrackets(input);

				if (result.Valid)
				{
					Console.WriteLine("True.");
				}
				else
				{
					Console.WriteLine($"False. Error: {result.ErrorMessage}");
				}

				Console.WriteLine();
			}
		}
	}
}
