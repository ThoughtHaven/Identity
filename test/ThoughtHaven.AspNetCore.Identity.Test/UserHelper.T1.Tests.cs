using System;
using System.Collections.Generic;
using ThoughtHaven.AspNetCore.Identity.Fakes;
using ThoughtHaven.AspNetCore.Identity.Passwords;
using Xunit;

namespace ThoughtHaven.AspNetCore.Identity
{
    public class UserHelperTests
    {
        public class TUserGeneric
        {
            public class Constructor
            {
                public class PrimaryOverload
                {
                    [Fact]
                    public void NullUserEmailStore_Throws()
                    {
                        Assert.Throws<ArgumentNullException>("userEmailStore", () =>
                        {
                            new UserHelper<User>(
                                userEmailStore: null!,
                                singleUseTokenService: new FakeSingleUseTokenService(),
                                timedLockoutStore: new FakeTimedLockoutStore(),
                                passwordHasher: new FakePasswordHasher(),
                                passwordStrengthValidators: PasswordStrengthValidators(),
                                clock: new FakeSystemClock());
                        });
                    }

                    [Fact]
                    public void NullSingleUseTokenService_Throws()
                    {
                        Assert.Throws<ArgumentNullException>("singleUseTokenService", () =>
                        {
                            new UserHelper<User>(
                                userEmailStore: new FakeUserEmailStore(),
                                singleUseTokenService: null!,
                                timedLockoutStore: new FakeTimedLockoutStore(),
                                passwordHasher: new FakePasswordHasher(),
                                passwordStrengthValidators: PasswordStrengthValidators(),
                                clock: new FakeSystemClock());
                        });
                    }

                    [Fact]
                    public void NullTimedLockoutStore_Throws()
                    {
                        Assert.Throws<ArgumentNullException>("timedLockoutStore", () =>
                        {
                            new UserHelper<User>(
                                userEmailStore: new FakeUserEmailStore(),
                                singleUseTokenService: new FakeSingleUseTokenService(),
                                timedLockoutStore: null!,
                                passwordHasher: new FakePasswordHasher(),
                                passwordStrengthValidators: PasswordStrengthValidators(),
                                clock: new FakeSystemClock());
                        });
                    }

                    [Fact]
                    public void NullPasswordHasher_Throws()
                    {
                        Assert.Throws<ArgumentNullException>("passwordHasher", () =>
                        {
                            new UserHelper<User>(
                                userEmailStore: new FakeUserEmailStore(),
                                singleUseTokenService: new FakeSingleUseTokenService(),
                                timedLockoutStore: new FakeTimedLockoutStore(),
                                passwordHasher: null!,
                                passwordStrengthValidators: PasswordStrengthValidators(),
                                clock: new FakeSystemClock());
                        });
                    }

                    [Fact]
                    public void NullPasswordStrengthValidators_Throws()
                    {
                        Assert.Throws<ArgumentNullException>("passwordStrengthValidators", () =>
                        {
                            new UserHelper<User>(
                                userEmailStore: new FakeUserEmailStore(),
                                singleUseTokenService: new FakeSingleUseTokenService(),
                                timedLockoutStore: new FakeTimedLockoutStore(),
                                passwordHasher: new FakePasswordHasher(),
                                passwordStrengthValidators: null!,
                                clock: new FakeSystemClock());
                        });
                    }

                    [Fact]
                    public void NullClock_Throws()
                    {
                        Assert.Throws<ArgumentNullException>("clock", () =>
                        {
                            new UserHelper<User>(
                                userEmailStore: new FakeUserEmailStore(),
                                singleUseTokenService: new FakeSingleUseTokenService(),
                                timedLockoutStore: new FakeTimedLockoutStore(),
                                passwordHasher: new FakePasswordHasher(),
                                passwordStrengthValidators: PasswordStrengthValidators(),
                                clock: null!);
                        });
                    }
                }
            }

            public class UserEmailStoreMethod
            {
                public class EmptyOverload
                {
                    [Fact]
                    public void WhenCalled_ReturnsCastedStoreFromConstructor()
                    {
                        var store = new FakeUserEmailStore();

                        var result = new FakeUserHelper2(userEmailStore: store)
                            .UserEmailStore<User>();

                        Assert.Equal(store, result);
                    }
                }
            }
        }

        private static IEnumerable<IPasswordStrengthValidator> PasswordStrengthValidators() =>
            new IPasswordStrengthValidator[] { new MinimumLengthPasswordStrengthValidator() };
    }
}