// <copyright file="DatabaseFixture{T}.cs" company="Weeger Jean-Marc">
// Copyright Weeger Jean-Marc under MIT Licence. See https://opensource.org/licenses/mit-license.php.
// </copyright>

namespace Jmw.XUnitHelpers
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Helper class to provide fixture in a database.
    /// </summary>
    /// <remarks>
    /// <para>When the object constructs, it connects to the database, execute a series of SQL orders.</para>
    /// <para>When the objet is disposed, a second serie of SQL cleans-up the database.</para>
    /// </remarks>
    /// <typeparam name="T">Connection type (for instance MySQLConnection)</typeparam>
    public abstract class DatabaseFixture<T> : IDisposable
        where T : System.Data.IDbConnection, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseFixture{T}"/> class.
        /// Constructeur
        /// </summary>
        public DatabaseFixture()
        {
            this.Execute(this.GetFixturesSQLOrders());
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
            this.Execute(this.GetRemoveFixturesSQLOrders());
        }

        /// <summary>
        /// Function you need to implement. it should returns the connection string to the database.
        /// </summary>
        /// <returns>Connection String</returns>
        public abstract string GetConnectionString();

        /// <summary>
        /// Function you need to implement. It should returns a list of SQL orders to execute to create database fixtures.
        /// </summary>
        /// <returns>Enumeration of strings</returns>
        protected abstract IEnumerable<string> GetFixturesSQLOrders();

        /// <summary>
        /// Function you need to implement. It should returns a list of SQL orders to execute to remove database fixtures.
        /// </summary>
        /// <returns>Enumeration of strings</returns>
        protected abstract IEnumerable<string> GetRemoveFixturesSQLOrders();

        private void Execute(IEnumerable<string> data)
        {
            using (T connection = new T())
            {
                connection.ConnectionString = this.GetConnectionString();
                connection.Open();

                var trans = connection.BeginTransaction();

                var command = connection.CreateCommand();
                command.CommandType = System.Data.CommandType.Text;

                foreach (string sql in data)
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }

                trans.Commit();
            }
        }
    }
}
