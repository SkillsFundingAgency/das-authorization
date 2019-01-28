# SFA.DAS.Authorization.TestHarness

This is a test harness application to allow developers to test : -

  * Employer features - Toggling, toggling by user whitelisting, toggling by agreement signing.
  * Employer user roles - User membership checks for an account, user role checks for an account.
  * Provider permissions - Provider permission checks for an organisation.

As provided by the SFA.DAS.Authorization package

## Requirements

The test harness assumes the necessary Table Storage Configuration and DBs are present for Employer Features, Employer User Roles, and Provider Permissions, when testing these specific areas. Refer to the Readmes for das-authorization and das-provider-relationships for these requirements.

Additionally for Employer Roles, some additional test data will need to be loaded to give the User created by the *AutoLogin* option  the **Owner** role:

Load the `AccountUser.json` file under the `TestData` into your local  [Cosmos DB Emulator].

[Cosmos DB Emulator]: https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator
