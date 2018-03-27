// <copyright file="SQLiteFixture.cs" company="Weeger Jean-Marc">
// Copyright Weeger Jean-Marc under MIT Licence. See https://opensource.org/licenses/mit-license.php.
// </copyright>

namespace Jmw.XUnitHelpers.Tests.Fixtures
{
    using System.Collections.Generic;
    using System.IO;
    using Jmw.XUnitHelpers;
    using Microsoft.Data.Sqlite;
    using Xunit;

    /// <summary>
    /// Database fixture for SQLite
    /// </summary>
    public class SQLiteFixture : DatabaseFixture<SqliteConnection>
    {
        /// <summary>
        /// Override of <see cref="DatabaseFixture{T}.Dispose"/> to allow deletion of sqlite file.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            File.Delete("fixture.db");
        }

        /// <summary>
        /// Returns a SQLite Connection String
        /// </summary>
        /// <returns>Connection string</returns>
        public override string GetConnectionString()
        {
            var builder = new SqliteConnectionStringBuilder();

            builder.DataSource = "fixture.db";

            return builder.ToString();
        }

        /// <summary>
        /// Returns a single string
        /// </summary>
        /// <returns>String from resources</returns>
        protected override IEnumerable<string> GetFixturesSQLOrders()
        {
            return new string[] { Properties.Resources.CreateFixtures };
        }

        /// <summary>
        /// Returns a single string
        /// </summary>
        /// <returns>String from resources</returns>
        protected override IEnumerable<string> GetRemoveFixturesSQLOrders()
        {
            return new string[] { Properties.Resources.RemoveFixtures };
        }
    }

#pragma warning disable SA1402
    /// <summary>
    /// xUnit fixture collection. See https://xunit.github.io/docs/shared-context#collection-fixture
    /// </summary>
    [CollectionDefinition(nameof(SQLiteFixtureCollection))]
    public class SQLiteFixtureCollection : ICollectionFixture<SQLiteFixture>
    {
    }
#pragma warning restore SA1402
}
