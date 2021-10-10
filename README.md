Example Project with Boring Business Case
====

This repository contains example repository that demonstrates TDD, BDD and some other guidelines that I like to follow.
The boring side is to create a simple application to give users movie recommendations based on their genre preference.

- I have chosen C# and .NET core 3.1 since currently I have been working on that stack and know the current state of libraries that I like to work with.

- REST API has been implemented using ASP.NET. I also provided additional API endpoints for liking and disliking a movie since it makes sense that user can like and dislike movies in order to get recommendations. The API uses access tokens for user authentication. Please see the complete API description below.

- It is assumed that the requirement is not to use external movie databases simultaneously (instead, provide a clear interface that allows changing the backend). If that is the case, we could do it by making a facade implementation for Netflix and IMDB and then call both services in method implementations. For example, `GetMoviesByGenres` would call Netflix's and IMDB's version of the method and then remove duplicated movies. However, this raises another problem; when there are duplicated results, which data should be used?


FAQ
===

- What kind of architecture would you use to achieve this Boring Business Case?
  - Since movies are something that can be watched and are categorized to genres, it is natural to have a REST API that talks about movies instead of genres directly. Users can watch movies and they can like or dislike them. And since each movie has one or multiple genres, we can infer the genres that the user likes this way. Then we can use some external movie database to search movies that share the genres with the movies that the user has liked already.

- How would you design the application for testability?
  - I have been following these steps to achieve testability:
    1. Have a concrete and exact specification of use cases. Please see the use cases below in Gherkin.
    2. Do the minimum to only implement the required use cases, and nothing else.
    3. Use dependency injection and loose coupling.
    4. Prefer interfaces.
    5. Do not duplicate code.
    6. Do not use hidden states.
    7. Avoid using global data.
    8. Have tests in multiple abstraction levels to make debugging easier and let you to pin-point the problems more easily. In this application, there are unit and integration tests. Unit tests test the code in method level and integration tests test the code in API level.
    9. Make your application easy to run. I like Docker and docker-compose since that can pack the whole application life-cycle into two files. There is a `Dockerfile` and `docker-compose.yml` files that define exactly how the application is built and how to run unit and integration tests. Basically, you can get the application up and running with just one command.
    10. Have a documentation about the design, implementation and something for users that they know how to use your application. You are reading that now!

- How would you take persistence into account in the design?
  - I have already implemented `IUnitOfWork` interface that contains all the data models used by the application. The unit of work instance gets disposed after every HTTP request which means that in persistence mode, it could make a transaction of all changed data and commit that into database. It is easy to implement the interface and make it to use Entity Framework database context, similarly to `InMemoryUnitOfWork` and `InMemoryDataContext`. Then the implementation can be injected in the place of `InMemoryUnitOfWork` and that's it.

- If you later decided to migrate to a microservices architecture, what kind of patterns would you use for this?
  - I think the best way would be to separate external services, application APIs and possible database to their own microservices. This way we could scale each service individually based on the need and maybe let other teams to have responsibility of each service. There are multiple ways to actually implement it, but I personally prefer Docker a lot since it makes building, separating and running services very easy. In a cloud environment, there are built-in PaaS services that let you to ship the code and run it, such as Azure Functions and Lambdas in AWS. However, I would be very cautious about these since their pricing can surprise if there are service traffic peaks. In addition, their run environments can change without you noticing. Docker solves these problems since it packs the whole run environment with the application. Fortunately, there are also PaaS services (like Azure AKS and Amazon EKS) that can provision virtual machines automatically which are capable of running Docker. This way, you know exactly that your application will always work and know exactly how much it costs.


REST API description
===

- `/movie/like/{movie id}` (POST) likes a movie that has an ID of `movie id`.

  Request headers:
  - `Authorization: Bearer {token}` (required) set an access token of `token` that authorizes the user.

- `/movie/dislike/{movie id}` (POST) dislikes a movie that has an ID of `movie id`.

  Request headers:
  - `Authorization: Bearer {token}` (required) set an access token of `token` that authorizes the user.

- `/movie/recommend` (GET) gets movie recommendations based on genres of user's liked movies.

  Request headers:
  - `Authorization: Bearer {token}` (required) set an access token of `token` that authorizes the user.

  Query parameters:
  - `limit` (required) limits how many movie recommendations are returned.
  - `offset` (optional) sets movie recommendation offset to enable pagination. If not given, set to zero.

- `/user/token/{user id}` (GET) generates an access token for user with an ID of `user id`.


You can use `/user/token/{user id}` endpoint to generate an access token for user with an ID of `user id`. This will generate an access token without an expiration date. I have already generated an access token for _Dummy user_ with an ID of 1. Feel free to use it if you like: `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJEdW1teSB1c2VyIiwidXNlcklkIjoiMSIsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTAwMCJ9.XZ0FhaeawcbnKLsyOUbSSRSU8DkEB6Mf2PsfJ3NQatw
`

Use cases
===

Here is a list of the use cases that the application consist of. Each use case has a matching BDD style integration test.

**Scenario: Generate access token**\
**When** user requests an access token**\
**Then** generate a JWT token

**Scenario: Like a movie**\
**Given** user has an access token\
**When** user likes a movie\
**Then** add the liked movie to the user preference

**Scenario: Dislike a movie**\
**Given** user has an access token\
  **And** user has liked the movie\
**When** user dislikes a movie\
**Then** remove the liked movie from the user preference

**Scenario: Get movie recommendations for user based on genre preference**\
**Given** user has an access token\
**When** user requests more movie recommendations\
**Then** extract unique genres from user's liked movies\
 **And** query next chunk of movies from a movie library\
 **And** list a movie if the movie's genres contain any of the extracted genres\
 **And** filter out the movies that user has liked already

**Scenario: Get paginated movies from movie library**\
**Given** user has an access token\
**When** user scrolls down the movie list\
**Then** query next chunk of movie results from movie library

**Scenario: Get user preference movies**\
**Given** user has liked some movies\
**When** user queries which movies are their preference\
**Then** create a subset of given movies to contain only ones that user has liked

Development & Usage
===

There are two ways to run and develop the application:
- You need Docker and docker-compose. Please refer to your platform's documentation on how to install them. Then you can run the application by commanding:
  - `docker-compose up app`

  It will build and test the application if not already built. You can then access the application in http://localhost:5000. When developing, you can run unit tests by commanding:
  - `docker-compose run --rm unit-test`

  Similarly, integration tests can be ran by commanding:
  - `docker-compose run --rm integration-test`

- Alternatively, you can just use Visual Studio 2019 with .NET core and ASP.NET toolkits. Open a solution file and then you can start the application by pressing F5. However, make sure that the target is `tech.haamu.Movie` which uses Kestrel standalone web server and not IIS. You can run unit and integration tests with a Test Explorer.
