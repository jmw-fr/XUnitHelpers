// <copyright file="DatabaseFixture_UnitTest.cs" company="Weeger Jean-Marc">
// Copyright Weeger Jean-Marc under MIT Licence. See https://opensource.org/licenses/mit-license.php.
// </copyright>

namespace Jmw.XUnitHelpers.Tests
{
    using System.Threading.Tasks;
    using Dapper;
    using Microsoft.Data.Sqlite;
    using Xunit;

    /// <summary>
    /// Tests of <see cref="DatabaseFixture{T}"/>.
    /// </summary>
    [Collection(nameof(Fixtures.SQLiteFixtureCollection))]
    public class DatabaseFixture_UnitTest
    {
        private Fixtures.SQLiteFixture sqliteFixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseFixture_UnitTest"/> class.
        /// </summary>
        /// <param name="sqliteFixture">SQLite database fixture</param>
        public DatabaseFixture_UnitTest(Fixtures.SQLiteFixture sqliteFixture)
        {
            this.sqliteFixture = sqliteFixture;
        }

        /// <summary>
        /// Test that <see cref="Fixtures.SQLiteFixture"/> has correctly created the table and data.
        /// </summary>
        /// <returns>Async task</returns>
        [Fact(DisplayName = nameof(DatabaseFixture_Test))]
        public async Task DatabaseFixture_Test()
        {
            using (var conn = new SqliteConnection())
            {
                conn.ConnectionString = this.sqliteFixture.GetConnectionString();

                Fixtures.Test test = await conn.QuerySingleAsync<Fixtures.Test>("SELECT * FROM `test`");

                Assert.Equal(1, test.Id);
                Assert.Equal("Name", test.Name);
            }
        }
    }
}
