# SecurePass Backend

SecurePass Backend is the backend API for SecurePass, a password management system. This API provides endpoints for managing user accounts, password records, authentication, and more.

## Summary

SecurePass is designed to securely store and manage passwords for various accounts and services. It offers features such as:

- **User Authentication**: Securely authenticate users to access their password vault.
- **Account Management**: Manage user accounts, including signup, sign-in, logout, and password management.
- **Password Records**: CRUD operations for password records, including adding, updating, deleting, and sharing.
- **Security**: Ensure secure storage and transmission of passwords and sensitive information.

## Demo
https://github.com/ddas09/secure-pass-webapp/assets/75975903/9b1933a9-b269-4dfa-a876-2f41cec722f1

## Architecture Diagrams

- **Application Architecture**:

![Architechture](https://github.com/ddas09/secure-pass/assets/75975903/8143ad59-0f02-4712-a7f0-077ee687b3ee)

- **Password Recovery**:

![PasswordRecovery](https://github.com/ddas09/secure-pass/assets/75975903/77af1d11-b93d-40e6-9fed-325279f1f5cb)

- **Vault Record Encryption**:

![VaultRecordEncryption](https://github.com/ddas09/secure-pass/assets/75975903/0c3c0fb7-3dca-42b5-bdc7-eecd624a2f5e)

## Getting Started

These instructions will help you set up and run the SecurePass backend API on your local machine for development and testing purposes.

### Prerequisites

Make sure you have the following installed on your machine:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- [pgAdmin](https://www.pgadmin.org/download/)

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/ddas09/secure-pass.git
   ```

2. Navigate to the project directory:

   ```bash
   cd secure-pass
   ```

3. Set up the database:

   - Create a PostgreSQL database for SecurePass.
   - Update the connection string in the `appsettings.json` file with your PostgreSQL database credentials.

   ```json
   {
     "ConnectionStrings": {
       "SecurePass": "<your-connection-string>"
     }
   }
   ```

4. Update `appsettings.json`:

   - **Token Secrets**: Generate random secret keys for JWT token generation. You can use any secure random string generator online for this purpose. Update the `JwtConfiguration` section in the `appsettings.json` file with your token secrets.

   - **Token Expirations**: Adjust the different token expirations according to your needs.

   ```json
   {
     "JwtConfiguration": {
       "SignupTokenExpirationTimeInMinutes": 180,
       "AccessTokenSecret": "<some-secret-key-generated-oneline>",
       "AccessTokenExpirationTimeInMinutes": 15,
       "RefreshTokenSecret": "<some-secret-key-generated-oneline>",
       "RefreshTokenExpirationTimeInMinutes": 30,
       "RecoveryTokenExpirationTimeInMinutes": 60,
       "IdentityTokenSecret": "<some-secret-key-generated-oneline>",
       "IdentityTokenExpirationTimeInMinutes": 15,
       "Issuer": "<url-of-token-issuer>",
       "Audience": "<url-of-audiences>"
     }
   }
   ```

   - **Mail Configuration**: Obtain an app password for Gmail using the [instructions here](https://www.getmailbird.com/gmail-app-password/). Update the `MailSettings` section in the `appsettings.json` file with your Gmail credentials and app password.

   ```json
   {
     "MailConfiguration": {
       "Mail": "<mail-id-for-sending-mail>",
       "DisplayName": "<mail-display-name>",
       "Password": "<secure-password>",
       "Host": "<host-of-mail-smtp>",
       "Username": "<mail-id-for-sending-mail>",
       "Port": "<mail-port>"
     }
   }
   ```

   - **Hangfire Configuration**: Choose hangfire credentials of your choice. This can be used to access the hangfire dashboad at this url - `http://localhost:5001/hangfire/dashboard`.

   ```json
   {
     "Hangfire": {
       "AdminUser": "<hangfire-username-of-your-choice>",
       "BasicAuthPass": "<hangfire-password-of-your-choice>"
     }
   }
   ```

5. Install dependencies:

   ```bash
   dotnet restore
   ```

### Running Tests

To run the tests for SecurePass Backend, open a terminal or command prompt, navigate to the project directory, and run:

```bash
dotnet test
```

### Running the Backend

To run the backend API locally:

1. Ensure that your PostgreSQL server is running.

2. Run the API:

```bash
dotnet run
```

## Contributing

1. Fork the repository.

2. Create a new branch:
   ```bash
   git checkout -b feature/your-feature
   ```
3. Make your changes.
4. Commit your changes 
   ```bash
   git commit -a 'Add new feature'
   ```
5. Push to the branch 
   ```bash
   git push origin feature/your-feature
   ```
6. Create a new Pull Request.

## License
This project is open-source and licensed under the [MIT License](https://opensource.org/license/mit).
The API will be running at `https://localhost:5001` by default.

### API Documentation

Once the backend is running, you can access the API documentation (Swagger UI) at `https://localhost:5001/swagger`. This documentation provides details about the available endpoints and how to use them.
