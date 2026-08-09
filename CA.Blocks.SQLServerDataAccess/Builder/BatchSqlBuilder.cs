using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;

namespace CA.Blocks.SQLServerDataAccess.Builder;

/// <summary>
/// Provides a fluid builder for constructing parameterized dynamic SQL statements 
/// alongside their corresponding <see cref="SqlParameter"/> objects to prevent SQL injection.
/// </summary>
public class BatchSqlBuilder()
{
    /// <summary>
    /// Internal string buffer used to accumulate the generated SQL query text.
    /// </summary>
    private readonly StringBuilder _sb = new();

    /// <summary>
    /// Internal collection storing all SQL parameters generated during query construction.
    /// </summary>
    private readonly List<SqlParameter> _parameters = [];

    /// <summary>
    /// Appends a raw SQL text fragment to the SQL buffer without adding a new line.
    /// </summary>
    /// <param name="text">The raw SQL fragment to append.</param>
    public void AddSqlFragment(string text)
    {
        _sb.Append(text);
    }

    /// <summary>
    /// Appends an opening parenthesis <c>(</c> to the SQL buffer.
    /// </summary>
    public void AddSqlOpenBracket()
    {
        AddSqlFragment("(");
    }

    /// <summary>
    /// Appends a closing parenthesis <c>)</c> to the SQL buffer.
    /// </summary>
    public void AddSqlCloseBracket()
    {
        AddSqlFragment(")");
    }

    /// <summary>
    /// Appends a comma and space <c>, </c> to the SQL buffer.
    /// </summary>
    public void AddSqlComma()
    {
        AddSqlFragment(", ");
    }

    /// <summary>
    /// Appends specified text followed by a line terminator to the SQL buffer.
    /// </summary>
    /// <param name="text">The optional text to write before the line terminator.</param>
    public void AddLine(string text = "")
    {
        _sb.AppendLine(text);
    }

    /// <summary>
    /// Generates a parameter using the specified factory delegate, registers it to the parameter collection, 
    /// and appends the parameter name into the SQL buffer.
    /// </summary>
    /// <param name="toSqlParamFunc">A factory delegate that accepts the parameter name and returns a <see cref="SqlParameter"/>.</param>
    /// <param name="name">The name of the parameter (e.g., "@ParamName").</param>
    public void AddParameter(Func<string, SqlParameter> toSqlParamFunc, string name)
    {
        _parameters.Add(toSqlParamFunc(name));
        _sb.Append(name);
    }

    /// <summary>
    /// Generates an indexed parameter name (formatted as <c>{name}_{index}</c>), creates the <see cref="SqlParameter"/> 
    /// via the provided factory, registers it, and appends the generated name into the SQL buffer.
    /// </summary>
    /// <param name="toSqlParamFunc">A factory delegate that accepts the indexed parameter name and returns a <see cref="SqlParameter"/>.</param>
    /// <param name="name">The base parameter name prefix (e.g., "@Id").</param>
    /// <param name="index">The zero-based index to append to the parameter name.</param>
    public void AddIndexedParameter(Func<string, SqlParameter> toSqlParamFunc, string name, int index)
    {
        var paramName = $"{name}_{index}";
        _parameters.Add(toSqlParamFunc(paramName));
        _sb.Append(paramName);
    }

    /// <summary>
    /// Returns the complete generated SQL command text.
    /// </summary>
    /// <returns>A string containing the compiled SQL query.</returns>
    public string GetSqlStatement()
    {
        return _sb.ToString();
    }

    /// <summary>
    /// Gets the list of <see cref="SqlParameter"/> objects generated during query construction.
    /// </summary>
    /// <returns>A read-only or mutable list of parameters ready for execution against a <see cref="SqlCommand"/>.</returns>
    public IList<SqlParameter> GetParameters()
    {
        return _parameters;
    }
}

