# Consumption service

Manages consumption entries.

## Configuration

Available configuration options:

```json
{
  "Database": {
    "Type": "database-type-here"
  },
  "ValidationOptions": {
    "DateTimeLeewayMinutes": 5
  }
}
```

or:

```env
Database__Type=database-type-here
ValidationOptions__DateTimeLeewayMinutes=5
```

Description:

| Config key                              | Default | Required | Description                                                                                                 |
|-----------------------------------------|---------|----------|-------------------------------------------------------------------------------------------------------------|
| Database:Type                           | null    | no       | The type of database to use. When no value is provided, an in-memory database is used.                      |
| ValidationOptions:DateTimeLeewayMinutes | 5       | no       | The number of minutes to allow for the the dateTime of consumption entries to be ahead of the current time. |

