using SecurePass.Data.Entities;

namespace SecurePass.UnitTests.TestData.Services
{
    public static class AccountServiceTestData
    {
        public static readonly int ValidUserId = 1; 

        public static IEnumerable<object[]> GetAllUsersTestData()
        {
            yield return new object[]
            {
                new List<User>
                {
                    new() 
                    {
                        Id = 1,
                        Email = "john.doe@example.com",
                        FirstName = "John",
                        LastName = "Doe",
                        AuthenticationHash = "hash1",
                        DataKey = "dataKey1",
                        RSAPublicKey = "publicKey1",
                        RSAPrivateKey = "privateKey1",
                        RandomSalt = "salt1",
                    }
                }
            };

            yield return new object[]
            {
                new List<User>
                {
                    new() 
                    {
                        Id = 2,
                        Email = "alice.smith@example.com",
                        FirstName = "Alice",
                        LastName = "Smith",
                        AuthenticationHash = "hash2",
                        DataKey = "dataKey2",
                        RSAPublicKey = "publicKey2",
                        RSAPrivateKey = "privateKey2",
                        RandomSalt = "salt2",
                    }
                }
            };
        }
    }
}
