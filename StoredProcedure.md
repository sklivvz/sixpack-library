http://sites.google.com/site/sixpacklibrary/_/rsrc/1237036619924/Home/sixpack-icon.gif?height=48&width=48
# Executing Stored Procedures #

The easiest way to use the `StoredProcedure` class is this:
```
DataSet ds = new StoredProcedure("myStoredProcName")
                 .AddParameter("@first", 42, DbType.Int, 0)
                 .AddParameter("@second", "hello", DbType.String, 50)
                 .GetDataSet();
```

This example will execute a stored procedure on the database defined by your first connection string, passing two parameters and returning a `DataSet`.

## _void_ Stored Procedures ##
You can execute a stored procedure that does not return anything by using the `Execute` method instead of the `GetDataSet` method.
```
new StoredProcedure("myStoredProcName")
    .AddParameter("@first", 42, DbType.Int, 0)
    .AddParameter("@second", "hello", DbType.String, 50)
    .Execute();
```

## _scalar_ Stored Procedures ##
You can execute a stored procedure that returns a scalar by using the `Execute` method instead of the `GetDataSet` method.
```
object scalar = new StoredProcedure("myStoredProcName")
                    .AddParameter("@first", 42, DbType.Int, 0)
                    .AddParameter("@second", "hello", DbType.String, 50)
                    .ExecuteScalar();
```

## Specifying a connection ##
You can specify which connection string to use by passing the connection string name in the constructor:
```
DataSet ds = new StoredProcedure("myStoredProcName", "myConnection")
                 .AddParameter("@first", 42, DbType.Int, 0)
                 .AddParameter("@second", "hello", DbType.String, 50)
                 .GetDataSet();
```

## Deadlocks ##
If two stored procedure invocations lock each other inside transactions, the RDBMS will kill one of them to unlock the other. When this happens, it is correct behaviour to resubmit the killed stored procedure.
The `StoredProcedure` class takes care of this scenario, but only if enabled. To enable deadlock resubmission, include this key in your `.config` file.
```
<configuration>
  <appSettings>
    <!-- This will let the StoreProcedure class 
         retry submitting deadlock victims 5 times. -->
    <add name="StoredProcedureDeadlockRetries" value="5" />
  </appSettings>
</configuration>
```