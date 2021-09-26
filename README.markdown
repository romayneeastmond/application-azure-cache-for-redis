# StackExchange.Redis Caching Local Database Objects

This project uses StackExchange.Redis to store objects in an Azure Cache for Redis cache.

## Azure Cache for Redis

The solution requires the settings of a provisioned Azure Cache for Redis instance or it will simply display the output from the local database.

## How to Use

Restore any necessary NuGet packages before building or deploying. Ensure that the settings in the CacheSecrets.config file are changed to match the Redis endpoint and access key. The Web.config file needs a reference to CacheSecrets.Development.config changed to CacheSecrets.config.

## Copyright and Ownership

All terms used are copyright to their original authors.