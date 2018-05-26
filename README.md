Thought Haven Identity
===

An alternative Identity framework for AspNetCore built on flexibility, extensibility, and code separation.

Apps should have a smart IdentityService class that varies based on the shape of the generic User. For example, does the user class contain an email address? Then email management methods will appear on the IdentityService [`identity.UpdateEmail(email)`].

What about an email and password? Then use them to login [`identity.Login(email, password)`].

If the User has no email and/or password, then those helper methods simply drop off the identity instance.

**Applications decide** whether or not their users will have IDs, password hashes, security stamps, and last logins. And the IdentityService lights up with only the methods that make sense for that User.

No more `UserName` properties that every developer must remember to ignore. No `UserName` property, no user name methods. Update the User class with a `UserName` later, and the IdentityService wakes up to support it.

## Other projects:
* [AspNetCore](https://github.com/ThoughtHaven/AspNetCore): Wrappers and helpers built on top of Microsoft's wonderful AspNetCore and MVC libraries. Get started faster and easier while following enforced best practices.
* [Azure](https://github.com/ThoughtHaven/Azure): Data abstractions and helpers around the official Microsoft Azure library so that cloud storage development feels more like working with POCOs, not TableEntities, etc.
* [Core](https://github.com/ThoughtHaven/Core): Low-level libraries and helpers for use in any application.
* [Security](https://github.com/ThoughtHaven/Security): Making the secure thing easier and faster.
* [Emailers](https://github.com/ThoughtHaven/Emailers): Abstractions for email messages and services, as well as a SendGrid implementation.
