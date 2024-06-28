# MovieRecommendation

MovieRecommendation is a simple web API application that provides movie recommendations to users. This project is built using .NET Core and periodically fetches movie data from themoviedb.org. The fetched data is periodically saved to the database or updated.

## Features
+ User authentication and authorization (JWT Bearer Authentication and Auth0)
+ Fetching and storing movie data
+ Providing movie recommendations to users

## Installation 
### Requirements
* .NET Core SDK (.NET 8.0)
* PostgreSQL or a suitable database server
* Auth0 account
* themoviedb.org API key

## API Overview

### Authentication
The API requires authentication. Auth0 or a similar authentication service should be used. Configure your Auth0 settings in the appsettings.json file. Here is an example how appsettings.json file looks like. Don't forget to ignore this file on gitignore. (You may use user-secrets as well)
```
{
  "ConnectionStrings:PostgreSQL": "Host= YourHost;Database=YourDatabase;User Id=YourId;Password=YourPassword;",


  "Auth0": {
    "Domain": "",
    "Audience": "",
    "ClientId": "",
    "ClientSecret": "",
    "Authority": ""
  },

  "Tmdb": {
    "ApiKey": "",
    "AccessToken": "",
    "PageSize": 20
  },

  "IpRateLimiting": {
    "EnableEndpointRateLimiting": true,
    "StackBlockedRequests": false,
    "RealIpHeader": "X-Real-IP",
    "ClientIdHeader": "X-ClientId",
    "HttpStatusCode": 429,
    "GeneralRules": [
      {
        "Endpoint": "*",
        "Period": "1m",
        "Limit": 5
      }
    ]
  }
}
```

## User Scenarios

### List Movies:
Endpoint: /api/movies?page={pageNumber}&pageSize={pageSize}
Description: Users can retrieve a paginated list of movies. The page size can be specified as a parameter.

### Add Note and Rating to a Movie:
Endpoint: /api/movies/{movieId}/note
Description: Users can add a note (text) and a rating (1-10) to a selected movie.
Request Body:
```
{
  "note": "Great movie!",
  "rating": 8
}
```
### View Movie Details:
Endpoint: /api/movies/{movieId}
Description: Users can view detailed information about a movie, including movie details, average rating, user's rating, and added notes.

### Recommend a Movie:
Endpoint: /api/movies/{movieId}/recommend
Description: Users can send a movie recommendation to a given email address.
Request Body:

```
{
  "recipientemail": "example@example.com"
}
```
