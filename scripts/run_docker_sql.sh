#!/bin/sh

docker run \
    -p 1433:1433 \
    -e 'ACCEPT_EULA=Y' \
    -e 'MSSQL_SA_PASSWORD=f66c5b93-1987-4547-aa89-d75c30017b0f' \
    --name sky_hawk_sql \
    -d "mcr.microsoft.com/mssql/server:2022-CU9-ubuntu-20.04"

# I'm using mcr.microsoft.com/mssql/server:latest image from before 4 months
# My best bet is that it's either :2022-CU10-ubuntu-20.04 or :2022-CU9-ubuntu-20.04
