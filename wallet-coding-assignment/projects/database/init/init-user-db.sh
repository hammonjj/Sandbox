#!/bin/bash
set -e

psql -v --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE USER equi;
    CREATE DATABASE equi;
    GRANT ALL PRIVILEGES ON DATABASE equi TO equi;
EOSQL
