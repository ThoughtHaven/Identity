using System.Collections.Generic;

namespace ThoughtHaven.AspNetCore.Identity.Fakes
{
    public class FakeUserValidators : List<FakeUserValidator>
    {
        public bool AllValidatorsCalled
        {
            get
            {
                foreach (var validator in this)
                { if (!validator.Validate_Called) { return false; } }

                return true;
            }
        }

        public FakeUserValidators(byte validCount = 1, byte invalidCount = 0)
        {
            for (var i = 0; i < validCount; i++) { this.Add(new FakeUserValidator()); }

            for (var i = 0; i < invalidCount; i++)
            { this.Add(new FakeUserValidator(invalid: true)); }
        }
    }
}