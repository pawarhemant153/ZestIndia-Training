using Dapper;
using Day15_DapperNHibernate.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Day15_DapperNHibernate.Repositories
{
    public class EmployeeRepository
    {
        private readonly IConfiguration _configuration;
        public EmployeeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection Connection
        {
            get
            {
                return new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection"));
            }
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            string query = "SELECT *FROM Employees";

            using(var connection = Connection)
            {
                return await connection.QueryAsync<Employee>(query);
            }
        }
        public async Task<Employee?> GetEmployeeById(int id)
        {
            string query = "SELECT *FROM Employees WHERE Id = @Id";

            using(var connection = Connection)
            {
                return await connection.QueryFirstOrDefaultAsync<Employee>(
                    query,
                    new {Id = id});
            }
        }

        public async Task<int> AddEmployee(Employee employee)
        {
            string query = @"INSERT INTO Employees(Name, Department, Salary)
                            VALUES(@Name, @Department, @Salary)";

            using(var connection = Connection)
            {
                return await connection.ExecuteAsync(query, employee);
            }
        }

        public async Task<int> UpdateEmployee(Employee employee)
        {
            string query = @"UPDATE Employees
                            SET Name = @Name,
                                Department =@Department,
                                Salary = @Salary
                            WHERE Id = @Id";

            using(var connection = Connection)
            {
                return await Connection.ExecuteAsync(query, employee);
            }
        }
        public async Task<int> DeleteEmployee(int id)
        {
            string query = "DELETE FROM Employees WHERE Id = @Id";

            using (var connection = Connection)
            {
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
