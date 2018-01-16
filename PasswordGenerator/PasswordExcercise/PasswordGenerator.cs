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
        List<char> passwordChars;
        Random random;
        int defaultMinChars= 1;

        public PasswordGenerator()
        {
            random = new Random();
            passwordChars = new List<char>();
        }

        public string GeneratePassword(PasswordRequirements requirements)
        {
            if (requirements == null)
            {
                throw new ArgumentNullException(nameof(requirements));
            }

            this.requirements = requirements;

            GenerateRequiredChars();
            GenerateMissingChars();

            var password = string.Join("", passwordChars);
            return password;
        }

        private void GenerateMissingChars()
        {
            while (!PasswordLengthSatisfied())
            {
                passwordChars.Add(GenerateRandomChar());
            }
        }

        private void GenerateRequiredChars()
        {
            char character;
            while (!CharTypesRequirementsSatisfied())
            {
                character = GenerateRandomChar();

                if (char.IsLower(character) && !LowerRequirementSatisfied())
                {
                    passwordChars.Add(character);
                }
                else if (char.IsUpper(character) && !UpperRequirementSatisfied())
                {
                    passwordChars.Add(character);
                }
                else if (char.IsNumber(character) && !NumericRequirementSatisfied())
                {
                    passwordChars.Add(character);
                }
                else if ((char.IsSymbol(character) || char.IsPunctuation(character))
                    && !SpecialCharRequirementSatisfied())
                {
                    passwordChars.Add(character);
                }
            }
        }

        private char GenerateRandomChar()
        {
            return (char)random.Next(33, 126);
        }

        private bool CharTypesRequirementsSatisfied()
        {
            return LowerRequirementSatisfied() && UpperRequirementSatisfied()
                && NumericRequirementSatisfied() && SpecialCharRequirementSatisfied();
        }

        private bool PasswordLengthSatisfied()
        {
            return passwordChars.Count >= requirements.MinLength
                && passwordChars.Count <= requirements.MaxLength;
        }

        private bool SpecialCharRequirementSatisfied()
        {
            var count = passwordChars
                .Where(p => char.IsSymbol(p) || char.IsPunctuation(p))
                .Count();
            return count >= requirements.MinSpecialChars && count >= defaultMinChars;
        }

        private bool NumericRequirementSatisfied()
        {
            var count = passwordChars.Where(p => char.IsNumber(p)).Count();
            return count >= requirements.MinNumericChars && count >= defaultMinChars;
        }

        private bool UpperRequirementSatisfied()
        {
            var count = passwordChars.Where(p => char.IsUpper(p)).Count();
            return count >= requirements.MinUpperAlphaChars && count >= defaultMinChars;
        }

        private bool LowerRequirementSatisfied()
        {
            var count = passwordChars.Where(p => char.IsLower(p)).Count();
            return count >= requirements.MinLowerAlphaChars && count >= defaultMinChars;
        }
    }
}
