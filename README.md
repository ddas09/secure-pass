# SecurePass - Secure password management

## About

A tool to manage your passwords and share them securely.

## Dependencies

- Dotnet 6.0
- Node 14.15.0
- Angular 13.0.3
- Sql Server Express 2019

# Encryption Model

![onepass_architecture](https://user-images.githubusercontent.com/75975903/188354106-526fe65a-40a7-4a1a-b3ce-cb2296105ab6.png)

# Password Recovery

![password_recovery](https://user-images.githubusercontent.com/75975903/188354149-e96f41e8-ce54-480a-a38c-d7feaa81cfc2.png)

# Encryption of Vault Record

![vault_records](https://user-images.githubusercontent.com/75975903/188354192-09728345-c128-41eb-bfd3-8d221f506870.png)

# Steps

- **npm install** - will install all the frontend packages
- **ng s** - to start the local server
- change connection string in OnePassAPI/appsettings.json
- **dotnet restore** - will install all the backend packages
- **dotnet run** - to run backend server
