# Running this server
The API should boot via `docker compose up`.

# Changing the DB Schema
See the prisma docs (https://www.prisma.io/docs/getting-started/setup-prisma/add-to-existing-project/relational-databases/evolve-your-schema-typescript-postgres)

Outside of docker, run `npm run prisma:migrate` which should prompt you to name your migration.

You'll then want to restart docker to pickup the changes
```
docker compose down
docker compose up --build
```