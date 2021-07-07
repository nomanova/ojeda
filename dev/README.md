# Development

The development dependencies are available as docker containers, managed through the docker compose file in this folder.

```
$ docker compose --project-name ojeda up
```

## Database (PostgreSQL)

The database server will be exposed at localhost:5432, with the following settings:

- Database: ojeda
- Username: ojeda
- Password: ojeda

Resulting in the following connection string:

```
"User ID=ojeda;Password=ojeda;Host=localhost;Port=5432;Database=ojeda;Pooling=true;"
```

## SMTP Server

Mailhog (https://github.com/mailhog/MailHog) is used to locally simulate an SMTP server for sending transactional mails.
The SMTP server will be exposed at localhost:1025, while the email client is available at http://localhost:8025.
