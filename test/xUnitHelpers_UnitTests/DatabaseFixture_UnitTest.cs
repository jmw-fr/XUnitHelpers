// <copyright file="DatabaseFixture_UnitTest.cs" company="Weeger Jean-Marc">
// Copyright Weeger Jean-Marc under MIT Licence. See https://opensource.org/licenses/mit-license.php.
// </copyright>

namespace Jmw.XUnitHelpers.Tests
{
    using System.Threading.Tasks;
    using Dapper;
    using Jmw.XUnitHelpers.Tests.Fixtures;
    using Microsoft.Data.Sqlite;
    using Xunit;

    /// <summary>
    /// Tests of <see cref="DatabaseFixture{T}"/>.
    /// </summary>
    [Collection(nameof(SQLiteFixtureCollection))]
    public class DatabaseFixture_UnitTest
    {
        private SQLiteFixture sqliteFixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseFixture_UnitTest"/> class.
        /// </summary>
        /// <param name="sqliteFixture">SQLite database fixture</param>
        public DatabaseFixture_UnitTest(SQLiteFixture sqliteFixture)
        {
            this.sqliteFixture = sqliteFixture;
        }

        /// <summary>
        /// Test that <see cref="SQLiteFixture"/> has correctly created the table and data.
        /// </summary>
        /// <returns>Async task</returns>
        [Fact]
        [Trait("DatabaseFixture", nameof(SQLiteFixture))]
        public async Task DatabaseFixture_MustInsert_Fixture()
        {
            // Arrange
            TestData testData;

            // Act
            this.sqliteFixture.InsertFixtures();

            using (var conn = new SqliteConnection())
            {
                conn.ConnectionString = this.sqliteFixture.GetConnectionString();

                testData = await conn.QuerySingleAsync<TestData>("SELECT * FROM `test`");
            }

            // Assert
            Assert.NotNull(testData);
            Assert.True(this.sqliteFixture.FixturesExecuted);
            Assert.Equal(1, testData.Id);
            Assert.Equal("Name", testData.Name);
        }

        /// <summary>
        /// Test that <see cref="DatabaseFixture{T}"/> does not insert fixtures if flag is false.
        /// </summary>
        /// <returns>Async task</returns>
        [Fact]
        [Trait("DatabaseFixture", nameof(SQLiteFixture))]
        public async Task DatabaseFixture_MustNot_InsertFixtures()
        {
            // Arrange

            // Act
            using (var conn = new SqliteConnection())
            {
                conn.ConnectionString = this.sqliteFixture.GetConnectionString();

                // Assert
                Assert.False(this.sqliteFixture.FixturesExecuted);
                await Assert.ThrowsAsync<SqliteException>(async () => await conn.QuerySingleAsync<int>("SELECT count(*) FROM `test`"));
            }
        }

        /// <summary>
        /// Test that <see cref="DatabaseFixture{T}"/> does not insert fixtures if flag is false.
        /// </summary>
        /// <returns>Async task</returns>
        [Fact]
        [Trait("DatabaseFixture", nameof(SQLiteFixture))]
        public async Task DatabaseFixture_Must_DeleteFixtures()
        {
            // Arrange

            // Act
            this.sqliteFixture.InsertFixtures();
            this.sqliteFixture.RemoveFixtures();

            // Assert
            using (var conn = new SqliteConnection())
            {
                conn.ConnectionString = this.sqliteFixture.GetConnectionString();

                Assert.False(this.sqliteFixture.FixturesExecuted);
                await Assert.ThrowsAsync<SqliteException>(async () => await conn.QuerySingleAsync<int>("SELECT count(*) FROM `test`"));
            }
        }
    }
}
