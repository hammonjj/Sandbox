# Equi "Wallet" Coding Assignment

## Getting Started

To make things easier to get started quickly, we've set everything up in docker. That said, you will need to have docker available on your system. Then to start the database (postgres), Express api, and React web app just run:

`docker compose up`



_NOTE_: `node_modules` will *NOT* be populated on your filesystem after `docker compose up` (it will exist only in the docker virtualized filesystem).  That's fine as far as running the apps go, but it may confuse your IDE. To make your IDE happy you shuold run `npm install` in each of the `api` and `webapp` folders to create a local `node_modules`.


---

## Overview

### Database
This environment uses a Postgres database. We interact with the database via Prisma ORM. Prisma handles our schema, migrations, data interactions, and types.

Running migrations is covered in further detail within `packages/api/README`

### API
The API an Express server. There is a stock example for the `user` model available for use and reference with route, controller, service, dto, etc. You don't have to necessarily use this model, it's there as a demonstrator of a pattern.

### Webapp
This is a basic React app (via create-react-app for simplicity). As the exercise instructions point out, we are not scoring on elegant the front end is, so things are pretty bare bones here. We do however include an example for making various requests to the API utilizing axios + react-query.

---

## Adding a dependency:

If you choose to add any dependencies, you will need to do the following to ensure they are available in your containerized apps.

```
cd packages/<API OR WEBAPP>
npm i <DEPENDENCY>
docker compose down
docker compose up --build
```