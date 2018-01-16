using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordExcercise
{
    public class PasswordRequirements
    {
        public int MaxLength { get; set; }
        public int MinLength { get; set; }
        public int MinUpperAlphaChars { get; set; }
        public int MinLowerAlphaChars { get; set; }
        public int MinNumericChars { get; set; }
        public int MinSpecialChars { get; set; }
    }

    public interface IPasswordGenerator
    {
        string GeneratePassword(PasswordRequirements requirements);
    }

    public class PasswordGenerator : IPasswordGenerator
    {
        PasswordRequirements requirements;
        List<char> password = new List<char>();
        Random random;

        public PasswordGenerator()
        {
            random = new Random();
        }

        public string GeneratePassword(PasswordRequirements requirements)
        {
            if (requirements == null)
            {
                throw new ArgumentNullException(nameof(requirements));
            }

            this.requirements = requirements;
            GenerateRequiredChars();

            while (!PasswordLengthSatisfied())
            {
                password.Add(GenerateRandomChar());
            }

            return string.Join("", password);
        }

        private void GenerateRequiredChars()
        {
            while (!CharsTypeRequirementsSatisfied())
            {
                char character = GenerateRandomChar();

                if (char.IsLower(character) && !LowerRequirementSatisfied())
                {
                    password.Add(character);
                }
                else if (char.IsUpper(character) && !UpperRequirementSatisfied())
                {
                    password.Add(character);
                }
                else if (char.IsNumber(character) && !NumericRequirementSatisfied())
                {
                    password.Add(character);
                }
                else if ((char.IsSymbol(character) || char.IsPunctuation(character))
                    && !SpecialCharRequirementSatisfied())
                {
                    password.Add(character);
                }
            }
        }

        private char GenerateRandomChar()
        {
            return (char)random.Next(33, 126);
        }

        private bool CharsTypeRequirementsSatisfied()
        {
            return LowerRequirementSatisfied() && UpperRequirementSatisfied()
                && NumericRequirementSatisfied() && SpecialCharRequirementSatisfied();
        }

        private bool PasswordLengthSatisfied()
        {
            return password.Count >= requirements.MinLength
                && password.Count <= requirements.MaxLength;
        }

        private bool SpecialCharRequirementSatisfied()
        {
            return password.Where(p => char.IsSymbol(p) || char.IsPunctuation(p))
                .Count() >= requirements.MinSpecialChars;
        }

        private bool NumericRequirementSatisfied()
        {
            return password.Where(p => char.IsNumber(p))
                .Count() >= requirements.MinNumericChars;
        }

        private bool UpperRequirementSatisfied()
        {
            return password.Where(p => char.IsUpper(p))
                .Count() >= requirements.MinUpperAlphaChars;
        }

        private bool LowerRequirementSatisfied()
        {
            return password.Where(p => char.IsLower(p))
                .Count() >= requirements.MinLowerAlphaChars;
        }
    }
}
