# SFA.DAS.Authorization.TestHarness

This is a test harness application to allow developers to test : -

  * Employer features - Toggling, toggling by user whitelisting, toggling by agreement signing.
  * Employer user roles - User membership checks for an account, user role checks for an account.
  * Provider permissions - Provider permission checks for an organisation.

As provided by the SFA.DAS.Authorization package

## Requirements

# For testing Employer User Roles Authorization:
Install [Cosmos DB Emulator]

Load the `AccountUser.json` file under the `TestData` into your local  [Cosmos DB Emulator], this will give the User created by the *AutoLogin* option  the **Owner** role.

[Cosmos DB Emulator]: https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator
